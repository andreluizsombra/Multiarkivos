using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;

namespace MK.Easydoc.WebApp.Areas.Seguranca.Controllers
{
    public class MenuController : BaseController
    {
        //
        // GET: /Seguranca/Menu/

        public ActionResult Index()
        {
            RegistrarLOGSimples(8, 22, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo SEGURANCA

            return View();
        }

    }
}
