﻿using System;
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

namespace MK.Easydoc.WebApp.Controllers
{
    public class LoginController : BaseController
    {

        #region Private Read-Only Fields

        private readonly IUsuarioService _usuarioService;
        private readonly IAuthenticationService _authenticatioService;

        #endregion

        #region Constructors

        public LoginController()
        {
            this._usuarioService = DependencyResolver.Current.GetService<IUsuarioService>();
            this._authenticatioService = DependencyResolver.Current.GetService<IAuthenticationService>();
        }

        #endregion

        #region Public Methods

        //
        // GET: /Login/
        public ActionResult Index()
        {
            ViewBag.Atencao = "";
            return View();
        }

        //
        // POST: /Login/
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Autenticar(LoginViewModel model, string returnUrl)
        {
            var log = new LogRepository();

            AbandonarSessao();

            if (ModelState.IsValid)
            {
                try
                {
                    bool valido = this._usuarioService.ValidarUsuario(model.NomeUsuario, model.Senha);

                    if (valido)
                    {
                        bool _autenticado = this._authenticatioService.AuthenticateUser(this.HttpContext, model.NomeUsuario, model.Senha, model.ManterConectado);
                        if (_autenticado)
                        {
                            Session["NomeUsuario"] = model.NomeUsuario;

                            
                            var cli = new ClienteRepository();
                            cli.PrimeiroClienteServicoPadrao(model.NomeUsuario);
                            
                            Session["IdCliente"] = cli.TCliente.ID;
                            Session["NomeCliente"] = cli.TCliente.Descricao;
                            Session["NomeServico"] = cli.Servico;
                            Session["IdServico"] = cli.idServico;
                                    

                            //var cli = new ClienteRepository();
                            //cli.PrimeiroCliente(model.NomeUsuario);
                            //Session["NomeCliente"] = cli.TCliente.Descricao;
                            //Session["NomeServico"] = cli.Servico;
                            //Session["IdServico"] = cli.idServico;

                            int _idUsuario = new UsuarioRepository().GetUsuario(model.NomeUsuario).ID;
                            log.RegistrarLOG(cli.TCliente.ID, cli.idServico, 0, _idUsuario, 1, 1, 0, 0, model.NomeUsuario);
                            log.RegistrarLOGDetalhe("Usuario autenticado no sistema", model.NomeUsuario);

                            return RedirectToRoute(new { action = "../Home", controller = "", area = "" });// Redirect (returnUrl ?? FormsAuthentication.DefaultUrl);
                            
                        }

                        FormsAuthentication.SetAuthCookie(model.NomeUsuario, false);
                    }
                }
                catch (Exception ex) {
                    //ModelState.AddModelError("Error", ex.Message);
                    log.RegistrarLOG(0, 0, 0, 0, 1, 2, 0, 0, model.NomeUsuario);
                    log.RegistrarLOGDetalhe("Autenticacao Invalida", model.NomeUsuario);
                    ViewBag.Atencao = ex.Message;
                }
            }
            return View("Index");
        }

        


        //
        // GET: /Login/
        public ActionResult EncerrarAcesso()
        {
            try
            {
                var log = new LogRepository();
                int _idUsuario = new UsuarioRepository().GetUsuario(Session["NomeUsuario"].ToString()).ID;
                log.RegistrarLOG(int.Parse(Session["IdCliente"].ToString()), int.Parse(Session["IdServico"].ToString()), 0, _idUsuario, 2, 3, 0, 0, Session["NomeUsuario"].ToString());
                log.RegistrarLOGDetalhe("Finalizou o sistema", Session["NomeUsuario"].ToString());

                AbandonarSessao();
                FormsAuthentication.SignOut();
                this.SingOut();
            }
            catch (Exception ex) { ModelState.AddModelError("error", ex); TempData["ViewData"] = ViewData; }

            return RedirectToAction("Index");
        }

        //
        // GET: /Login/
        public ActionResult RecuperarSenha()
        {
            ViewBag.SendResult = string.Empty;
            return View();
        }

        //
        // POST: /Login/
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReenviarSenha()
        {
            bool _reenviado = false;

            try
            {
                _reenviado = true;
            }
            catch (Exception) { _reenviado = false; }
            finally
            {
                if (_reenviado == true)
                    TempData["SendResult"] = "<div class='alert alert-success'> E-mail com as instruções encaminhado com sucesso! </div>";
                else
                    TempData["SendResult"] = "<div class='alert alert-error'> Ocorreu um erro durante o envio do e-mail! </div>";
            }

            return RedirectToAction("RecuperarSenha");
        }

        #endregion

        #region Private Methods

        private void SingOut()
        {
            var application = default(HttpApplication);

            try
            {
                foreach (var key in Request.Cookies.AllKeys)
                    Request.Cookies.Add(new HttpCookie(key, null) { Expires = DateTime.Now.AddDays(-1) });

                foreach (var key in Response.Cookies.AllKeys)
                    Response.Cookies.Add(new HttpCookie(key, null) { Expires = DateTime.Now.AddDays(-1) });

                application = this.HttpContext.ApplicationInstance;
                if (application != null)
                {
                    IDictionaryEnumerator cacheEnum = application.Context.Cache.GetEnumerator();
                    while (cacheEnum.MoveNext())
                    {
                        var cacheKey = ((DictionaryEntry)cacheEnum.Current).Key;
                        var cacheResult = application.Context.Cache.Get(cacheKey.ToString());

                        application.Context.Cache.Remove(cacheKey.ToString());
                    }
                }

                AbandonarSessao();

                Response.Cookies.Clear();
                ViewData.Clear();
                TempData.Clear();
                Session.Clear();
                Session.Abandon();

                FormsAuthentication.SignOut();
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

    }
}
