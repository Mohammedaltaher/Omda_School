using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class ResultsController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            ViewBag.ddlDepartment = db.Department.Where(ac => ac.IsDeleted != "Y").ToList();
            ViewBag.ddlYear = db.Year.Where(ac => ac.IsDeleted != "Y").OrderByDescending(o => o.IsCurrent).ToList();
            var result = db.Result.ToList();
            return View(result);
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
        public JsonResult GetResultsList(Search search)
        {
            try
            {
                var resultList = db.Result.Where(c => c.IsDeleted != "Y").Select(a => new
                {
                    a.Id,
                    a.Type,
                    a.LevelAdmin,
                    LevelName = a.Level.Name,
                    YearName = a.Year.Name,
                    DepartmentName = a.Level.Department.Name,
                    a.Level.DepartmentID ,
                    a.LevelID ,
                    a.YearID 

                }).ToList();
                if (search.DepartmentID != 0) resultList = resultList.Where(a => a.DepartmentID == search.DepartmentID).ToList();
                if (search.LevelID != 0) resultList = resultList.Where(a => a.LevelID == search.LevelID).ToList();
                if (search.YearID != 0) resultList = resultList.Where(a => a.YearID == search.YearID).ToList();
                if (search.StudentName != null) resultList = resultList.Where(a => a.LevelAdmin.Contains(search.StudentName)).ToList();
                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1);
            }
        }

        public JsonResult BindResults(int id)
        {
            try
            {
                // int ID = int.Parse(id);

                var result = db.Result.Where(c => c.Id == id).Select(a => new
                {
                    a.Id,
                    a.Type,
                    a.LevelAdmin,
                    a.YearID,
                    a.LevelID ,
                    a.Level.DepartmentID

                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult AddResults(Result result)
        {
            try
            {
                result.IsDeleted = "N";
                db.Result.Add(result);
                db.SaveChanges();
                intitResult(result);
                return Json(new { code = 1 });
            }
            catch (DbEntityValidationException e)
            {
                return Json(new { code = 0 });
                // throw;
            }

        }
        [HttpPost]
        public ActionResult EditResults(Result result)
        {
            try
            {
                var inDB = db.Result.SingleOrDefault(B => B.Id == result.Id);
                inDB.Type = result.Type;
                inDB.LevelAdmin = result.LevelAdmin;
                inDB.LevelID = result.LevelID;
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
        public ActionResult DeleteResults(int id)
        {
            try
            {
                var inDB = db.Result.SingleOrDefault(B => B.Id == id);
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
        public void intitResult(Result result)
        {
            var resultList = db.Student.Where(c => c.IsDeleted != "Y" && c.YearID == result.YearID && c.LevelID == result.LevelID).Select(a => new { a.Id }).ToList();
            var SubjectList = db.Subject.Where(c => c.IsDeleted != "Y" && c.LevelID == result.LevelID).Select(a => new { a.Id }).ToList();
            var intitDegree = 0;
            foreach (var subject in SubjectList)
            {
                foreach (var student in resultList)
                {
                    var degree = new Degree
                    {
                        SubjectID = subject.Id,
                        StudentID = student.Id,
                        ResultID = result.Id,
                        StudentDegree = intitDegree,
                        IsDeleted = "N"
                    };
                    db.Degree.Add(degree);
                    db.SaveChanges();
                }
            }

        }
    }
}