using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int LevelID { get; set; }
        public Level Level { get; set; }
        public int YearID { get; set; }
        public Year Year { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string address { get; set; }
        public string BirthDay { get; set; }
        public double Fees { get; set; }
        public double PaidFees { get; set; }
        public string IsDeleted { get; set; }
    }
}