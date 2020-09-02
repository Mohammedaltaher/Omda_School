using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class DegreesController : Controller
    {
        Context db = new Context();
        public ActionResult Index(int id)
        {
            var result = db.Result.SingleOrDefault(c => c.IsDeleted != "Y" && c.Id == id);

            ViewBag.ddlDepartment = db.Department.Where(ac => ac.IsDeleted != "Y" ).ToList();
            ViewBag.ddlYear = db.Year.Where(ac => ac.IsDeleted != "Y" && ac.Id == result.YearID).OrderByDescending(o => o.IsCurrent).ToList();
            ViewBag.ddlSubject = db.Subject.Where(ac => ac.IsDeleted != "Y" && ac.LevelID == result.LevelID).ToList();
            ViewBag.ddlLevel = db.Level.Where(ac => ac.IsDeleted != "Y" && ac.Id == result.LevelID).ToList();
            var degree = db.Degree.Where(c => c.IsDeleted != "Y" && c.ResultID == id).ToList();
            ViewBag.ResultID = id;
            return View(degree);
        }
        public JsonResult PopulateSubject(int? LevelID)
        {
            var SubjectList = db.Subject.Where(x => x.LevelID == LevelID).Select(p => new { p.Id, p.Name }).ToList();
            List<Subject> _Subject = new List<Subject>();

            foreach (var item in SubjectList)
            {
                _Subject.Add(new Subject
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }
            return Json(_Subject, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDegreesList(Search search)
        {
            try
            {
                var degreeList = db.Degree.Where(c => c.IsDeleted != "Y" && c.ResultID == search.YearID && c.SubjectID == search.LevelID ).Select(a => new
                {
                    a.Id,
                    a.StudentDegree  ,
                    StudentName = a.Student.Name ,
                    a.SubjectID,
                    a.Result.YearID ,
                    a.Result.Level.DepartmentID ,
                    a.Result.LevelID ,
                    a.ResultID 
                }).ToList();

                if (search.StudentName != null) degreeList = degreeList.Where(a => a.StudentName.Contains(search.StudentName)).ToList();


                return Json(degreeList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1);
            }
        }

        public JsonResult BindDegrees(int id)
        {
            try
            {
                // int ID = int.Parse(id);

                var degree = db.Degree.Where(c => c.Id == id).Select(a => new
                {
                    a.Id,
                    a.StudentDegree,
                    a.StudentID

                });
                return Json(degree, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult AddDegrees(Degree degree)
        {
            try
            {
                degree.IsDeleted = "N";
                db.Degree.Add(degree);
                db.SaveChanges();
                return Json(new { code = 1 });
            }
            catch (Exception e)
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
        public ActionResult EditDegrees(Degree degree)
        {
            try
            {
                var inDB = db.Degree.SingleOrDefault(B => B.Id == degree.Id);
                inDB.StudentDegree = degree.StudentDegree;
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
        public ActionResult DeleteDegrees(int id)
        {
            try
            {
                var inDB = db.Degree.SingleOrDefault(B => B.Id == id);
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