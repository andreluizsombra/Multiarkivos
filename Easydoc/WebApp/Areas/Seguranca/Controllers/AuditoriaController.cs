using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.Core.Helpers;
using MK.Easydoc.WebApp.ViewModels;
using System.Collections;
using MK.Easydoc.Core.Entities;
using System.Collections.Generic;
using MK.Easydoc.Core.Repositories;
using System.Linq;
using MK.Easydoc.WebApp.Controllers;

namespace MK.Easydoc.WebApp.Areas.Seguranca.Controllers
{
    public class AuditoriaController : BaseController
    {
        //
        // GET: /Seguranca/Auditoria/
        void CarregaSelAcao()
        {
            var lstAcao = new AcaoRepository().Listar(IdServico_Atual);
            ViewBag.ListaAcao = lstAcao;
        }
        public ActionResult Consulta()
        {
            CarregaSelAcao();
            return View("Listar");
        }
        public ActionResult Pesquisa(FormCollection frm)
        {
            try
            {
                int _idAcao = int.Parse(frm["selAcao"].ToString());
                int _condicao = int.Parse(frm["selCondicao"].ToString());
                string _txtpesquisa = frm["txtpesquisa"].ToString();
                var lst = new LogRepository().ConsultaLOG(IdServico_Atual, _idAcao, _txtpesquisa);
                ViewBag.ListaLOG = lst;
                CarregaSelAcao();
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View("Listar");
        }

        [HttpPost]
        public JsonResult AjaxListaLogDetalhe(int idLOG)
        {
            var _lst = new LogRepository().ConsultaLOG_Detalhe(idLOG);
            return Json(_lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Seguranca/Auditoria/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Seguranca/Auditoria/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Seguranca/Auditoria/Create

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
        // GET: /Seguranca/Auditoria/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Seguranca/Auditoria/Edit/5

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
        // GET: /Seguranca/Auditoria/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Seguranca/Auditoria/Delete/5

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
