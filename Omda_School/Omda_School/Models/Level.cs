using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Level
    {
        public int Id { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public string Name { get; set; }
        public string IsDeleted { get; set; }
    }
}