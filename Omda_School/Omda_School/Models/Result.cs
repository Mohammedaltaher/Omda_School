using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string LevelAdmin { get; set; }
        public int LevelID { get; set; }
        public Level Level { get; set; }
        public int YearID { get; set; }
        public Year Year { get; set; }
        public string IsDeleted { get; set; }
    }
}