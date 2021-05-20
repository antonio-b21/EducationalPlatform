using EducationalPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Controllers
{
    [Authorize]
    public class DocumentController : BaseController
    {
        //
        // GET: Download
        public ActionResult Download(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null ||
                document.Section.Course.Students.All(student => student.Id != User.Identity.GetUserId()) &&
                document.Section.Course.ProfessorId != User.Identity.GetUserId() &&
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            return File(document.File, "application/pdf", document.Title + ".pdf");
        }


        //
        // GET: New
        public ActionResult New(int id)
        {
            Section section = db.Sections.Find(id);
            if (section == null ||
                section.Course.ProfessorId != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            Document document = new Document();
            document.SectionId = id;
            return View(document);
        }

        //
        // POST: New
        [HttpPost]
        public ActionResult New(Document document)
        {
            Section section = db.Sections.Find(document.SectionId);
            if (section.Course.ProfessorId != User.Identity.GetUserId())
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            HttpPostedFileBase file = Request.Files["FileData"];
            document.Title = document.Title.Trim().Replace(" ", "_");
            document.File = ConvertToBytes(file);
            document.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid && file.ContentType == "application/pdf")
                {
                    db.Documents.Add(document);
                    db.SaveChanges();
                    TempData["message"] = "File uploaded!";
                    return Redirect("/Course/Show/" + document.Section.CourseId);
                }

                return View(document);
            }
            catch (Exception)
            {
                return View(document);
            }
        }


        //
        // GET: Edit
        public ActionResult Edit(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null ||
                document.Section.Course.ProfessorId != User.Identity.GetUserId() &&
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            return View(document);
        }

        //
        //PUT: Edit
        [HttpPut]
        public ActionResult Edit(int id, Document requestDocument)
        {
            Document document = db.Documents.Find(id);
            if (document == null ||
                document.Section.Course.ProfessorId != User.Identity.GetUserId() &&
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
                if (ModelState.IsValid && TryUpdateModel(document))
                {
                    document.Title = requestDocument.Title.Replace(" ", "_");
                    db.SaveChanges();
                    TempData["message"] = "File updated!";
                    return Redirect("/Course/Show/" + document.Section.CourseId);
                }

                return View(requestDocument);
            }
            catch (Exception)
            {
                return View(requestDocument);
            }
        }


        //
        //DELETE: Delete
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null ||
                document.Section.Course.ProfessorId != User.Identity.GetUserId() &&
                !User.IsInRole("Admin"))
            {
                throw new HttpException(404, "");
            }

            if (CheckProfile() != null)
            {
                return CheckProfile();
            }

            Section section = document.Section;
            db.Documents.Remove(document);
            db.SaveChanges();
            return Redirect("/Course/Show/" + section.CourseId);
        }


        #region Helpers
        private byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);
            byte[] fileBytes = reader.ReadBytes(file.ContentLength);
            return fileBytes;
        }
        #endregion
    }
}