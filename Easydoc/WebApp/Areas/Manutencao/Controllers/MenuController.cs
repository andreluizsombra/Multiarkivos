using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Areas.Manutencao.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Manutencao/Menu/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListaCliente()
        {
            return View("ListaCliente");
        }

    }
}
