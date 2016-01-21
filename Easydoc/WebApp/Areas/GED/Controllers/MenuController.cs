using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Repositories;

namespace MK.Easydoc.WebApp.Areas.GED.Controllers
{
    public class MenuController : BaseController
    {
        //
        // GET: /GED/Menu/

        public ActionResult Index()
        {
            RegistrarLOGSimples(2, 4, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo Captura
            return View();
        }
    }
}
