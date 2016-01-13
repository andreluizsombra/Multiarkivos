﻿using System;
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
    public class ServicoController : BaseController
    {
        //
        // GET: /Manutencao/Servico/

        public ActionResult Index()
        {
            var lst = new ServicoRepository().ListaServicoCliente(UsuarioAtual.ID);
            ViewBag.ListaCliente = lst;
            return View(lst);
        }

        //
        // GET: /Manutencao/Servico/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manutencao/Servico/Create
        void CarregarComboCliente()
        {
            var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
            var lista = new SelectList(
                            listaCliente.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaCliente = lista;
        }
        public ActionResult Create()
        {
            CarregarComboCliente();
            return View();
        }

        //
        // POST: /Manutencao/Servico/Create

        [HttpPost]
        public ActionResult Incluir(FormCollection frm)
        {
            var Retorno = new Retorno();
            var _Servico = new Servico();
            try
            {
                var ser = new Servico()
                {
                    TipoAcao = 1,
                    Descricao = frm["nomeservico"].ToString(),
                    IdCliente = int.Parse(frm["SelCliente"].ToString()),
                    ServicoDefault = frm["servdefault"] == null ? false : bool.Parse(frm["servdefault"].ToString()),
                    ArquivoDados = frm["arqdados"]==null ? false : bool.Parse(frm["arqdados"].ToString()),
                    ControleAtencao = frm["contversao"]==null ? false : bool.Parse(frm["contversao"].ToString()),
                    IdUsuarioAtual = UsuarioAtual.ID
                };
                _Servico = ser;
                var Ret = new ServicoRepository();
                Retorno = Ret.Incluir(ser);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                //ViewBag.Msg = Retorno.Mensagem;
                TempData["Msg"] = Retorno.Mensagem;

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = Retorno.Mensagem;
                ViewBag.Servico = _Servico;
                var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
                var lista = new SelectList(
                                listaCliente.ToList(),
                                "ID",
                                "Descricao"
                            );
                ViewBag.ListaCliente = lista;
                return View("Create");
            }
        }

        [HttpPost]
        public ActionResult Alterar(FormCollection frm)
        {
            var Retorno = new Retorno();
            var _Servico = new Servico();
            int _idServico = int.Parse(frm["idservico"].ToString());
            try
            {
                var ser = new Servico()
                {
                    TipoAcao = 2,
                    ID = _idServico, 
                    Descricao = frm["nomeservico"].ToString(),
                    IdCliente = int.Parse(frm["SelCliente"].ToString()),
                    ServicoDefault =  frm["servdefault"]  == "on" ? true : false,
                    ArquivoDados =    frm["arqdados"]     == "on" ? true : false,
                    ControleAtencao = frm["contatencao"]  == "on" ? true : false,
                    IdUsuarioAtual = UsuarioAtual.ID
                };
                _Servico = ser;
                var Ret = new ServicoRepository();
                Retorno = Ret.Incluir(ser);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                //ViewBag.Msg = Retorno.Mensagem;
                TempData["Msg"] = Retorno.Mensagem;

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = Retorno.Mensagem;
                ViewBag.Servico = _Servico;
                CarregarComboCliente();
                var lst = new ServicoRepository().ListaServicoCliente(UsuarioAtual.ID).SingleOrDefault(s => s.ID == _idServico);
                return View("Edit",lst);
            }
        }

        [HttpPost]
        public JsonResult AjaxExcluir(string idServico)
        {
            try
            {
                int _idservico = int.Parse(idServico);
                var srv = new ServicoRepository().ListaServicoCliente(UsuarioAtual.ID).SingleOrDefault(s=> s.ID==_idservico);
                srv.TipoAcao = 3; //TipoAcao 3 é Exclusão
                srv.IdUsuarioAtual = UsuarioAtual.ID; 
                    
                var Ret = new ServicoRepository();
                var Retorno = Ret.Incluir(srv);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                //ViewBag.Msg = Retorno.Mensagem;
                return Json(Retorno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                //throw new Exception(ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Manutencao/Servico/Edit/5

        public ActionResult Edit(int id)
        {
            var lst = new ServicoRepository().ListaServicoCliente(UsuarioAtual.ID).SingleOrDefault(s => s.ID == id);
            CarregarComboCliente();
            return View(lst);
        }

        //
        // POST: /Manutencao/Servico/Edit/5

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
        // GET: /Manutencao/Servico/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Manutencao/Servico/Delete/5

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
