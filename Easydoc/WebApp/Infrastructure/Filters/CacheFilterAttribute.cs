using System;
using System.Web;
using System.Web.Mvc;

namespace MK.Easydoc.WebApp.Infrastructure.Filters
{
    public class CacheFilterAttribute : ActionFilterAttribute
    {
        #region Public Properties

        /// <summary>
        /// Define o valor do cache em segundos, o valor padrão é 10.
        /// </summary>
        /// <value>Duração do cache em segundos.</value>
        public int Duration { get; set; }

        #endregion

        #region Constructors

        public CacheFilterAttribute()
        {
            this.Duration = 10;
        }

        #endregion

        #region Public Methods

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Duration <= 0) return;

            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.Add(cacheDuration));
            cache.SetMaxAge(cacheDuration);
            cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        }

        #endregion
    }
}