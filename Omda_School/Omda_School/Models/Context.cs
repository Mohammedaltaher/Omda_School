using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Omda_School.Models
{
    public class Context : DbContext

    {
        public Context()
           : base("DefaultConnection")
        {
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Year> Year { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<Degree> Degree { get; set; }
    }
}