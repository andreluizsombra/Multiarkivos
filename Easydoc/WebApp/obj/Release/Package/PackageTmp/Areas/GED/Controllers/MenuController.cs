using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;

namespace MK.Easydoc.WebApp.Areas.GED.Controllers
{
    public class MenuController : BaseController
    {
        //
        // GET: /GED/Menu/

        public ActionResult Index()
        {
            return View();
        }
    }
}
