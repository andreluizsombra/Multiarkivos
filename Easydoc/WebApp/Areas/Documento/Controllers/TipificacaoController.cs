using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.WebApp.ViewModels;
using MK.Easydoc.Core.Repositories;

namespace MK.Easydoc.WebApp.Areas.Documento.Controllers
{
    public class TipificacaoController : BaseController
    {
        private const string _LOTE_SESSION_NAME = "__TipificarController__lote_imagens__";
        public Lote LoteImagens
        {
            get
            {
                return (Lote)Session[_LOTE_SESSION_NAME] ?? new Lote();
            }
            private set { Session[_LOTE_SESSION_NAME] = value; }
        }

        #region Private Read-Only Fields

        private readonly ILoteService _loteService;
        private readonly IDocumentoService _docService;


        Lote _lote = new Lote();
        #endregion

        #region Constructors

        public TipificacaoController()
        {
            _lote = new Lote();
            _loteService = DependencyResolver.Current.GetService<ILoteService>();
            _docService = DependencyResolver.Current.GetService<IDocumentoService>();
        }

        public ActionResult ListarPentendesTEMP()
        {
            List<Lote> _lotes = new List<Lote>();
            _lotes = (from l in this._loteService.ListarLotesTipificar(UsuarioAtual.ID, 1, ServicoAtual.ID)
                      select l).ToList<Lote>();

            ViewBag.LotesPendentes = _lotes;
            if (_lotes.Count == 0)
            {
                ViewBag.QtdLote = 0;
                return View();
            }
            else
            {
                return RedirectToAction("Tipificar", new { idlote = _lotes[0].Itens[0].IdLote, idloteItem = _lotes[0].Itens[0].ID });
            }
        }
        #endregion
        //
        // GET: /Documento/Tipificacao/

        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Tipificar(string idlote, string idloteItem)
        public ActionResult Tipificar()
        {
             List<Lote> _lotes = new List<Lote>();
            _lotes = (from l in this._loteService.ListarLotesTipificar(UsuarioAtual.ID, 1, ServicoAtual.ID)
                      select l).ToList<Lote>();

            ViewBag.LotesPendentes = _lotes;
            int TotalLote = _lotes.Count;
            if (TotalLote == 0)
            {
                ViewBag.QtdLote = TotalLote;
                return View();
            }
            else
            {
                ViewBag.QtdLote = TotalLote;
                ViewBag.IdLote = _lotes[0].Itens[0].IdLote;
                ViewBag.IdLoteItem = _lotes[0].Itens[0].ID;
                return View();
            }
            RegistrarLOGSimples(3, 10, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo de tipificacao
        }

        //public ActionResult Tipificar(string id_lote)
        //{
        //    ViewBag.IdLote = id_lote;
        //    ViewBag.Imagem = "/Content/Uploads/Souza Cruz/Contratos/2014/12/23/1/U1C1S2_2014122317131.jpg";
        //    return View();
        //}

        [HttpPost]
        public ActionResult AjaxCallTipificarDocumento(int id_lote, int id_item, int id_documento_modelo)
        {
            var tipificar = default(TipificarViewModel);
            var lote = default(Lote);

            try
            {
                                

                _loteService.TipificarItem(UsuarioAtual.ID, ServicoAtual.ID, id_lote, id_item, id_documento_modelo);
                lote = _loteService.GetLote(id_lote, UsuarioAtual.ID, ServicoAtual.ID, 0);

                tipificar = new TipificarViewModel();
                tipificar.QtdeItensLote = lote.Itens.Count();
                tipificar.QtdeItensLotePendente = (from li in lote.Itens where li.DocumentoModelo.ID == 0 select li).Count();


                string _caminho = string.Empty;
                string _arquivo = string.Empty;

                tipificar.PathImagem = Path.Combine(_caminho, _arquivo);

                if (tipificar.QtdeItensLotePendente > 0)
                {
                    _caminho = lote.PathCaptura;
                    _arquivo = (from li in lote.Itens where li.DocumentoModelo.ID == 0 select li).FirstOrDefault().NomeFinal;

                    tipificar.PathImagem = Path.Combine(_caminho, _arquivo);
                }
                else
                {
                    tipificar = new TipificarViewModel();
                    //return View("Menu");
                    return Json(new RetornoViewModel(false, "Todos os documentos deste lote foram tipificados com sucesso!", tipificar));
                }
                RegistrarLOGSimples(3, 11, UsuarioAtual.NomeUsuario);
                // LOG: Tipificou o documento
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null, tipificar));
        }
        //AjaxCallEnviarLoteSupervisao
        [HttpPost]
        public ActionResult AjaxCallEnviarLoteSupervisao(int id_lote, int id_item)
        {
            var tipificar = default(TipificarViewModel);
            var lote = default(Lote);

            try
            {

                //_loteService.TipificarItem(UsuarioAtual.ID, ServicoAtual.ID, id_lote, id_item, id_documento_modelo);
                lote = _loteService.GetLote(id_lote, UsuarioAtual.ID, ServicoAtual.ID, 0);

                lote.StatusLote = 1020;
                _loteService.AtualizarLote(lote);

                RegistrarLOGSimples(3, 12, UsuarioAtual.NomeUsuario);
                // LOG: Enviou documento para supervisão
                tipificar = new TipificarViewModel();

                return Json(new RetornoViewModel(false, "Lote encaminhado para supervisão!", tipificar));
                
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null, tipificar));
        }
        //-------------------------------------
        [HttpPost]
        public ActionResult AjaxCallEnviarLoteExcluir(int id_lote)
        {
            var tipificar = default(TipificarViewModel);
            //var lote = default(Lote);
            try
            {

                _loteService.ApagarLote(id_lote);
                tipificar = new TipificarViewModel();
                return Json(new RetornoViewModel(false, "Lote Excluido!", tipificar));

            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null, tipificar));
        }
        //-------------------------------------



        [HttpPost]
        public JsonResult AjaxCallBuscarDocumentoTipificar(int id_lote)
        {
            var tipificar = default(TipificarViewModel);
            var lote = default(Lote);

            try
            {
                // Verifica se o id passado esta pendente ou retona o primeiro caso o parametro venha com 0
                //this.Initialize();

                if (id_lote == 0)
                {
                    id_lote = (from l in this._loteService.ListarLotesPendente(UsuarioAtual.ID, 1, ServicoAtual.ID)
                               select l).FirstOrDefault().ID;
                }

                lote = _loteService.GetLote(id_lote, UsuarioAtual.ID, ServicoAtual.ID, 0);

                tipificar = new TipificarViewModel();
                tipificar.QtdeItensLote = lote.Itens.Count();
                tipificar.QtdeItensLotePendente = (from li in lote.Itens where li.DocumentoModelo.ID == 0 select li).Count();


                LoteItem _item = (from li in lote.Itens where li.DocumentoModelo.ID == 0 select li).FirstOrDefault();

                string _caminho = lote.PathCaptura;
                string _arquivo = _item.NomeFinal.Trim();//(from li in lote.Itens where li.DocumentoModelo.ID == 0 select li).FirstOrDefault().NomeFinal;

                ViewBag.IdLoteItem = _item.ID;



                tipificar.PathImagem = string.Format(@"{0}\{1}\{2}", @"\easydoc\StoragePrivate", _caminho.Trim().TrimEnd('\\', ' '), _arquivo.Trim()); //Path.Combine(_caminho, _arquivo);
                
                //Novo caminho
                tipificar.CaminhoImg = string.Format(@"{0}/{1}\{2}", @"/StoragePrivate", _caminho.Trim().TrimEnd('\\', ' '), _arquivo.Trim()); //Path.Combine(_caminho, _arquivo);

                if (tipificar == null) tipificar = new TipificarViewModel();
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null, tipificar));
        }

        [HttpPost]
        public JsonResult AjaxCallBuscarTiposDocumento()
        {
            var docs = default(List<MK.Easydoc.Core.Entities.DocumentoModelo>);

            try
            {
                var lista = this._docService.ListarTipos(ServicoAtual.ID);

                docs = (from doc in lista
                        select doc
                        ).ToList();
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null, docs));
        }



    }
}
