using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class LevelsController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            ViewBag.ddlDepartment = db.Department.Where(ac => ac.IsDeleted != "Y").ToList();
            var level = db.Level.ToList();
            return View(level);
        }
        [HttpPost]
        public JsonResult GetLevelsList(Search search)
        {
            try
            {
                var levelList = db.Level.Where(c => c.IsDeleted != "Y").Select(a => new
                {
                    a.Id,
                    a.Name  ,
                    DepartmentName = a.Department.Name ,
                    a.DepartmentID
                }).ToList();
                if (search.DepartmentID != 0) levelList = levelList.Where(a => a.DepartmentID == search.DepartmentID).ToList();
                return Json(levelList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1);
            }
        }

        public JsonResult BindLevels(int id)
        {
            try
            {
                // int ID = int.Parse(id);

                var level = db.Level.Where(c => c.Id == id).Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.DepartmentID

                });
                return Json(level, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult AddLevels(Level level)
        {
            try
            {
                level.IsDeleted = "N";
                db.Level.Add(level);
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
        public ActionResult EditLevels(Level level)
        {
            try
            {
                var inDB = db.Level.SingleOrDefault(B => B.Id == level.Id);
                inDB.Name = level.Name;
                inDB.DepartmentID = level.DepartmentID;
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
        public ActionResult DeleteLevels(int id)
        {
            try
            {
                var inDB = db.Level.SingleOrDefault(B => B.Id == id);
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