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
    public class ProfessorController : BaseController
    {
        //
        // GET: Show
        public ActionResult Show(string id)
        {
            Professor professor = db.Professors.Find(id);
            if (professor == null)
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
            return View(professor);
        }

        //
        // GET: Search
        public ActionResult Search(string search)
        {
            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            search = search ?? "";
            List<String> searchItems = new List<string>(search.Split(" .,?!()[]{};:".ToCharArray()));
            searchItems = searchItems.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            var professors = db.Professors;
            List<Professor> selectedProfessors = new List<Professor>();
            if (searchItems.Count() > 0)
            {
                foreach (var professor in professors)
                {
                    foreach (var item in searchItems.ToArray())
                    {
                        if (professor.FirstName.Contains(item) || professor.LastName.Contains(item))
                        {
                            selectedProfessors.Add(professor);
                            break;
                        }
                    }
                }
            }

            ViewBag.Professors = selectedProfessors;
            ViewBag.Search = search;
            return View();
        }


        //
        // GET: Professor
        public ActionResult Index()
        {
            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            return Redirect("/Professor/Show/" + User.Identity.GetUserId());
        }


        //
        // GET: New
        public ActionResult New()
        {
            if (!User.IsInRole("Professor"))
            {
                throw new HttpException(404, "");
            }

            Professor professor = new Professor();
            professor.Id = User.Identity.GetUserId();
            return View(professor);
        }

        //
        // POST: New
        [HttpPost]
        public ActionResult New(Professor professor)
        {
            if (!User.IsInRole("Professor"))
            {
                throw new HttpException(404, "");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Professors.Add(professor);
                    db.SaveChanges();
                    TempData["message"] = "Professor profile created!";
                    return RedirectToAction("Index");
                }

                return View(professor);
            }
            catch (Exception)
            {
                return View(professor);
            }
        }


        //
        // GET: Edit
        public ActionResult Edit(string id)
        {
            Professor professor = db.Professors.Find(id);
            if (professor == null ||
                professor.Id != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            return View(professor);
        }

        //
        // PUT: Edit
        [HttpPut]
        public ActionResult Edit(string id, Professor requestProfessor)
        {
            Professor professor = db.Professors.Find(id);
            if (professor == null ||
                professor.Id != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            try
            {
                if (ModelState.IsValid && TryUpdateModel(professor))
                {
                    professor.FirstName = requestProfessor.FirstName;
                    professor.LastName = requestProfessor.LastName;
                    professor.Degree = requestProfessor.Degree;
                    db.SaveChanges();
                    TempData["message"] = "Professor profile updated!";
                    return Redirect("/Professor/Show/" + professor.Id);
                }

                return View(requestProfessor);
            }
            catch (Exception)
            {
                return View(requestProfessor);
            }
        }


        //DELETE: Delete
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Professor professor = db.Professors.Find(id);
            if (professor == null ||
                professor.Id != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            db.Professors.Remove(professor);
            db.SaveChanges();
            return RedirectToAction("New");
        }
    }
}
