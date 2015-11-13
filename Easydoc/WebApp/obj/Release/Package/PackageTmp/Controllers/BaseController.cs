﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Helpers;
using MK.Easydoc.WebApp.ViewModels;
using MK.Easydoc.WebApp.Infrastructure.Filters;

namespace MK.Easydoc.WebApp.Controllers
{
    [CacheFilterAttribute(Duration = 0)]
    public class BaseController : Controller
    {
        #region Private Read-Only Fields

        private readonly IUsuarioService _usuarioService;
        private readonly IServicoService _servicoService;

        private readonly IClienteService _clienteService;

        #endregion

        #region Constructors

        public BaseController()
        {
            this._usuarioService = DependencyResolver.Current.GetService<IUsuarioService>();
            this._servicoService = DependencyResolver.Current.GetService<IServicoService>();
            this._clienteService = DependencyResolver.Current.GetService<IClienteService>();

        }

        #endregion

        #region Public Properties

        protected virtual Cliente ClienteAtual
        {
            get { return UserDataCookieHelper.GetUserDataCookie().ClienteAtual; }
        }

        protected virtual Servico ServicoAtual
        {
            get { return UserDataCookieHelper.GetUserDataCookie().ServicoAtual; }
        }

        protected virtual int IdClienteAtual
        {
            get { return UserDataCookieHelper.GetUserDataCookie().IdClienteAtual; }
        }

        protected virtual string UrlCSSEmpresaAtual
        {
            get { return UserDataCookieHelper.GetUserDataCookie().UrlCSSClienteAtual; }
        }

        protected virtual Usuario UsuarioAtual
        {
            get
            {
                //return UserDataCookieHelper.GetUserDataCookie().UsuarioAtual;
                
                
                var usuario = default(Usuario);
                

                if (UserDataCookieHelper.GetUserDataCookie().UsuarioAtual != null)
                {
                    usuario = UserDataCookieHelper.GetUserDataCookie().UsuarioAtual;
                } else
	            {
                    usuario = this._usuarioService.GetUsuario(User.Identity.Name);
	            }     
                //clientes = new List<Cliente>();

                //foreach (Servico _ser  in usuario.Servicos)
                //{

                //    clientes.Add(_clienteService.GetCliente(usuario.ID, _ser.IdCliente));

                //    //if ((from c in clientes where c.ID == _ser.IdCliente select c).Count()==0)
                //    //{
                //    //    //clientes.Add((new Cliente{ID=_ser.IdCliente,Descricao="TesteF"}));
                        
                //    //}
                //}

                //foreach (Cliente item in clientes)
                //{
                //    item.Servicos.AddRange((from s in usuario.Servicos where s.IdCliente == item.ID select s).ToList<Servico>());
                //}

                //UsuarioLogadoViewModel _usuarioAtual = (new UsuarioLogadoViewModel { Clientes = clientes});

                return usuario;//_usuarioAtual; 
            }
        }


        #endregion

        #region Protected Methods

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }

        #endregion

        #region Ajax Methods

        public void AbandonarSessao() {

            UserDataCookieHelper.CreateUserDataCookie(null);

            UserDataCookieHelper.CreateUserDataCookie(new UserDataEntity()
            {
                IdClienteAtual = 0,
                ClienteAtual = null,//new Cliente(),
                ServicoAtual = null,//new Servico(),
                UsuarioAtual = null,//new Usuario(),
                UrlCSSClienteAtual = null
            });

            HttpContext.Response.Expires = -1;                
            HttpContext.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
        }

        [HttpPost]
        public JsonResult AjaxCallTrocarClienteAtual(int idCliente)
        {
            var usuario = default(Usuario);
            var cliente = default(Cliente);

            try
            {
                usuario = this._usuarioService.GetUsuario(User.Identity.Name);
                if (usuario == null) { throw new Exception("Não foi possível trocar a empresa atual."); }

                cliente = (new Cliente { ID = idCliente, Descricao = "TEsteF" });//usuario.Servicos.Where(e => e.IdCliente == idCliente).FirstOrDefault().Cliente;

                if (usuario == null) { throw new Exception("Não foi possível trocar a empresa atual."); }

                switch (cliente.ID)
                {
                    case 1: // sanofi                        
                        UserDataCookieHelper.CreateUserDataCookie(new UserDataEntity()
                        {
                            IdClienteAtual = idCliente,
                            ClienteAtual = cliente,
                            UrlCSSClienteAtual = Url.Content("~/assets/themes/sanofi-theme.css")
                        });

                        break;
                    case 2: // medley
                        UserDataCookieHelper.CreateUserDataCookie(new UserDataEntity()
                        {
                            IdClienteAtual = idCliente,
                            ClienteAtual = cliente,
                            UrlCSSClienteAtual = Url.Content("~/assets/themes/medley-theme.css")

                        });
                        break;
                }

                //HttpContext.Response.Expires = -1;                
                //HttpContext.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null));
        }

        [HttpPost]
        public JsonResult AjaxCallTrocarServicoAtual(int idServico)
        {
            var usuario = default(Usuario);
            var servico = default(Servico);
            try
            {
                if (!User.Identity.IsAuthenticated) return Json(new RetornoViewModel(false,"erro"));
                if (UsuarioAtual == null)
                {
                    usuario = this._usuarioService.GetUsuario(User.Identity.Name);
                }
                else if (UsuarioAtual.ID == 0)
                {
                    usuario = this._usuarioService.GetUsuario(User.Identity.Name);
                } else {
                    usuario = UsuarioAtual;
                }
                
                if (usuario == null) { throw new Exception("Não foi possível trocar a empresa atual."); }

                if (idServico != 0)
                {
                    servico = _servicoService.GetServico(usuario.ID, idServico); //usuario.Servicos.Where(e => e.ID == idServico).FirstOrDefault();
                }
                else if (ServicoAtual != null)
                {
                    servico = ServicoAtual;
                }
                else
                {
                    servico = _servicoService.ListarServicosUsuario(usuario.ID).Where(e => e.Default).FirstOrDefault();
                }

                if (usuario == null) { throw new Exception("Não foi possível trocar a Cliente atual."); }


                UserDataCookieHelper.CreateUserDataCookie(new UserDataEntity()
                {
                    IdClienteAtual = servico.IdCliente,
                    ClienteAtual = _clienteService.GetCliente(usuario.ID, servico.IdCliente),
                    ServicoAtual = servico, //salva aqui o servico
                    UsuarioAtual = usuario,
                    UrlCSSClienteAtual = Url.Content("~/assets/themes/Easydoc-theme.css")
                });

                //HttpContext.Response.Expires = -1;                
                //HttpContext.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null));
        }

        #endregion

    }
}