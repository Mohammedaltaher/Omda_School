using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class StudentsController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            ViewBag.ddlDepartment = db.Department.Where(ac => ac.IsDeleted != "Y").ToList();
            ViewBag.ddlYear = db.Year.Where(ac => ac.IsDeleted != "Y").OrderByDescending(o => o.IsCurrent).ToList();
            var student = db.Student.ToList();
            return View(student);
        }
        public JsonResult PopulateLevel(int? DepartmentID)
        {
            var LevelList = db.Level.Where(x => x.DepartmentID == DepartmentID).Select(p => new { p.Id, p.Name }).ToList();
            List<Level> _Level = new List<Level>();

            foreach (var item in LevelList)
            {
                _Level.Add(new Level
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }
            return Json(_Level, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetStudentsList(Search search)
        {
            try
            {

                var studentList = db.Student.Where(c => c.IsDeleted != "Y").Select(a => new
                {
                    a.Id,
                    a.Name  ,
                    a.address  ,
                    a.Phone  ,
                    a.BirthDay  ,
                    a.Fees,
                    a.PaidFees,
                    LevelName = a.Level.Name ,
                    YearName = a.Year.Name ,
                    DepartmentName = a.Level.Department.Name,
                    a.LevelID ,
                    a.Level.DepartmentID ,
                    a.YearID
                }).ToList();
                if (search.DepartmentID != 0) studentList =  studentList.Where(a => a.DepartmentID == search.DepartmentID).ToList();
                if (search.LevelID != 0) studentList = studentList.Where(a => a.LevelID == search.LevelID).ToList();
                if (search.YearID != 0) studentList = studentList.Where(a => a.YearID == search.YearID).ToList();
                if (search.StudentName != null) studentList = studentList.Where(a => a.Name.Contains(search.StudentName)).ToList();

                return Json(studentList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1);
            }
        }

        public JsonResult BindStudents(int id)
        {
            try
            {
                // int ID = int.Parse(id);

                var student = db.Student.Where(c => c.Id == id).Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.address,
                    a.Phone,
                    a.BirthDay,
                    a.Fees,
                    a.PaidFees,
                    a.LevelID ,
                    a.YearID ,
                    DeptId = a.Level.DepartmentID

                });
                return Json(student, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult AddStudents(Student student)
        {
            try
            {
                student.IsDeleted = "N";
                db.Student.Add(student);
                db.SaveChanges();
                return Json(new { code = 1 });
            }
            catch (DbEntityValidationException e)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                return Json(new { code = 0 });
                // throw;
            }

        }
        [HttpPost]
        public ActionResult EditStudents(Student student)
        {
            try
            {
                var inDB = db.Student.SingleOrDefault(B => B.Id == student.Id);
                inDB.Name = student.Name;
                inDB.address = student.address;
                inDB.Phone = student.Phone;
                inDB.address = student.address;
                inDB.BirthDay = student.BirthDay;
                inDB.Fees = student.Fees;
                inDB.PaidFees = student.PaidFees;
                inDB.LevelID = student.LevelID;
                inDB.YearID = student.YearID;
                inDB.IsDeleted = "N" ;
                db.SaveChanges();
                return Json(new { code = 1 });
            }
            catch (Exception)
            {
                return Json(new { code = 0 });
            }
        }
        [HttpPost]
        public ActionResult DeleteStudents(int id)
        {
            try
            {
                var inDB = db.Student.SingleOrDefault(B => B.Id == id);
                if (inDB != null)
                {
                    inDB.IsDeleted = "Y";
                    db.SaveChanges();
                    return Json(new { code = 1 });
                }
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 0 });
            }
        }
    }
}