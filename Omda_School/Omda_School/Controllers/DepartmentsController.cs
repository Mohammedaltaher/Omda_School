using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class DepartmentsController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            var department = db.Department.ToList();
            return View(department);
        }
        [HttpPost]
        public JsonResult GetDepartmentsList()
        {
            try
            {
                var departmentList = db.Department.Where(c => c.IsDeleted != "Y").Select(a => new
                {
                    a.Id,
                    a.Name 
                }).ToList();

                return Json(departmentList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1);
            }
        }

        public JsonResult BindDepartments(int id)
        {
            try
            {
                // int ID = int.Parse(id);

                var department = db.Department.Where(c => c.Id == id).Select(a => new
                {
                    a.Id,
                    a.Name

                });
                return Json(department, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult AddDepartments(Department department)
        {
            try
            {
                department.IsDeleted = "N";
                db.Department.Add(department);
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
        public ActionResult EditDepartments(Department department)
        {
            try
            {
                var inDB = db.Department.SingleOrDefault(B => B.Id == department.Id);
                inDB.Name = department.Name;
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
        public ActionResult DeleteDepartments(int id)
        {
            try
            {
                var inDB = db.Department.SingleOrDefault(B => B.Id == id);
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