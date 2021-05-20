using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (CheckProfile() != null)
            {
                return CheckProfile();
            }
            else if (User.IsInRole("Student"))
            {
                return Redirect("/Student");
            }
            else if (User.IsInRole("Professor"))
            {
                return Redirect("/Professor");
            }

            return Redirect("/Course");
        }
    }
}