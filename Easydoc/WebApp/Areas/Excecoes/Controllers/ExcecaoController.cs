using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Areas.Excecoes.Controllers
{
    public class ExcecaoController : Controller
    {
        //
        // GET: /Excecoes/Excecao/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Excecoes/Excecao/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Excecoes/Excecao/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Excecoes/Excecao/Create

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
        // GET: /Excecoes/Excecao/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Excecoes/Excecao/Edit/5

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
        // GET: /Excecoes/Excecao/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Excecoes/Excecao/Delete/5

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
