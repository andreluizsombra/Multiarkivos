using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;

namespace MK.Easydoc.WebApp.Areas.Documento.Controllers
{
    public class ProcessarController : BaseController
    {
        //
        // GET: /Documento/Processar/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Documento/Processar/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Documento/Processar/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Documento/Processar/Create

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
        // GET: /Documento/Processar/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Documento/Processar/Edit/5

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
        // GET: /Documento/Processar/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Documento/Processar/Delete/5

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
