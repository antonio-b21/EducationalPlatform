using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    [Authorize]
    public class CourseController : BaseController
    {
        //
        // GET: Show
        public ActionResult Show(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null ||
                course.Students.All(student => student.Id != User.Identity.GetUserId()) &&
                course.ProfessorId != User.Identity.GetUserId() &&
                !User.IsInRole("Admin"))
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

            ViewBag.isAdmin = User.IsInRole("Admin");
            ViewBag.isAssignedProfessor = course.ProfessorId == User.Identity.GetUserId();
            return View(course);
        }


        //
        // GET: Course
        public ActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            ViewBag.Courses = db.Courses.Include("Professor");
            return View();
        }


        //
        // GET: New
        public ActionResult New()
        {
            if (!User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            Course course = new Course();
            course.Profs = GetAllProfessors();
            course.Studs = GetAllStudents();
            return View(course);
        }

        //
        // POST: New
        [HttpPost]
        public ActionResult New(Course course)
        {
            if (!User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    course.Students = new Collection<Student>();
                    foreach (string StudentId in course.StudentsIds)
                    {
                        Student student = db.Students.Find(StudentId);
                        course.Students.Add(student);
                    }

                    db.Courses.Add(course);
                    db.SaveChanges();
                    TempData["message"] = "Course created!";
                    return RedirectToAction("Index");
                }

                course.Profs = GetAllProfessors();
                course.Studs = GetAllStudents();
                return View(course);
            }
            catch (Exception)
            {
                course.Profs = GetAllProfessors();
                course.Studs = GetAllStudents();
                return View(course);
            }
        }


        //
        // GET: Edit
        public ActionResult Edit(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null ||
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            course.Profs = GetAllProfessors();
            course.Studs = GetAllStudents();
            List<string> currentSelection = new List<string>();
            foreach (var student in course.Students)
            {
                currentSelection.Add(student.Id);
            }
            course.StudentsIds = currentSelection.ToArray();
            return View(course);
        }

        //
        // PUT: Edit
        [HttpPut]
        public ActionResult Edit(int id, Course requestCourse)
        {
            Course course = db.Courses.Find(id);
            if (course == null ||
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            try
            {
                if (ModelState.IsValid && TryUpdateModel(course))
                {
                    course.Title = requestCourse.Title;
                    course.ProfessorId = requestCourse.ProfessorId;
                    foreach (Student student in course.Students.ToList())
                    {
                        course.Students.Remove(student);
                    }
                    foreach (var studentId in course.StudentsIds)
                    {
                        Student student = db.Students.Find(studentId);
                        course.Students.Add(student);
                    }
                    db.SaveChanges();
                    TempData["message"] = "Course updated!";
                    return Redirect("/Course/Show/" + course.Id);
                }

                course.Profs = GetAllProfessors();
                course.Studs = GetAllStudents();
                List<string> currentSelection = new List<string>();
                foreach (var student in course.Students)
                {
                    currentSelection.Add(student.Id);
                }
                course.StudentsIds = currentSelection.ToArray();
                return View(requestCourse);
            }
            catch (Exception)
            {
                course.Profs = GetAllProfessors();
                course.Studs = GetAllStudents();
                List<string> currentSelection = new List<string>();
                foreach (var student in course.Students)
                {
                    currentSelection.Add(student.Id);
                }
                course.StudentsIds = currentSelection.ToArray();
                return View(requestCourse);
            }
        }


        //
        //DELETE: Delete
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null ||
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region Helpers
        [NonAction]
        private IEnumerable<SelectListItem> GetAllProfessors()
        {
            var selectList = new List<SelectListItem>();
            var professors = db.Professors;

            foreach (var professor in professors)
            {
                selectList.Add(new SelectListItem
                {
                    Value = professor.Id,
                    Text = professor.FirstName + " " + professor.LastName
                });
            }
            return selectList;
        }

        private IEnumerable<SelectListItem> GetAllStudents()
        {
            var selectList = new List<SelectListItem>();
            var students = db.Students;

            foreach (var student in students)
            {
                selectList.Add(new SelectListItem
                {
                    Value = student.Id,
                    Text = student.FirstName + " " + student.LastName
                });
            }
            return selectList;
        }
        #endregion
    }
}