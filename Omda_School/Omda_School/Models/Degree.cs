//using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Degree
    {
        public int Id { get; set; }
        public int StudentDegree { get; set; }
        public int ResultID { get; set; }
        public virtual Result Result { get; set; }
        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
        public string IsDeleted { get; set; }

    }
}