using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Search
    {
        public int YearID { get; set; }
        public Year Year { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public int LevelID { get; set; }
        public Level Level { get; set; }
        public string StudentName { get; set; }
    }
}