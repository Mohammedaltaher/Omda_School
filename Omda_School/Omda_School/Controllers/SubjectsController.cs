using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class SubjectsController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            ViewBag.ddlDepartment = db.Department.Where(ac => ac.IsDeleted != "Y").ToList();
            var subject = db.Subject.ToList();
            return View(subject);
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
        public JsonResult GetSubjectsList(Search search)
        {
            try
            {
                var subjectList = db.Subject.Where(c => c.IsDeleted != "Y").Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.ExamDegree,
                    LevelName = a.Level.Name,
                    DepartmentName = a.Level.Department.Name ,
                    a.Level.DepartmentID ,
                    a.LevelID 
                }).ToList();
                if (search.DepartmentID != 0) subjectList = subjectList.Where(a => a.DepartmentID == search.DepartmentID).ToList();
                if (search.LevelID != 0) subjectList = subjectList.Where(a => a.LevelID == search.LevelID).ToList();
                return Json(subjectList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1);
            }
        }

        public JsonResult BindSubjects(int id)
        {
            try
            {
                // int ID = int.Parse(id);

                var subject = db.Subject.Where(c => c.Id == id).Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.ExamDegree,
                    a.LevelID ,
                    a.Level.DepartmentID

                });
                return Json(subject, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult AddSubjects(Subject subject)
        {
            try
            {
                subject.IsDeleted = "N";
                db.Subject.Add(subject);
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
        public ActionResult EditSubjects(Subject subject)
        {
            try
            {
                var inDB = db.Subject.SingleOrDefault(B => B.Id == subject.Id);
                inDB.Name = subject.Name;
                inDB.ExamDegree = subject.ExamDegree;
                inDB.LevelID = subject.LevelID;
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
        public ActionResult DeleteSubjects(int id)
        {
            try
            {
                var inDB = db.Subject.SingleOrDefault(B => B.Id == id);
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