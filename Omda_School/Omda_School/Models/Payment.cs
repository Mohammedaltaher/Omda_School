using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentDate { get; set; }
        public string Note { get; set; }
        public int PaymentYearDateID { get; set; }
        public float Amount { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
        public string IsDeleted { get; set; }
    }
}