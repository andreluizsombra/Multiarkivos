using System;
using Autofac;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Reflection;
//using SquishIt.Framework;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Optimization;
using MK.Easydoc.WebApp.Infrastructure.Modules;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.WebApp.Infrastructure.Filters;

namespace MK.Easydoc.WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequireAuthenticationFilterAttribute());
            filters.Add(new RoleAccessFilterAttribute());
            filters.Add(new CompressFilterAttribute());
            filters.Add(new CacheFilterAttribute());
            //filters.Add(new ElmahHandleErrorFilter());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*
            // rota default do hotsite
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Hotsite", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );*/
            routes.MapRoute(
              name: "Principal",
              url: "Principal",
              defaults: new { controller = "Home", action = "Inicio" }
            );

            routes.MapRoute(
             name: "Logout",
             url: "Logout",
             defaults: new { controller = "Login", action = "EncerrarAcesso" }
           );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", area = "", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "MK.Easydoc.WebApp.Controllers" }
            );
             
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterDependencyResolver();
            
        }


        protected void RegisterDependencyResolver()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.Load("MK.Easydoc.Core"))
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule(new LogInjectionModule());

            IContainer _container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
        }


        protected void Application_AuthenticateRequest()
        {
            try
            {
               DependencyResolver.Current.GetService<IAuthenticationService>().PerformApplicationAuthenticateRequest(this);
            }
            catch (Exception) { throw; }
        }
    }
}