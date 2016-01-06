using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Infrastructure;

namespace MK.Easydoc.WebApp.Areas.Manutencao.Controllers
{
    public class ClienteController : BaseController
    {
        public ActionResult Index()
        {
            var Clientes = new ClienteRepository().ListarClientesUsuario(UsuarioAtual.ID);
            ViewBag.ListaClientes = Clientes.ToList();
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}
