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
    public class PerfilController : Controller
    {
        //
        // GET: /Manutencao/Perfil/

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult ListaServico(string idCliente)
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
            var listaServico = new ClienteRepository().ListaServicoPorCliente(Session["NomeUsuario"].ToString(), idCliente);

            var lista = new SelectList(
                            listaServico.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaServico = lista;

            return View("Create");
        }

        [HttpPost]
        public JsonResult AjaxListaModulos(int idServico)
        {
            var _modulo = new ServicoRepository().ListaModulos(idServico);
            return Json(_modulo.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AjaxCheckModulos(int idServico, int idPerfil)
        {
            var _modulo = new ServicoRepository().BuscarCheckModulos(idServico, idPerfil);
            return Json(_modulo.ToList(), JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Manutencao/Perfil/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manutencao/Perfil/Create

        public ActionResult Create()
        {
            var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
            //ViewBag.Teste = listaCliente;
            var lista = new SelectList(
                            listaCliente.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaCliente = lista;
            return View("Create");
        }

        //
        // POST: /Manutencao/Perfil/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manutencao/Perfil/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Manutencao/Perfil/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manutencao/Perfil/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Manutencao/Perfil/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
