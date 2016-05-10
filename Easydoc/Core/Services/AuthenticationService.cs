using System;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.Core.Security;

namespace MK.Easydoc.Core.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        #region Private Constants

        private const string AUTHENTICATION_NAME = ".easydoc.auth.";
        private DateTime DATA_EXPIRATION_FOR_IS_PERSISTENT = DateTime.Now.AddYears(1);
        private DateTime DATA_EXPIRATION_FOR_NON_IS_PERSISTENT = DateTime.Now.AddMinutes(30);

        #endregion

        #region Private Read-Only Fields

        //private readonly ILog _logger;
        private readonly IUsuarioService _usuarioService;

        #endregion

        #region Constructors

        //public AuthenticationService(ILog logger, IUsuarioService usuarioService)
        public AuthenticationService(IUsuarioService usuarioService)
        {
            //this._logger = logger;
            this._usuarioService = usuarioService;
        }

        #endregion

        #region IAuthenticationService

        public bool AuthenticateUser(HttpContextBase context, string username, string password, bool remember)
        {
            var user = default(Usuario);
            var encTicket = default(string);
            var cookie = default(HttpCookie);
            var authenticated = default(bool);
            var formsAuthenticationTicket = default(FormsAuthenticationTicket);

            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new InvalidOperationException("Favor informar um username válido.");

                if (string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException("Favor informar uma senha válida.");

                user = this._usuarioService.GetUsuario(username);

                if (user == null)
                    throw new ApplicationException("Não foi possível encontrar o usuário.");

                // TODO : Alterar essa comparação de senhas
                authenticated = true;

                if (!authenticated)
                    throw new InvalidOperationException("Não foi possível autenticar o usuário.");

                formsAuthenticationTicket = new FormsAuthenticationTicket(1
                    , username
                    , DateTime.Now
                    , (remember == true ? DATA_EXPIRATION_FOR_IS_PERSISTENT : DATA_EXPIRATION_FOR_NON_IS_PERSISTENT)
                    , remember
                    , username
                    , FormsAuthentication.FormsCookiePath);

                encTicket = FormsAuthentication.Encrypt(formsAuthenticationTicket);

                cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                cookie.Path = FormsAuthentication.FormsCookiePath;
                cookie.Expires = formsAuthenticationTicket.Expiration;

                context.Response.Cookies.Add(cookie);
            }
            catch (Exception ex) { throw ex; }
            //catch (Exception ex) { this._logger.Error(ex.Message, ex); throw ex; }

            return authenticated;
        }

        public void PerformApplicationAuthenticateRequest(HttpApplication application)
        {
            var principal = default(easydocPrincipal);
            var identity = default(easydocIdentity);
            var authCookie = default(HttpCookie);
            var authTicket = default(FormsAuthenticationTicket);
            var user = default(Usuario);

            try
            {
                authCookie = application.Context.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

                if (authCookie == null) { return; }

                authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket == null) { return; }

                if (authTicket.UserData != "")
                {
                    if (application.Context.Cache.Get(authTicket.UserData) == null)
                    {
                        user = this._usuarioService.GetUsuario(authTicket.UserData);

                        identity = new easydocIdentity(AUTHENTICATION_NAME
                            , true
                            , user.NomeUsuario
                            , user.ID
                            , user.NomeCompleto
                            , user.Email
                            , user.Perfil);

                        principal = new easydocPrincipal(identity);

                        application.Context.Cache.Insert(authTicket.UserData
                            , principal
                            , null
                            , Cache.NoAbsoluteExpiration
                            , TimeSpan.FromMinutes(15));
                    }
                    else { principal = (application.Context.Cache.Get(authTicket.UserData) as easydocPrincipal); }

                    application.Context.User = principal;
                }
            }
            catch (Exception ex) { throw ex; }
            //catch (Exception ex) { this._logger.Error(ex.Message, ex); throw ex; }
        }

        #endregion        

    }
}
