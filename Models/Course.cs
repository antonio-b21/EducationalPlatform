using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationalPlatform.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title is a required field!")]
        [StringLength(100, ErrorMessage = "The Title field has a maximum length of 100 charaters!")]
        public string Title { get; set; }

        public virtual ICollection<Section> Sections { get; set; }

        [Required(ErrorMessage = "The Professor Id is a required field!")]
        public string ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }
        public IEnumerable<SelectListItem> Profs { get; set; }

        [Required(ErrorMessage = "The list of Students is a required field!")]
        public string[] StudentsIds { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public IEnumerable<SelectListItem> Studs { get; set; }
    }
}
