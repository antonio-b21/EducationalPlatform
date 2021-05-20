using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        //
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }


        //
        // GET: NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}