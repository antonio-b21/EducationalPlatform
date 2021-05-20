using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationalPlatform.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title is a required field!")]
        [StringLength(100, ErrorMessage = "The Title field has a maximum length of 100 charaters!")]
        public string Title { get; set; }

        public byte[] File { get; set; }

        public DateTime Date { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
    }
}