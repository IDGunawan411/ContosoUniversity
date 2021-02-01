using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace ContosoUniversity.Models
{
    public enum Credits
    {
        Movie = 1,
        Game = 2,
        Book = 3
    }

    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public string Secret { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}