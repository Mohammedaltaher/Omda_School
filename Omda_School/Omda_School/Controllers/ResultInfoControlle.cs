using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class ResultInfoController : Controller
    {
        Context db = new Context();
        public ActionResult Index(int id)
        {
            ViewBag.ddlDepartment = db.Department.Where(ac => ac.IsDeleted != "Y").ToList();
            var degree = db.Degree.Where(c => c.IsDeleted != "Y" && c.ResultID == id).ToList();
            var result = db.Result.SingleOrDefault(c => c.IsDeleted != "Y" && c.Id == id);
            ViewBag.ddlResult = db.Result.Where(c => c.IsDeleted != "Y" && c.Id == id).Select(t=> new
            {
                t.LevelAdmin ,
               levelName = t.Level.Name ,
               yearName =  t.Year.Name ,
               t.Type , 
               deptName = t.Level.Department.Name
            });
            ViewBag.ResultID = id;
            ViewBag.ddlSubject = db.Subject.Where(ac => ac.IsDeleted != "Y" && ac.LevelID == result.LevelID).ToList();
            ViewBag.ddlStudent= db.Student.Where(ac => ac.IsDeleted != "Y" && ac.LevelID == result.LevelID).ToList();
            return View(degree);
        }
        [HttpPost]
        public JsonResult GetDegreesList(int id)
        {
            try
            {
                var degreeList = db.Degree.Where(c => c.IsDeleted != "Y" && c.ResultID == id).Select(a => new
                {
                    a.Id,
                    a.StudentDegree,
                    a.StudentID,
                    a.SubjectID ,
                   StudentName = a.Student.Name
                }).ToList();

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
        public ActionResult EditDegrees(Degree degree)
        {
            try
            {
                var inDB = db.Degree.SingleOrDefault(B => B.Id == degree.Id);
                inDB.StudentDegree = degree.StudentDegree;
                inDB.IsDeleted = "N";
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