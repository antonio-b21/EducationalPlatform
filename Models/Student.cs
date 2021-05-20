using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationalPlatform.Models
{
    public class Student
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "The First Name is a required field!")]
        [StringLength(100, ErrorMessage = "The First Name field has a maximum length of 100 charaters!")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "The Last Name is a required field!")]
        [StringLength(50, ErrorMessage = "The Last Name field has a maximum length of 50 charaters!")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "The Year of Study is a required field!")]
        [RegularExpression("([1,2,3])", ErrorMessage = "The Year of Study must be 1, 2 or 3!")]
        public int YearOfStudy { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}