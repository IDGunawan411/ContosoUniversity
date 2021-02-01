using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public string ID_Student { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string EnrollmentDate { get; set; }
        
        public string Secret { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}