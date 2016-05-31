using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Services;
using MK.Easydoc.WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Areas.Manutencao.Controllers
{
    public class DocumentoModeloController : BaseController
    {
        //
        // GET: /Manutencao/DocumentoModelo/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Novo()
        {
            ListaClienteServico(IdCliente_Atual.ToString(), IdServico_Atual);
            ViewBag.idCliente = IdCliente_Atual;
            ViewBag.idServico = IdServico_Atual;

            return View();
        }

        public ActionResult ListarCampos(int idCliente, int idServico)
        {
            try
            {
                var ListaCampos = new DocumentoRepository().ListarCamposDocumento(idServico);
                ListaClienteServico(idCliente.ToString());
                ViewBag.idCliente = idCliente;
                ViewBag.idServico = idServico;
                ViewBag.ListaCampos = ListaCampos;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            } 
            return View("Novo");
        }

        public ActionResult ListaServico(string idCliente)
        {
            ListaClienteServico(idCliente);
            ViewBag.idCliente = idCliente;
            return View("Novo");
        }
        void ListaClienteServico(string idCliente, int idServico = 0)
        {
            var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
            //ViewBag.Teste = listaCliente;
            var listacli = new SelectList(
                            listaCliente.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaCliente = listacli;
            ViewBag.CodCliente = idCliente;
            ViewBag.idCliente = idCliente;
            ViewBag.idServico = idServico;
            var listaServico = new ClienteRepository().ListaServicoPorCliente(Session["NomeUsuario"].ToString(), idCliente);

            var lista = new SelectList(
                            listaServico.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaServico = lista;
        }
    }
}
