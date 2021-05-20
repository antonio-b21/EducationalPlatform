using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationalPlatform.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title is a required field!")]
        [StringLength(100, ErrorMessage = "The Title field has a maximum length of 100 charaters!")]
        public string Title { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}