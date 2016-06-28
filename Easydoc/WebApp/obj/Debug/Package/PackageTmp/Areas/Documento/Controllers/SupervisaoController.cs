using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.WebApp.ViewModels;

namespace MK.Easydoc.WebApp.Areas.Documento.Controllers
{
    public class SupervisaoController : BaseController
    {
        #region Private Read-Only Fields

        private readonly ILoteService _loteService;
        private readonly IDocumentoService _docService;


        Lote _lote = new Lote();
        #endregion

        #region Constructors

        public SupervisaoController()
        {
            _lote = new Lote();
            _loteService = DependencyResolver.Current.GetService<ILoteService>();
            _docService = DependencyResolver.Current.GetService<IDocumentoService>();
        }
        #endregion Constructors

        public ActionResult ListarPendentes()
        {
            RegistrarLOGSimples(5, 17, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo Supervisão

            List<MK.Easydoc.Core.Entities.Documento> _documentos = new List<Core.Entities.Documento>(); 
            List<Lote> _lotes = new List<Lote>();
            List<DocumentoLoteViewModel> _documentosView = new List<DocumentoLoteViewModel>();

            if (ServicoAtual == null)
            {
                return RedirectToAction("EncerrarAcesso", "Login");
            }

            _documentos.AddRange(_docService.ListarDocumentosSupervisao(UsuarioAtual.ID, 1, ServicoAtual.ID).ToList<Core.Entities.Documento>());
            
            foreach (Core.Entities.Documento  _documento in _documentos)
            {
                DocumentoLoteViewModel _documentoView = new DocumentoLoteViewModel();
                _documentoView.Documento = _documento;
                _documentoView.CamposDocumentoJSON = _docService.PesquisarDocumentosModulo(ServicoAtual.ID, _documento.Modelo.ID, "Caixa, Lote", string.Format("IdDocumento={0}", _documento.ID));

                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //JsonResult _j = new JsonResult();
                //_j = Json(_documentoView.CamposDocumentoJSON);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                List<DocumentoDetalhe> _docs = jss.Deserialize<List<DocumentoDetalhe>>(_documentoView.CamposDocumentoJSON).ToList<DocumentoDetalhe>();
                //MEXER AQUI

                string det;

                foreach (DocumentoDetalhe detalhe in _docs)
                {
                    det = _docService.PesquisarMotivo(detalhe.IdDocumento, IdServico_Atual);
                    //_documentoView.CamposDocumentoDetalhe = string.Format("| Caixa: {0} | Lote: {1} | Motivo: {2}", detalhe.Caixa, detalhe.Lote,det); 
                    _documentoView.CamposDocumentoDetalhe = string.Format("Motivo: {0}", det); 
                }

                //var x = serializer.Deserialize(_j.Data.ToString(),DocumentoDetalhe);
                _documentosView.Add(_documentoView);

            }

            ViewBag.DocumentosPendentes = _documentosView;

            return View();
        }
        private string ConverteData(string data) {
            string _data = "";

            if (data != null)
            {
                _data = new DateTime(int.Parse(data.Substring(0, 4)), int.Parse(data.Substring(4, 2)), int.Parse(data.Substring(6, 2))).ToShortDateString();
            }else
	        {
                _data = null;
	        }
            return _data;   
        }

    }
    public class DocumentoDetalhe {
        public int IdDocumento { get; set; }
        public string PathArquivo { get; set; }
        public string Matricula { get; set; }
        public string Periodo { get; set; }
        public string Lote { get; set; }
        public string Caixa { get; set; }
    }
}