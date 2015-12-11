﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Repositories;

namespace MK.Easydoc.WebApp.Areas.Seguranca.Controllers
{
    public class UsuarioController : BaseController
    {
        // GET: /Seguranca/Usuario/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaUsuarios(FormCollection frm)
        {
            int _tipo = int.Parse(frm["selTipo"].ToString());
            int _condicao = int.Parse(frm["selCondicao"].ToString());
            string _txtpesquisa = frm["txtpesquisa"].ToString();

            //int idcliente = new ClienteRepository().GetCliente()

            var usu = new UsuarioRepository().GetUsuarioCadastro(_tipo, _condicao, ClienteAtual.ID, _txtpesquisa, UsuarioAtual.ID);
            ViewBag.ListaUsuarios = usu;
            return View("ManutencaoUsuario");
        }

        public ActionResult Novo()
        {
            var cliente = new ClienteRepository().ListarClientesUsuario(UsuarioAtual.ID).ToList();
             ViewBag.Cliente = new SelectList
              (
                   cliente ,
                   "ID",
                   "Descricao"
               );
             var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
             //ViewBag.Teste = listaCliente;
             var listacli = new SelectList(
                             listaCliente.ToList(),
                             "ID",
                             "Descricao"
                         );
             ViewBag.ListaCliente = listacli;
             ViewBag.CodCliente = ClienteAtual.ID;
             var listaServico = new ClienteRepository().ListaServicoPorCliente(Session["NomeUsuario"].ToString(), ClienteAtual.ID.ToString());

             var lista = new SelectList(
                             listaServico.ToList(),
                             "ID",
                             "Descricao"
                         );
             ViewBag.ListaServico = lista;

            return View("Novo");
        }

        [HttpPost]
        public JsonResult AjaxCallBuscarServicos(int idCliente)
        {
            var cli = new ClienteRepository().ListaServicoPorCliente(Session["NomeUsuario"].ToString(), idCliente.ToString());

            return Json(cli.ToList(),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Incluir()
        {
            return View("Novo");
        }

        // GET: /Seguranca/ManutencaoUsuario/
        public ActionResult ManutencaoUsuario()
        {
            return View();
        }
        public void BloquearUsuario(int idUsuBloqueado)
        {
            new UsuarioRepository().BloquearUsuario(ServicoAtual.ID, idUsuBloqueado, UsuarioAtual.ID);
        }
        //
        // GET: /ControleAcesso/Usuario/
        public ActionResult MinhaConta()
        {
            ViewBag.FeedbackTrocaSenha = String.Empty;
            ViewBag.FeedbackOffline = "<span class='label label-important'>Não utiliza o sistema!</span>";

            return View();
        }

        //
        // GET: /ControleAcesso/Usuario/
        public ActionResult TrocarSenha()
        {
            ViewBag.FeedbackTrocaSenha = string.Empty;
            return View();
        }

        //
        // POST: /ControleAcesso/Usuario/
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AlterarSenha()
        {
            bool _senhaAlterada = true;

            try
            {
                _senhaAlterada = true;
            }
            catch (Exception) { _senhaAlterada = false; }
            finally
            {
                if (_senhaAlterada == true)
                    ViewBag.FeedbackTrocaSenha = "<div class='alert alert-success'> Senha alterada com sucesso! </div>";
                else
                    ViewBag.FeedbackTrocaSenha = "<div class='alert alert-error'> Ocorreu um erro durante a troca de senha! </div>";
            }

            return View("TrocarSenha");
        }
    }
}
