using MK.Easydoc.Core.Entities;
using MK.Easydoc.WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Areas.Manutencao.Controllers
{
    public class MenuController : BaseController
    {
        //
        // GET: /Manutencao/Menu/

        public ActionResult Index()
        {
            RegistrarLOGSimples(7, 19, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo Manutencao
            Session["Filtro"] = new Filtro() { Tipo = 0 };
            return View();
        }
        public ActionResult ListaCliente()
        {
            return View("ListaCliente");
        }

    }
}
