using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Repositories;

namespace MK.Easydoc.WebApp.Areas.Relatorio.Controllers
{
    public class ListaController : BaseController
    {
        //
        // GET: /Relatorio/Lista/

        public ActionResult Index()
        {
            ViewBag.Lista = new RelatorioRepository().Listar(IdServico_Atual);
            return View();
        }

        //
        // GET: /Relatorio/Lista/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Relatorio/Lista/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Relatorio/Lista/Create

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
        // GET: /Relatorio/Lista/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Relatorio/Lista/Edit/5

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
        // GET: /Relatorio/Lista/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Relatorio/Lista/Delete/5

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
