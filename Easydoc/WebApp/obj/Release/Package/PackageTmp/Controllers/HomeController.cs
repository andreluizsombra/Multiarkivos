using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories;

namespace MK.Easydoc.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //AjaxCallTrocarServicoAtual(0);
            //ViewBag.Message = string.Empty;

            if (ServicoAtual == null)
            {
                Servico _serv = UsuarioAtual.Clientes.Where(c => c.Servicos.Where(s => s.Default = true).FirstOrDefault().Default = true).FirstOrDefault().Servicos.Where(s => s.Default = true).FirstOrDefault();
                AjaxCallTrocarServicoAtual(_serv.ID);
                
                var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
                ViewBag.Teste = listaCliente;
                ViewBag.ListaCliente = new SelectList(
                        listaCliente.ToList(),
                        "ID",
                        "Descricao"
                    );
            }
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "";
            ViewBag.UsuarioAtual = UsuarioAtual;
            return View();
        }


        public ActionResult Inicio()
        {
            ViewBag.Message = "";
            //ViewBag.UsuarioAtual = UsuarioAtual;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}
