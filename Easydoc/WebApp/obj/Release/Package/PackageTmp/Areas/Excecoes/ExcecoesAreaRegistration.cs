using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Areas.excecoes
{
    public class excecoesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "excecoes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "excecoes_default",
                "excecoes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
