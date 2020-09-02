using Omda_School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace Omda_School.Controllers
{
    public class TestRPsController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            var year = db.Year.ToList();
            return View(year);
        }
        public ActionResult ExportCustomers()
        {
            List<Year> allCustomer = new List<Year>();
            allCustomer = db.Year.ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "TestRPT.rpt"));

            rd.SetDataSource(allCustomer);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "CustomerList.pdf");
        }

    }
}