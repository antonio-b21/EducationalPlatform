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
    public class StudentController : BaseController
    {
        //
        // GET: Show
        public ActionResult Show(string id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            ViewBag.currentUser = User.Identity.GetUserId();
            ViewBag.isAdmin = User.IsInRole("Admin");
            return View(student);
        }


        //
        // GET: Student
        public ActionResult Index()
        {
            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            return Redirect("/Student/Show/" + User.Identity.GetUserId());
        }


        //
        // GET: New
        public ActionResult New()
        {
            if (!User.IsInRole("Student"))
            {
                throw new HttpException(404, "");
            }

            Student student = new Student();
            student.Id = User.Identity.GetUserId();
            return View(student);
        }

        //
        // POST: New
        [HttpPost]
        public ActionResult New(Student student)
        {
            if (!User.IsInRole("Student"))
            {
                throw new HttpException(404, "");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    TempData["message"] = "Student profile created!";
                    return RedirectToAction("Index");
                }

                return View(student);
            }
            catch (Exception)
            {
                return View(student);
            }
        }


        //
        // GET: Edit
        public ActionResult Edit(string id)
        {
            Student student = db.Students.Find(id);
            if (student == null ||
                student.Id != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            return View(student);
        }

        //
        // PUT: Edit
        [HttpPut]
        public ActionResult Edit(string id, Student requestStudent)
        {
            Student student = db.Students.Find(id);
            if (student == null ||
                student.Id != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            try
            {
                if (ModelState.IsValid && TryUpdateModel(student))
                {
                    student.FirstName = requestStudent.FirstName;
                    student.LastName = requestStudent.LastName;
                    student.YearOfStudy = requestStudent.YearOfStudy;
                    db.SaveChanges();
                    TempData["message"] = "Student profile updated!";
                    return Redirect("/Student/Show/" + student.Id);
                }
                
                return View(requestStudent);
            }
            catch (Exception)
            {
                return View(requestStudent);
            }
        }


        //DELETE: Delete
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Student student = db.Students.Find(id);
            if (student == null ||
                student.Id != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("New");
        }
    }
}
