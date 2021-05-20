using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    [Authorize]
    public class SectionController : BaseController
    {
        //
        // GET: New
        public ActionResult New(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null ||
                course.ProfessorId != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            Section section = new Section();
            section.CourseId = id;
            ViewBag.CourseTitle = course.Title;
            return View(section);
        }

        //
        // POST: New
        [HttpPost]
        public ActionResult New(Section section)
        {
            Course course = db.Courses.Find(section.CourseId);
            if (course.ProfessorId != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Sections.Add(section);
                    db.SaveChanges();
                    TempData["message"] = "Section created!";
                    return Redirect("/Course/Show/" + section.CourseId);
                }

                ViewBag.CourseTitle = course.Title;
                return View(section);
            }
            catch (Exception)
            {
                ViewBag.CourseTitle = course.Title;
                return View(section);
            }
        }


        //
        // GET: Edit
        public ActionResult Edit(int id)
        {
            Section section = db.Sections.Find(id);
            if (section == null ||
                section.Course.ProfessorId != User.Identity.GetUserId() &&
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            Course course = db.Courses.Find(section.CourseId);
            ViewBag.CourseTitle = course.Title;
            return View(section);
        }

        //
        //PUT: Edit
        [HttpPut]
        public ActionResult Edit(int id, Section requestSection)
        {
            Section section = db.Sections.Find(id);
            if (section == null ||
                section.Course.ProfessorId != User.Identity.GetUserId() &&
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            try
            {
                if (ModelState.IsValid && TryUpdateModel(section))
                {
                    section.Title = requestSection.Title;
                    db.SaveChanges();
                    TempData["message"] = "Section updated!";
                    return Redirect("/Course/Show/" + section.CourseId);
                }

                Course course = db.Courses.Find(section.CourseId);
                ViewBag.CourseTitle = course.Title;
                return View(requestSection);
            }
            catch (Exception)
            {
                Course course = db.Courses.Find(section.CourseId);
                ViewBag.CourseTitle = course.Title;
                return View(requestSection);
            }
        }


        //
        //DELETE: Delete
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Section section = db.Sections.Find(id);
            if (section == null ||
                section.Course.ProfessorId != User.Identity.GetUserId() && 
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            db.Sections.Remove(section);
            db.SaveChanges();
            return Redirect("/Course/Show/" + section.CourseId);
        }
    }
}
