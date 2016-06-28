using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.WebApp.Controllers;

namespace MK.Easydoc.WebApp.Areas.Documento.Controllers
{
    public class MenuController : BaseController
    {


        #region Private Read-Only Fields

        private readonly ILoteService _loteService;
        private readonly IDocumentoService _docService;

        #endregion

        #region Constructors

        public MenuController()
        {
            _loteService = DependencyResolver.Current.GetService<ILoteService>();
            _docService = DependencyResolver.Current.GetService<IDocumentoService>();
        }

        #endregion

        //
        // GET: /GED/Menu/

        public ActionResult Index()
        {

            //|PC| - 19042016

            ViewBag.QtdeTip = RetornarQtdLotesPendenteTipificar();
            ViewBag.QtdeDig = RetornarQtdDocumentosDigitar();
            ViewBag.QtdeSup = RetornarQtdDocumentosSupervisao();
            ViewBag.QtdeFor = RetornarQtdDocumentosFormalizar();
            ViewBag.QtdeVin = RetornarQtdDocumentosVincular();

            return View();
        }


        private int RetornarQtdLotesPendenteTipificar()
        {
            int _qtde = 0;

            try
            {
                // Verifica se o id passado esta pendente ou retona o primeiro caso o parametro venha com 0
                //this.Initialize();
                _qtde = (from l in this._loteService.ListarLotesTipificar(UsuarioAtual.ID, 1, ServicoAtual.ID)
                           select l).Count();

                return _qtde;
            }
            catch { return 0; }
        
        }

        private int RetornarQtdDocumentosDigitar()
        {
            int _qtde = 0;

            try
            {
                // Verifica se o id passado esta pendente ou retona o primeiro caso o parametro venha com 0
                //this.Initialize();
                _qtde = (from l in this._docService.ListarDocumentosTipificar(UsuarioAtual.ID, 1, ServicoAtual.ID)
                         select l).Count();

                return _qtde;
            }
            catch { return 0; }

        }

        private int RetornarQtdDocumentosSupervisao()
        {
            int _qtde = 0;

            try
            {
                // Verifica se o id passado esta pendente ou retona o primeiro caso o parametro venha com 0
                //this.Initialize();
                _qtde = (from l in this._docService.ListarDocumentosSupervisao(UsuarioAtual.ID, 1, ServicoAtual.ID)
                         select l).Count();

                return _qtde;
            }
            catch { return 0; }

        }

        private int RetornarQtdDocumentosFormalizar()
        {
            int _qtde = 0;

            try
            {
                // Verifica se o id passado esta pendente ou retona o primeiro caso o parametro venha com 0
                //this.Initialize();
                _qtde = (from l in this._docService.ListarDocumentosFormalizar(UsuarioAtual.ID, 1, ServicoAtual.ID)
                         select l).Count();

                return _qtde;
            }
            catch { return 0; }

        }

        private int RetornarQtdDocumentosVincular()
        {
            int _qtde = 0;

            try
            {
                // Verifica se o id passado esta pendente ou retona o primeiro caso o parametro venha com 0
                //this.Initialize();
                _qtde = (from l in this._docService.ListarDocumentosVincular(UsuarioAtual.ID, 1, ServicoAtual.ID)
                         select l).Count();

                return _qtde;
            }
            catch { return 0; }

        }
    }
}
