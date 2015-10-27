using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MK.Easydoc.Core.Security;

namespace MK.Easydoc.WebApp.Infrastructure.Filters
{
    public sealed class RoleAccessFilterAttribute : ActionFilterAttribute
    {
        #region Public Override Methods

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var skipAction = default(bool);
            var skipController = default(bool);
            var actionDescriptor = default(GrantAccessAttribute);
            var controllerDescriptor = default(GrantAccessAttribute);

            bool skipExecuting = filterContext.ActionDescriptor.IsDefined(typeof(GrantAccessAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(GrantAccessAttribute), true);

            if (!skipExecuting) { base.OnActionExecuting(filterContext); }
            else
            {

                if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(GrantAccessAttribute), true))
                    controllerDescriptor = ((GrantAccessAttribute)filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(GrantAccessAttribute), true)[0]);

                if (filterContext.ActionDescriptor.IsDefined(typeof(GrantAccessAttribute), true))
                    actionDescriptor = ((GrantAccessAttribute)filterContext.ActionDescriptor.GetCustomAttributes(typeof(GrantAccessAttribute), true)[0]);

                if (controllerDescriptor != null)
                    skipController = controllerDescriptor.Roles.Contains(((easydocIdentity)filterContext.HttpContext.User.Identity).Role);

                if (actionDescriptor != null)
                    skipAction = actionDescriptor.Roles.Contains(((easydocIdentity)filterContext.HttpContext.User.Identity).Role);

                if (skipController == false && skipAction == false)
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Error", action = "AcessoNegado", area = String.Empty })
                    );
                else
                    base.OnActionExecuting(filterContext);

            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        #endregion
    }
}