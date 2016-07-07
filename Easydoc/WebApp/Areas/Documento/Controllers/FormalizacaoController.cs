using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.WebApp.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;


namespace MK.Easydoc.WebApp.Areas.Documento.Controllers
{
    public class FormalizacaoController : BaseController
    {
        
        #region Private Read-Only Fields

        private readonly ILoteService _loteService;
        private readonly IDocumentoService _docService;

        #endregion
        public FormalizacaoController() //Método construtor da classe FormalizacaoController
        {
            _loteService = DependencyResolver.Current.GetService<ILoteService>();
            _docService = DependencyResolver.Current.GetService<IDocumentoService>();
        }

        [HttpPost]
        public JsonResult AjaxGravarFormalizacaoPorPergunta(string idDocumento, string idFormalizacao, string valor )
        {
            var _frm = new Formalizacao()
            {
                IdServico = IdServico_Atual
                ,
                IdDocumento = int.Parse(idDocumento)
                ,
                IdFormalizacao = int.Parse(idFormalizacao)
                ,
                Valor = int.Parse(valor)
            };
            var _formalizacao = new DocumentoRepository().GravarFormalizacao(_frm);
            return Json(_formalizacao, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AjaxMudaStatusDocumento(string idDocumento)
        {
            var ret = new Retorno();
            try
            {
                _docService.MudaStatusDocumento(int.Parse(idDocumento), UsuarioAtual.ID, 2010, ServicoAtual.ID);
                bool EmUso = _docService.EmUso(int.Parse(idDocumento), UsuarioAtual.ID, 2, ServicoAtual.ID);
                                


            }
            catch (Exception erx)
            {
                ret.CodigoRetorno = 1;
                ret.Mensagem = erx.Message;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AjaxVoltarDocumentoEmUso(string idDocumento)
        {
            var ret = new Retorno();
            try
            {
                bool EmUso = _docService.EmUso(int.Parse(idDocumento), UsuarioAtual.ID, 2, ServicoAtual.ID);
            }
            catch (Exception erx)
            {
                ret.CodigoRetorno=1;
                ret.Mensagem = erx.Message;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AjaxListaOcorrencias(int idServico)
        {
            var _ocor = new DocumentoRepository().ListaOcorrencia(idServico,1);
            return Json(_ocor.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Digitar(int id)
        {
            //RegistrarLOGSimples(4, 14, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo de formalizacao

            bool UsaArquivoDados = _loteService.UsaArquivoDados(ServicoAtual.ID);
            bool EmUso;

            MK.Easydoc.Core.Entities.Documento _documento = new Core.Entities.Documento();
            if (id == 0)
            {
                _documento = _docService.GetDocumentoFormalizar(UsuarioAtual.ID, ServicoAtual.ID);
                EmUso = _docService.EmUso(_documento.ID, UsuarioAtual.ID, 1, ServicoAtual.ID);

                if (EmUso)
                {
                    _documento.ID = 0;
                    //return View(_documento); 
                }
            }
            else
            {
                _documento = _docService.SelecionaDocumentoDigitar(UsuarioAtual.ID, ServicoAtual.ID, id);
            }

            List<Ocorrencia> _motivo;
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
                    ViewData["dupliciadade"] = _docService.GetDuplicidade(id, ServicoAtual.ID);
                }
            }


            //------------------------------------
            if (_documento.ID > 0)
            {
                DocumentoLoteViewModel _documentoView = new DocumentoLoteViewModel();
                _documentoView.Documento = _documento;
                _documentoView.CamposDocumentoJSON = _docService.PesquisarDocumentosModulo(ServicoAtual.ID, _documento.Modelo.ID, "Caixa, Lote", string.Format("IdDocumento={0}", _documento.ID));

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

                string status = "";

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
                        if (UsaArquivoDados) { ViewData["Det"] = string.Format(status + " | Caixa: {0} | Lote: {1} ", detalhe.Caixa, detalhe.Lote); }
                    }
                    else
                    {
                        det = _docService.PesquisarMotivo(detalhe.IdDocumento,IdServico_Atual);
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
            foreach (Ocorrencia motivo in _motivo)
            {

                ViewData["Just"] = ViewData["Just"] + "<tr><td><li>" + motivo.IdOcorrencia + "<td> " + motivo.descOcorrencia + "</td></tr>";
                ViewData["Valida"] = ViewData["Valida"] + motivo.IdOcorrencia.ToString();
            }
            ViewData["Just"] = ViewData["Just"] + "</table>";

            // Se o ID passado como parametr for diferente do retornado pelo metodo, direciona poara sup... 

            if (_documento.StatusDocumento != 1010)
            {
                if (id != 0 && _documento.ID == 0)
                {
                    return RedirectToAction("ListarPendentes", new { area = "Documento", controller = "Supervisao" });
                }
            }
            ViewBag.ListaOcorrencia = new DocumentoRepository().ListarOcorrencia(IdServico_Atual,1);
            _documento.Nuvem = new StoragePrivateRepository(IdCliente_Atual).Nuvem; //TODO: 07/07/2016
            return View("Formalizar",_documento);
        }

        [HttpPost]
        public ActionResult AjaxCallEnviarDocumentoSupervisao(int id_documento, int id_motivo)
        {
            try
            {
                MK.Easydoc.Core.Entities.Documento _doc = (new Core.Entities.Documento { ID = id_documento, StatusDocumento = 1020 });
                _docService.AtualizarDocumento(_doc, ServicoAtual.ID);
                bool EmUso = _docService.EmUso(id_documento, UsuarioAtual.ID, 2, ServicoAtual.ID);
                _docService.IncluirMotivo(IdServico_Atual, id_documento, id_motivo, 1, UsuarioAtual.ID);

                RegistrarLOGSimples(4, 16, UsuarioAtual.NomeUsuario);
                // LOG: Enviou documento a supervisão
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, "Documento enviado para a supervisão!", null));
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Documento/Formalizacao/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Documento/Formalizacao/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Documento/Formalizacao/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Documento/Formalizacao/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Documento/Formalizacao/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Documento/Formalizacao/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Documento/Formalizacao/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
