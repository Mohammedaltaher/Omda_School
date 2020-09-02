using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class ReciptsController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            var year = db.Year.ToList();
            return View(year);
        }
        [HttpPost]
        public JsonResult GetYearsList()
        {
            try
            {
                var yearList = db.Year.Where(c => c.IsDeleted != "Y").Select(a => new
                {
                    a.Id,
                    a.IsCurrent ,
                    a.Name 
                }).ToList();

                return Json(yearList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1);
            }
        }

        public JsonResult BindYears(int id)
        {
            try
            {
                // int ID = int.Parse(id);

                var year = db.Year.Where(c => c.Id == id).Select(a => new
                {
                    a.Id,
                    a.IsCurrent,
                    a.Name

                });
                return Json(year, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult AddYears(Year year)
        {
            try
            {
                year.IsDeleted = "N";
                if(year.IsCurrent == "Y")
                {
                    var inDB = db.Year;
                    foreach (var item in inDB)
                    {
                        item.IsCurrent = "N";
                    }
                    db.SaveChanges();
                }
                db.Year.Add(year);
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
        public ActionResult EditYears(Year year)
        {
            try
            {
                if (year.IsCurrent == "Y")
                {
                    var inDB1 = db.Year;
                    foreach (var item in inDB1)
                    {
                        item.IsCurrent = "N";
                    }
                    db.SaveChanges();
                }
                var inDB = db.Year.SingleOrDefault(B => B.Id == year.Id);
                inDB.Name = year.Name;
                inDB.IsCurrent= year.IsCurrent;
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
        public ActionResult DeleteYears(int id)
        {
            try
            {
                var inDB = db.Year.SingleOrDefault(B => B.Id == id);
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