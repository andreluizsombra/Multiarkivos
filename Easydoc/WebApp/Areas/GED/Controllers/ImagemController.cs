using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;

namespace MK.Easydoc.WebApp.Areas.GED.Controllers
{
    public class ImagemController : BaseController
    {
        //
        // GET: /GED/Imagem/upload

        public ActionResult Upload()
        {
            
            return View();
        }

        ////
        //// POST: /Account/Register
        //[AllowAnonymous]
        //[HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Upload (IEnumerable<HttpPostedFileBase> file)
        //{
        //    // Attempt to register the user
        //    string physicalPath = HttpContext.Server.MapPath("../") + "UploadImages" + "\\";
        //    for (int i = 0; i < Request.Files.Count; i++)
        //    {
        //        Request.Files[0].SaveAs(physicalPath + System.IO.Path.GetFileName(Request.Files[i].FileName));
        //    }
        //    return View();
        //}

    }
}
