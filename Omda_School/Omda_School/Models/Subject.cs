using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExamDegree { get; set; }
        public int LevelID { get; set; }
        public Level Level { get; set; }
        public string IsDeleted { get; set; }
    }
}