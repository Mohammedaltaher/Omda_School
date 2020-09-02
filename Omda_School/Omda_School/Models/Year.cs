using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Year
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsDeleted { get; set; }
        public string IsCurrent { get; set; }

    }
}