using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.WebApp.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace MK.Easydoc.WebApp.Areas.Documento.Controllers
{
    public class DigitacaoController : BaseController
    {


        #region Private Read-Only Fields

        private readonly ILoteService _loteService;
        private readonly IDocumentoService _docService;

        #endregion


        public DigitacaoController()
        {
            _loteService = DependencyResolver.Current.GetService<ILoteService>();
            _docService = DependencyResolver.Current.GetService<IDocumentoService>();
        }


        //
        // GET: /Documento/Digitacao/

        public ActionResult Tipificar()
        {
            return View();
        }


        //public ActionResult Digitar()
        //{

        //    MK.Easydoc.Core.Entities.Documento _documento = new MK.Easydoc.Core.Entities.Documento();
        //    _documento = _docService.GetDocumentoDigitar(UsuarioAtual.ID, ServicoAtual.ID);
        //    return View(_documento);
        //}

        public ActionResult Digitar(int id)
        {

            bool UsaArquivoDados = _loteService.UsaArquivoDados(ServicoAtual.ID);
            bool EmUso;


            
            
            MK.Easydoc.Core.Entities.Documento _documento = new Core.Entities.Documento();
            if (id==0)
            {
                _documento = _docService.GetDocumentoDigitar(UsuarioAtual.ID, ServicoAtual.ID);
                EmUso = _docService.EmUso(_documento.ID, UsuarioAtual.ID, 1);

                if (EmUso) 
                { 
                    _documento.ID = 0; 
                    //return View(_documento); 
                }

              
            }else
            {
                _documento = _docService.SelecionaDocumentoDigitar(UsuarioAtual.ID, ServicoAtual.ID,id);
            }

            List<Motivo> _motivo;
            ViewData["dupliciadade"] = "";
            if (_documento.StatusDocumento == 1000)
            {
                _motivo = _docService.GetMotivoDigitar(_documento.Modelo.ID, 1);
            }
            else
            {
                _motivo = _docService.GetMotivoDigitar(_documento.Modelo.ID, 2);
                ViewData["dupliciadade"] = "";
                if (_documento.Modelo.ID == 10)
                {                    
                    ViewData["dupliciadade"] = _docService.GetDuplicidade(id);
                }
            }
                        

            //------------------------------------
            if (_documento.ID>0)
            { 
                DocumentoLoteViewModel _documentoView = new DocumentoLoteViewModel();
                _documentoView.Documento = _documento;
                _documentoView.CamposDocumentoJSON = _docService.PesquisarDocumentos(ServicoAtual.ID, _documento.Modelo.ID, "Caixa, Lote", string.Format("IdDocumento={0}", _documento.ID));

                JavaScriptSerializer jss = new JavaScriptSerializer();
                List<DocumentoDetalhe> _docs = jss.Deserialize<List<DocumentoDetalhe>>(_documentoView.CamposDocumentoJSON).ToList<DocumentoDetalhe>();

                if (_documento.StatusDocumento == 1010)
                {
                    ViewData["breadonly"] = "true";
                }
                else
                {
                    ViewData["breadonly"] = "false";
                }

                string status="";

                switch (_documento.StatusDocumento)
                {
                    case 1000:
                        status = "A digitar";
                        break;
                    case 1020:
                        status = "Supervisão";
                        break;
                    case 1029:
                        status = "Excluído";
                        break;
                    case 1010:
                        status = "Digitado";
                        break;
                    default:
                        status = "";
                        break;
                }
                

                string det;
                ViewData["Det"] = "";
                foreach (DocumentoDetalhe detalhe in _docs)            
                {
                    if (_documento.StatusDocumento == 1000 || _documento.StatusDocumento == 1010)
                    {
                        if (UsaArquivoDados) { ViewData["Det"] = string.Format( status +" | Caixa: {0} | Lote: {1} ", detalhe.Caixa, detalhe.Lote ); }                        
                    }
                    else
                    {
                        det = _docService.PesquisarMotivo(detalhe.IdDocumento);
                        if (UsaArquivoDados)
                        {
                            ViewData["Det"] = string.Format(status + "| Caixa: {0} | Lote: {1} | Motivo: {2}", detalhe.Caixa, detalhe.Lote, det);
                        }
                        else
                        {
                            ViewData["Det"] = string.Format(status + "| Motivo: {0}", det);
                        }                        
                    }
                }
            }
            ViewData["Just"] = "";
            ViewData["Valida"] = "";
            ViewData["Just"] = "<table>";
            ViewData["Just"] = ViewData["Just"] + "<tr><td>Cod.<td>Descrição</td></tr> ";
            foreach (Motivo motivo in _motivo)
            {

                ViewData["Just"] = ViewData["Just"] + "<tr><td><li>" + motivo.atalho + "<td> " + motivo.descricao +"</td></tr>";
                ViewData["Valida"] = ViewData["Valida"] + motivo.atalho.ToString();          
            }
            ViewData["Just"] = ViewData["Just"] + "</table>";

            // Se o ID passado como parametr for diferente do retornado pelo metodo, direciona poara sup... 
          
            if (_documento.StatusDocumento!=1010)
            { 
                if (id != 0 && _documento.ID==0 )
                {
                    return RedirectToAction("ListarPendentes", new { area = "Documento", controller = "Supervisao" });
                }
            }
            return View(_documento);
        }


        public ActionResult LotesPendente()
        {
            return View();
        }

        //
        // GET: /Documento/Digitacao/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AjaxCallEnviarDocumentoSupervisao(int id_documento,int id_motivo)       
        {
            try
            {
                MK.Easydoc.Core.Entities.Documento _doc = (new Core.Entities.Documento {ID=id_documento,StatusDocumento=1020});
                _docService.AtualizarDocumento(_doc);
                bool EmUso = _docService.EmUso(id_documento, UsuarioAtual.ID, 2);
                _docService.IncluirMotivo(id_documento, id_motivo, ServicoAtual.ID,1);

            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, "Documento enviado para a supervisão!", null));
        }

        [HttpPost]
        public ActionResult ajax_Aprovar(int id_documento)
        {
            try
            {
                string _msg = string.Empty;
                _msg = string.Empty;

                int id_documentoModelo=0;

                if (ServicoAtual.ID == 1) { id_documentoModelo=1; }
                if (ServicoAtual.ID == 2) { id_documentoModelo = 10; }


                System.Threading.Thread.Sleep(1000);

                _msg = _docService.ValidarDocumento(id_documento, id_documentoModelo, ServicoAtual.ID);
                if (!string.IsNullOrEmpty(_msg))
                {
                    return Json(new RetornoViewModel(false, _msg));                    
                }
                

                MK.Easydoc.Core.Entities.Documento _doc = (new Core.Entities.Documento { ID = id_documento, StatusDocumento = 1010 });
                _docService.AtualizarDocumento(_doc);
                bool EmUso = _docService.EmUso(id_documento, UsuarioAtual.ID, 2);
                //_docService.IncluirMotivo(id_documento, id_motivo, ServicoAtual.ID, 1);

                return Json(new RetornoViewModel(true));

            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, "Documento enviado para a supervisão!", null));
        }



        [HttpPost]
        public ActionResult AjaxCallDigitarDocumento(int id_documento_modelo, string documento_digitado)
        {
            //var tipificar = default(TipificarViewModel);
            //var lote = default(Lote);
            try
            {
                DocumentoDigitacaoViewModel _campoModelo = new DocumentoDigitacaoViewModel();
                _campoModelo = ConverteJSONCampoModelo(documento_digitado);

                string idstatus = _docService.GetStatusDocumento(_campoModelo.Campos[0].IndexDoc);
                string _msg = string.Empty;
                
                foreach (CampoModelo _campo in _campoModelo.Campos)
                {
                    _docService.AtualizarDocumentoCampo(_campo);                
                }
                               
                
                foreach (CampoModelo _campo in _campoModelo.Campos)
                {
                    _msg = _docService.ValidarCamposDocumento(id_documento_modelo, _campo);
                    if (!string.IsNullOrEmpty(_msg))
                           return Json(new RetornoViewModel(false, _msg));                    
                }
                
                _msg = string.Empty;
                _msg = _docService.ValidarDocumento(_campoModelo.IdDocumento, id_documento_modelo, ServicoAtual.ID);

                if (!string.IsNullOrEmpty(_msg))
                    return Json(new RetornoViewModel(false, _msg));

                bool EmUso = _docService.EmUso(_campoModelo.IdDocumento, UsuarioAtual.ID, 2);
                _docService.FinalizarDigitacao(_campoModelo.IdDocumento);
 
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, "Documento digitado com sucesso!", null));
        }

        //[HttpPost]
        //public ActionResult AjaxCallDigitarDocumentoAprovacao(int id_documento_modelo, string documento_digitado)
        //{
        //    //var tipificar = default(TipificarViewModel);
        //    //var lote = default(Lote);
        //    try
        //    {
        //        DocumentoDigitacaoViewModel _campoModelo = new DocumentoDigitacaoViewModel();
        //        _campoModelo = ConverteJSONCampoModelo(documento_digitado);

        //        //CampoModelo _campo = new CampoModelo();

        //        string idstatus = _docService.GetStatusDocumento(_campoModelo.Campos[0].IndexDoc);

        //        string _msg = string.Empty;
        //        foreach (CampoModelo _campo in _campoModelo.Campos)
        //        {
        //            _msg = _docService.ValidarCamposDocumento(id_documento_modelo, _campo);

        //            //if (idstatus != "1020")
        //            //{
        //            //    if (!string.IsNullOrEmpty(_msg))
        //            //        return Json(new RetornoViewModel(false, _msg));
        //            //}
        //        }

        //        foreach (CampoModelo _campo in _campoModelo.Campos)
        //        {
        //            _docService.AtualizarDocumentoCampo(_campo);
        //            //_docService
        //        }


        //        //if (idstatus != "1020")
        //        //{
        //        //    _msg = string.Empty;
        //        //    _msg = _docService.ValidarDocumento(_campoModelo.IdDocumento, id_documento_modelo, ServicoAtual.ID);

        //        //    if (!string.IsNullOrEmpty(_msg))
        //        //        return Json(new RetornoViewModel(false, _msg));
        //        //}

        //        _docService.FinalizarDigitacao(_campoModelo.IdDocumento);

        //    }
        //    catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
        //    return Json(new RetornoViewModel(true, "Documento digitado com sucesso!", null));
        //}




        [HttpPost]
        public ActionResult AjaxCallExcuirDocumento(int id_documento,int id_motivo )
        {
            //var tipificar = default(TipificarViewModel);
            //var lote = default(Lote);
            try
            {
                //MK.Easydoc.Core.Entities.Documento _doc = (new Core.Entities.Documento { ID = id_documento, StatusDocumento = 1020 });
                _docService.ExcluirDocumento(id_documento);
                _docService.IncluirMotivo(id_documento, id_motivo, ServicoAtual.ID,2);

            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, "Documento excluido com sucesso!", null));
        }

        [HttpPost]
        public ActionResult AjaxCallValidaDuplicidade(int id_documento, int id_)
        {
            
            try
            {

                _docService.IncluirMotivo(id_documento, id_, ServicoAtual.ID, 1);
                if (id_ == 8)
                {
                    _docService.MudaStatusDocumento(id_documento, ServicoAtual.ID,1020);                                        
                }
                else                
                {
                    string idDocumentoModelo = _docService.GetDocumentoModelo(id_documento);

                    if (idDocumentoModelo == "10")
                    {
                        _docService.Executar("update valida_doc10_serv02 set iddocumento="+id_documento+", DtVinculo=Getdate() where idvalida_doc10_serv02="+id_);
                    }
                    _docService.FinalizarDigitacao(id_documento);                    
                }
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, "Documento Alterado!", null));
            
        }
        
        private DocumentoDigitacaoViewModel ConverteJSONCampoModelo(string _doctoJSON)
        {

            DocumentoDigitacaoViewModel _documentoDigitacao = new DocumentoDigitacaoViewModel();
            JObject _docto = JObject.Parse(_doctoJSON);
            
            JObject jsonObj = new JObject();

            var jsonData = default(string);
            jsonData = JsonConvert.SerializeObject(_docto["Documento"]);

            _documentoDigitacao = (DocumentoDigitacaoViewModel)JsonConvert.DeserializeObject<DocumentoDigitacaoViewModel>(jsonData);

            //IList<JToken> docto = _docto["Documento"].Children().ToList();

            IList<JToken> _camposJSON = _docto["Documento"]["Campos"].Children().ToList();
            //IList<CampoModelo> _campos = new List<CampoModelo>();

            foreach (JToken result in _camposJSON)
            {
                CampoModelo campo = JsonConvert.DeserializeObject<CampoModelo>(result.ToString());
                _documentoDigitacao.Campos.Add(campo);
            }

            return _documentoDigitacao;
        }

       


    }
}
