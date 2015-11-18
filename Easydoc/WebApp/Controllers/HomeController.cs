﻿using System;
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
        private IDictionary<string, object> _qryparams;
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

        [HttpPost]
        public JsonResult RetornaPendencias(int idServico)
        {
            var serv = new ServicoRepository();
            bool flag = serv.GetControleAtencao(idServico);
             var resultado = new
                    {
                        rest = flag.ToString()
                    };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //TODO: AndreSombra 17/11/2015
        [HttpPost]
        public JsonResult Dashboard_Doc_Modulo(int _periodoInicial, int _periodoFinal)
        {
            this._qryparams = new Dictionary<string, object>();
            this._qryparams.Clear();
            this._qryparams["Usuario_ID"] = UsuarioAtual.ID;
            this._qryparams["Servico_ID"] = ServicoAtual.ID;
            this._qryparams["periodoInicial"] = _periodoInicial;
            this._qryparams["periodoFinal"] = _periodoFinal;

            var serv = new ServicoRepository();
            var resultado = serv.ExibirDashboard_Doc_Modulo(this._qryparams);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //TODO: AndreSombra 18/11/2015
        [HttpPost]
        public JsonResult Dashboard_Doc_Captura(int _periodoInicial, int _periodoFinal)
        {
            this._qryparams = new Dictionary<string, object>();
            this._qryparams.Clear();
            this._qryparams["Usuario_ID"] = UsuarioAtual.ID;
            this._qryparams["Servico_ID"] = ServicoAtual.ID;
            this._qryparams["periodoInicial"] = _periodoInicial;
            this._qryparams["periodoFinal"] = _periodoFinal;

            var serv = new ServicoRepository();
            var resultado = serv.ExibirDashboard_Captura(this._qryparams);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }	

        public ActionResult Dashboard()
        {
            ViewBag.Message = "";
            //return View("Dashboard");
            return View("~/Views/Shared/_MorrisDonut.cshtml");
        }
        public ActionResult About()
        {
            ViewBag.Message = "";
            ViewBag.UsuarioAtual = UsuarioAtual;
            return View();
        }

        public ActionResult Teste()
        {
            ViewBag.Message = "TESTE";
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
