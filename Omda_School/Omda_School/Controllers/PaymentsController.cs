using CrystalDecisions.CrystalReports.Engine;
using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omda_School.Controllers
{
    public class PaymentsController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            ViewBag.ddlDepartment = db.Department.Where(ac => ac.IsDeleted != "Y").ToList();
            ViewBag.ddlYear = db.Year.Where(ac => ac.IsDeleted != "Y" ).OrderByDescending(o => o.IsCurrent).ToList();
            var payment = db.Payment.ToList();
            return View(payment);
        }
      
        [HttpPost]
        public JsonResult GetPaymentsList(int id)
        {
            try
            {
                var paymentList = db.Payment.Where(c => c.IsDeleted != "Y" && c.StudentID == id).Select(a => new
                {
                    a.Id,
                    a.Amount,
                    a.PaymentDate,
                    a.Note,
                    StudentName = a.Student.Name
                }).ToList();

                //update student paid ammount
                var inDB = db.Student.SingleOrDefault(B => B.Id == id && B.IsDeleted != "Y");
                inDB.PaidFees = db.Payment.Where(p => p.StudentID == id && p.IsDeleted != "Y").Sum(e => e.Amount);
                db.SaveChanges();

               

                return Json(paymentList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1);
            }
        }

        public JsonResult BindPayments(int id)
        {
            try
            {
                // int ID = int.Parse(id);

                var payment = db.Payment.Where(c => c.Id == id).Select(a => new
                {
                    a.Id,
                    a.Amount,
                    a.PaymentDate ,
                    a.Note,
                    a.Student.Name

                });
                return Json(payment, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult AddPayments(Payment payment)
        {
            try
            {
                payment.IsDeleted = "N";
                payment.PaymentYearDateID = db.Student.SingleOrDefault(s => s.Id == payment.StudentID).YearID;
                db.Payment.Add(payment);
                db.SaveChanges();
                return Json(new { code = payment.Id  });
            }
            catch (DbEntityValidationException e)
            {
                return Json(new { code = 0 });
            }

        }
        [HttpPost]
        public ActionResult EditPayments(Payment payment)
        {
            try
            {
                var inDB = db.Payment.SingleOrDefault(B => B.Id == payment.Id);
                inDB.Amount = payment.Amount;
                inDB.PaymentDate = payment.PaymentDate;
                inDB.Note = payment.Note;
                inDB.IsDeleted = "N" ;
                db.SaveChanges();
                return Json(new { code = payment.Id });
            }
            catch (Exception)
            {
                return Json(new { code = 0 });
            }
        }
        [HttpPost]
        public ActionResult DeletePayments(int id)
        {
            try
            {
                var inDB = db.Payment.SingleOrDefault(B => B.Id == id);
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
        public ActionResult ExportRecipt(int id)
        {
            string SerailNumber = "000000";
            string voucherNumber = id.ToString();
            if (SerailNumber.Length < voucherNumber.Length)
                SerailNumber += SerailNumber; 
            SerailNumber = SerailNumber.Substring(0, SerailNumber.Length - voucherNumber.Length) + voucherNumber;
            var paymentList = db.Payment.Where(c => c.IsDeleted != "Y" && c.Id == id).Select(a => new
            {
                YearName = a.Student.Year.Name,
                LevelName = a.Student.Level.Name,
                DepartmentName = a.Student.Level.Department.Name,
                PaidFees = a.Amount,
                PaymentNote = a.Note,
                a.PaymentDate,
                StudentName = a.Student.Name ,
                SerialNumber = SerailNumber
            }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptPaymentReciept.rpt"));
            rd.SetDataSource(paymentList);
            // Response.Buffer = false;
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            byte[] byteArray = new byte[stream.Length];
            stream.Read(byteArray, 0, Convert.ToInt32(stream.Length - 1));
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(byteArray);
            Response.Flush();
            Response.Close();
            rd.Close();
            rd.Dispose();
            stream.Seek(0, SeekOrigin.Begin);
            return null; //  File(stream, "application/pdf", "CustomerList.pdf");
        }
    }
}