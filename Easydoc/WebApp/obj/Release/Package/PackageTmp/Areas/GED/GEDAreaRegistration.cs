using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Areas.GED
{
    public class GEDAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GED";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GED_default",
                "GED/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
