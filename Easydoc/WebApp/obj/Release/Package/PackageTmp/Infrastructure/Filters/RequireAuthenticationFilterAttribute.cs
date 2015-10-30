using System;
using System.Linq;
using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Infrastructure.Filters
{
    public class RequireAuthenticationFilterAttribute : AuthorizeAttribute
    {

        #region Private Read-Only Fields

        private readonly string[] _listaExcessoes = new string[] { "HotsiteController", "LoginController", "AssetsController", "BaseController" };

        #endregion

        #region Public Override Methods

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            //W
            //base.OnAuthorization(filterContext);
            //if (filterContext.Result is HttpUnauthorizedResult)
            //{
            //    filterContext.HttpContext.Response.Redirect("/");
            //}
            //else
            //{
            //    var skipAuthorization = _listaExcessoes.Contains(filterContext.Controller.GetType().Name, StringComparer.InvariantCultureIgnoreCase)
            //        || filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            //        || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);


            //    if (!skipAuthorization)
            //    {
            //        base.OnAuthorization(filterContext);
            //    }
            //}
            var skipAuthorization = _listaExcessoes.Contains(filterContext.Controller.GetType().Name, StringComparer.InvariantCultureIgnoreCase)
                    || filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);


            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);

                if (filterContext.Result is HttpUnauthorizedResult)
                {
                    filterContext.HttpContext.Response.Redirect("/");
                }
            }
        }
        #endregion
    }
}