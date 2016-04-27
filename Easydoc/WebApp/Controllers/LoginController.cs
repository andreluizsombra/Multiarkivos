using System;
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
using TecFort.Framework.Generico;
using MK.Easydoc.Core.Infrastructure.Framework;
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
        public ActionResult Index(string msg="")
        {
            if (msg != "")
            {
                ViewBag.Error = msg;
                TempData["Msg"] = msg;
            }
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
                    var _cripto = new Criptografia();
                    var _utils = new Util();
                    string _senha = _cripto.Executar(model.Senha.Trim().ToString(), _utils.ChaveCripto, Criptografia.TipoNivel.Baixo, Criptografia.TipoAcao.Encriptar, Criptografia.TipoCripto.Números);

                    bool valido = this._usuarioService.ValidarUsuario(model.NomeUsuario, _senha);

                    if (valido)
                    {
                        // TODO:14/04/2016 
                        var ret = new UsuarioRepository().VerificaServicoPerfil(model.NomeUsuario, _senha);
                        if (ret.CodigoRetorno != 0)
                        {
                            //return RedirectToRoute("Logout", new { msg = ret.Mensagem });
                            return RedirectToAction("Index", new { msg = ret.Mensagem });
                        }


                        bool _autenticado = this._authenticatioService.AuthenticateUser(this.HttpContext, model.NomeUsuario, model.Senha, model.ManterConectado);
                        if (_autenticado)
                        {
                            Session["NomeUsuario"] = model.NomeUsuario;

                            //TODO: 07/03/2016 Verifica se foi selecionado o Relembre-me e cria um cokie para armazenar o nome do usuário.
                            if (Request.Cookies["Login"] == null)
                            {
                                if (model.ManterConectado) 
                                {
                                    HttpCookie cokLogin = new HttpCookie("Login");
                                    cokLogin["username"] = model.NomeUsuario;
                                    cokLogin["lembrarnome"] = "sim";
                                    cokLogin.Expires = DateTime.Now.AddDays(1d);
                                    Response.Cookies.Add(cokLogin);
                                }
                            }
                            else
                            {
                                if (model.ManterConectado)
                                {
                                    HttpCookie cokLogin = new HttpCookie("Login");
                                    cokLogin["username"] = model.NomeUsuario;
                                    cokLogin["lembrarnome"] = "sim";
                                    Response.Cookies.Set(cokLogin);
                                }
                                if (model.ManterConectado == false)
                                {
                                    HttpCookie cokLogin = new HttpCookie("Login");
                                    cokLogin["username"] = "";
                                    cokLogin["lembrarnome"] = "nao";
                                    Response.Cookies.Set(cokLogin);
                                }
                            }

                            var cli = new ClienteRepository();
                            cli.PrimeiroClienteServicoPadrao(model.NomeUsuario, model.Senha);

                            if (cli == null) TempData["Error"] = "Nenhum Cliente e Serviço Padrão selecionado...";

                            Session["IdCliente"] = cli.TCliente.ID;
                            Session["NomeCliente"] = cli.TCliente.Descricao;
                            Session["NomeServico"] = cli.Servico;
                            Session["IdServico"] = cli.idServico;//aqui
                                    
                            
                            // LOG: Login Autenticado -- Cesar
                            int _idUsuario = new UsuarioRepository().GetUsuario(model.NomeUsuario).ID;

                            log.RegistrarLOG(cli.TCliente.ID, cli.idServico, 0, 0, 1, 1, 0, 0, model.NomeUsuario);
                            log.RegistrarLOGDetalhe(1, model.NomeUsuario);

                           
                            return RedirectToRoute(new { action = "../Home", controller = "", area = "" });// Redirect (returnUrl ?? FormsAuthentication.DefaultUrl);
                            
                        }

                        FormsAuthentication.SetAuthCookie(model.NomeUsuario, false);
                    }
                }
                catch (Exception ex) {
                    //ModelState.AddModelError("Error", ex.Message);
                    // LOG: Login Não Autenticado
                    log.RegistrarLOG(0, 0, 0, 0, 1, 2, 0, 0, model.NomeUsuario);
                    log.RegistrarLOGDetalhe(2, model.NomeUsuario);
                    ViewBag.Atencao = ex.Message;
                    TempData["Msg"] = ex.Message;
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
                var log = new LogRepository(); // LOG: LOG no Lougout
                int _idUsuario = new UsuarioRepository().GetUsuario(Session["NomeUsuario"].ToString()).ID;
                log.RegistrarLOG(int.Parse(Session["IdCliente"].ToString()), int.Parse(Session["IdServico"].ToString()), 0, _idUsuario, 2, 3, 0, 0, Session["NomeUsuario"].ToString());
                log.RegistrarLOGDetalhe(3, Session["NomeUsuario"].ToString());

                //TODO: 07/03/2016 Novo metodo usando a proc LiberaUsuarioLogado
                new UsuarioRepository().LiberaUsuarioLogado(IdServico_Atual, UsuarioAtual.ID);
                
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
