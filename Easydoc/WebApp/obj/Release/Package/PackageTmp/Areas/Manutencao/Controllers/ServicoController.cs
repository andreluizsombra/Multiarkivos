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
    public class ServicoController : BaseController
    {
        //
        // GET: /Manutencao/Servico/

        public ActionResult Index(string msg="")
        {
            RegistrarLOGSimples(7, 21, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo Manutencao/Servico

            //var lst = new ServicoRepository().ListaServicoCliente(UsuarioAtual.ID);
            //ViewBag.ListaCliente = lst;
            List<Servico> lst = null;
            //Session["Filtro"] = new Filtro() { Tipo = 0 };
            if (msg != "") {
                TempData["Msg"] = msg;
                ViewBag.Msg = msg;
            }
            return View(lst);
        }
        public ActionResult Limpar()
        {
            Session["Filtro"] = new Filtro() { Tipo = 0 };
            return View("Index");
        }

        public ActionResult Listar(string msg="")
        {
            var f = (Filtro)Session["Filtro"];
            var lstServico = new ServicoRepository().PesquisaServicoCliente(f.Tipo, f.Condicao, UsuarioAtual.ID, f.Pesquisa, UsuarioAtual.ID);
            ViewBag.ListaClientes = lstServico;
            if (msg != "")
            {
                TempData["Msg"] = msg;
                ViewBag.Msg = msg;
            }
            return View("index",lstServico);
        }


        public ActionResult Pesquisa(FormCollection frm)
        {
            int _tipo = Convert.ToInt16(frm["selTipo"].ToString());
            int _condicao = Convert.ToInt16(frm["selCondicao"].ToString());
            string _txtpesquisa = frm["txtpesquisa"].ToString();
            Session["Filtro"] = new Filtro() { Tipo = _tipo, Condicao = _condicao, Pesquisa = _txtpesquisa, IdUsuarioAtual = UsuarioAtual.ID };
            var lstServico = new ServicoRepository().PesquisaServicoCliente(_tipo, _condicao, UsuarioAtual.ID, _txtpesquisa, UsuarioAtual.ID);
            ViewBag.ListaClientes = lstServico;
            return View("Index",lstServico);
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
        public ActionResult Create(string msg="")
        {
            if (msg != "")
            {
                //TempData["Error"] = msg;
                ViewBag.Error = msg;
            }
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

                return RedirectToAction("Index", new { msg=Retorno.Mensagem });
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
                //return View("Create", new { msg = Retorno.Mensagem });
                return RedirectToAction("Create", new { msg = Retorno.Mensagem });
            }
        }

        [HttpPost]
        public ActionResult Alterar(FormCollection frm)
        {
            var Retorno = new Retorno();
            var _Servico = new Servico();
            int _idServico = int.Parse(frm["idservico"].ToString());
            string _desc_antiga = frm["desc_antiga"].ToString();
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
                    IdUsuarioAtual = UsuarioAtual.ID,
                    DescricaoAntiga = _desc_antiga
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

                return RedirectToAction("Listar", new { msg = Retorno.Mensagem });
            }
            catch
            {
                //TempData["Error"] = Retorno.Mensagem;
                //ViewBag.Servico = _Servico;
                //CarregarComboCliente();
                //var lst = new ServicoRepository().ListaServicoCliente(UsuarioAtual.ID).SingleOrDefault(s => s.ID == _idServico);
                //return View("Edit",lst);
                return RedirectToAction("Edit", new { id = _idServico, msg = Retorno.Mensagem });
                
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
                ViewBag.Msg = Retorno.Mensagem;
                TempData["Msg"] = Retorno.Mensagem;
                return Json(Retorno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                TempData["Error"] = ex.Message;
                var ret = new Retorno() { Mensagem = ex.Message };
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Manutencao/Servico/Edit/5

        public ActionResult Edit(int id, string msg="")
        {
            var lst = new ServicoRepository().ListaServicoCliente(UsuarioAtual.ID).SingleOrDefault(s => s.ID == id);
            CarregarComboCliente();
            if (msg != "") ViewBag.Error = msg;
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
