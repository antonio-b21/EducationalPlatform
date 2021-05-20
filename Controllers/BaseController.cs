using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    public class BaseController : Controller
    {
        protected Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        protected ActionResult CheckProfile()
        {
            string id = User.Identity.GetUserId();

            if (User.IsInRole("Student") && db.Students.Find(id) == null ||
                User.IsInRole("Professor") && db.Professors.Find(id) == null)
            {
                ApplicationUser user = db.Users.Find(id);
                return Redirect("/" + user.Role + "/New");
            }

            return null;
        }
    }
}
