﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Entities;
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


        void CarregarCombos()
        {
            var listaPerfil = new PerfilRepository().ListaPerfil(ServicoAtual.ID).ToList();

            var lista_perfil = new SelectList(
                            listaPerfil,
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaPerfil = lista_perfil;

            //Lista Situação
            ViewBag.ListaSituacao = new ServicoRepository().ListaSituacao();

            var cliente = new ClienteRepository().ListarClientesUsuario(UsuarioAtual.ID).ToList();
            ViewBag.Cliente = new SelectList
             (
                  cliente,
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
        }

        public ActionResult Novo()
        {
            CarregarCombos();
            Session["TipoAcao"] = "1";
            ViewBag.StGravado = 0; //Habilitar o campo Login
            return View("Novo");
        }

        [HttpPost]
        public JsonResult AjaxCallBuscarServicos(int idCliente)
        {
            var cli = new ClienteRepository().ListaServicoPorCliente(Session["NomeUsuario"].ToString(), idCliente.ToString());

            return Json(cli.ToList(),JsonRequestBehavior.AllowGet);
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
            var _modulo = new ServicoRepository().BuscarCheckModulos(idServico,idPerfil);
            return Json(_modulo.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AjaxVerificarCPF(string cpf)
        {
            var RetCPF = new UsuarioRepository().VerificaCPFDisponivel(UsuarioAtual.ID, decimal.Parse(cpf.ToString()));
            return Json(RetCPF, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AjaxVerificarLOGIN(string login)
        {
            var Ret = new UsuarioRepository().VerificaLoginDisponivel(UsuarioAtual.ID, login);
            return Json(Ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Incluir(FormCollection frm)
        {
            try{

                var Ret = new UsuarioRepository().VerificaLoginDisponivel(UsuarioAtual.ID, frm["login"].ToString());
                var RetCPF = new UsuarioRepository().VerificaCPFDisponivel(UsuarioAtual.ID, decimal.Parse(frm["cpf"].ToString()));

                if ((Session["TipoAcao"] != null) && (Session["TipoAcao"] == "4")) { Ret.CodigoRetorno = 0; RetCPF.CodigoRetorno = 0; }

                if(Ret.CodigoRetorno == 1 || RetCPF.CodigoRetorno==1){
                    var _usu = new Usuario();

                    _usu.CPF = frm["cpf"].ToString();
                    _usu.NomeCompleto = frm["nome"].ToString();
                    _usu.NomeUsuario = frm["login"].ToString();
                    _usu.Senha = frm["senha"].ToString();
                    _usu.TipoAcao = 1;
                    _usu.ClienteID = int.Parse(frm["SelCliente"].ToString());
                    _usu.ServicoID = int.Parse(frm["SelServico"].ToString());
                    _usu.PerfilID = int.Parse(frm["SelPerfil"].ToString());
                    _usu.Situacao = int.Parse(frm["SelSituacao"].ToString());
                    _usu.Email = frm["email"].ToString();

                    ViewBag.Usuario = _usu;
                    //CarregarCombos();
                    if (RetCPF.CodigoRetorno == 1)
                        throw new Exception(RetCPF.Mensagem);
                    else
                        throw new Exception(Ret.Mensagem);
                }

                var usu = new Usuario();

                 usu.CPF = frm["cpf"].ToString();
                 usu.NomeCompleto = frm["nome"].ToString();
                 usu.NomeUsuario = frm["login"].ToString();
                 usu.Senha = frm["senha"].ToString();

                 if ((Session["TipoAcao"] != null) && (Session["TipoAcao"] != "4")){
                     usu.TipoAcao = 1;
                 }
                 else { usu.TipoAcao = 4; }

                 usu.ClienteID = int.Parse(frm["SelCliente"].ToString());
                 usu.ServicoID = int.Parse(frm["SelServico"].ToString());
                 usu.PerfilID  = int.Parse(frm["SelPerfil"].ToString());
                 usu.Situacao  = int.Parse(frm["SelSituacao"].ToString());
                 usu.Email     = frm["email"].ToString();

                 if (frm["trocar"] != null) usu.TrocarSenha = int.Parse(frm["trocar"].ToString());

                 var usuRep = new UsuarioRepository();

                 
                //Incluir um novo Usuario ou um novo servico a este usuario
                 var Retorno = usuRep.IncluirUsuario(usu);
                 if (Retorno.CodigoRetorno == -1){
                     Session["TipoAcao"] = "1";
                     ViewBag.StGravado = 1;
                     throw new Exception(Retorno.Mensagem);
                 }   
                 if ((Retorno.CodigoRetorno == -2) || (Retorno.CodigoRetorno == -3)){
                     Session["TipoAcao"] = "4";
                     ViewBag.StGravado = 4;
                     ViewBag.Usuario = new UsuarioRepository().GetUsuario(usu.NomeUsuario);
                     ViewBag.Lista = usuRep.ListaClienteServicos((int)Session["IdNovoUsuario"]);
                     throw new Exception(Retorno.Mensagem);
                 }

                 ViewBag.Usuario = new UsuarioRepository().GetUsuario(usu.NomeUsuario);
                 Session["IdNovoUsuario"] = (int)Retorno.CodigoRetorno;
                 var _lista = usuRep.ListaClienteServicos((int)Retorno.CodigoRetorno);  //Apos gravar sem erro retorna o codigo do novo usuario para CodigoRetorno  //(usu.ID);
                 ViewBag.Lista = _lista;
                 
                 ViewBag.Msg = "Gravado com sucesso.";
                 Session["TipoAcao"] = "4";
                 ViewBag.StGravado = 4; //Deshabilita o campo Login
            }
            catch(Exception ex){
                CarregarCombos();
                ViewBag.Error = ex.Message;
            }
            CarregarCombos();
            return View("Novo");
        }

        [HttpPost]
        public JsonResult AjaxVerificaLoginDisponivel(string nomeUsuario)
        {
            var retorno = new UsuarioRepository().VerificaLoginDisponivel(UsuarioAtual.ID, nomeUsuario);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AjaxExcluirUsuario(int idUsuario, int idServico, int idUsuarioAtual)
        {
            var retorno = new UsuarioRepository().ExcluirUsuario(idUsuario, idServico, idUsuarioAtual);
            return Json(retorno, JsonRequestBehavior.AllowGet);
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
