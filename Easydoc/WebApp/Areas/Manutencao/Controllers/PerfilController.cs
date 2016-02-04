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
    public class PerfilController : BaseController
    {
        //
        // GET: /Manutencao/Perfil/

        public ActionResult Index()
        {
            return RedirectToAction("Create");
        }
        public ActionResult Create()
        {
            ListaClienteServico(IdCliente_Atual.ToString(), IdServico_Atual);
            ViewBag.idCliente = IdCliente_Atual;
            ViewBag.idServico = IdServico_Atual;

            return View("Create");
        }

        public ActionResult ListaServico(string idCliente)
        {
            ListaClienteServico(idCliente);
            
            return View("Create");
        }

        public ActionResult ListaPerfil(int idCliente, int idServico)
        {
            ListaClienteServico(idCliente.ToString(),idServico);
            //Lista Perfil
            var listaPerfil = new PerfilRepository().ListaPerfil(idCliente,idServico);
            var lista_perfil = new SelectList(
                            listaPerfil.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaPerfil = lista_perfil;

            return View("Create");
        }

        //Metodo somente para Listar Cliente,Servicos e Perfil [Não é um Controller]
        void ListaClienteServico(string idCliente, int idServico=0)
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

            //Lista Perfil
            var listaPerfil = new PerfilRepository().ListaPerfil(int.Parse(idCliente), idServico);
            var lista_perfil = new SelectList(
                            listaPerfil.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaPerfil = lista_perfil;
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
