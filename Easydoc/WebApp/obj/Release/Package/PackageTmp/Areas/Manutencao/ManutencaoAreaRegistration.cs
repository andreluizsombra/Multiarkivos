using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Areas.Manutencao
{
    public class ManutencaoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manutencao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Manutencao_default",
                "Manutencao/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
