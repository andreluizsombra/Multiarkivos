using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Areas.Documento
{
    public class DocumentoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Documento";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Documento_default",
                "Documento/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
