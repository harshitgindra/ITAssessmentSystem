using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace ITAssessmentSystem.Controllers
{
    public class ResultsController : Controller
    {
        //
        // GET: /Results/
        
        //Reference: https://stackoverflow.com/questions/12844779/search-on-all-fields-of-an-entity
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult AllData(string param, int? page, string colPram, string sortOrder)
        {
            IEnumerable<spASSESSMENT_GETSEARCHRESULTS_Result> results = null;
            param = string.IsNullOrEmpty(param) ? "" : param;
            ViewBag.Order = "";
            if (String.IsNullOrEmpty(colPram) && String.IsNullOrWhiteSpace(sortOrder))
                results = getAllRecords(param);
            else
                results = SortTable(param, colPram, sortOrder).ToList();

            var pageView = results.ToPagedList(page ?? 1, 5);
            return View("_AllData", "AllData", pageView);
        }

        public IEnumerable<spASSESSMENT_GETSEARCHRESULTS_Result> getAllRecords(string param)
        {
            using (var context = new assessmentEntities())
            {
                return (IList<spASSESSMENT_GETSEARCHRESULTS_Result>)context.spASSESSMENT_GETSEARCHRESULTS(param).ToList();
            }

        }

        public IEnumerable<spASSESSMENT_GETSEARCHRESULTS_Result> SortTable(string param, string colPram, string sortOrder)
        {
            using (var context = new assessmentEntities())
            {
                ViewBag.Order = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
                string colAndOrder = colPram + " " + sortOrder;
                return (IList<spASSESSMENT_GETSEARCHRESULTS_Result>)context.spASSESSMENT_GETSEARCHRESULTS(param).ToList();               
                
            }

        }


        public void Export(string param, string colPram, string sortOrder)
        {
            IEnumerable<spASSESSMENT_GETSEARCHRESULTS_Result> results = null;
            param = string.IsNullOrEmpty(param) ? "" : param;
            if (String.IsNullOrEmpty(colPram) && String.IsNullOrWhiteSpace(sortOrder))
            {
                results = getAllRecords(param);
            }
            else
            {
                results = SortTable(param, colPram, sortOrder);
            }
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Export.csv");
            Response.ContentType = "text/csv";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            sw.WriteLine(string.Format("\"{0}\",\"{1}\", \"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\"",
                         "Professor Email ID",
                         "DEPARTMENT",
                         "SEMESTER",
                         "COURSE",
                         "PERFORMANCE_INDICATOR",
                         "TOPIC",
                         "POOR",
                         "DEVELOPED",
                         "DEVELOPING",
                         "EXEMPLARY"));
            foreach (var item in results)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\", \"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\"",
                    item.INSTRUCTOR_EMAILID,
                    item.department_desc,
                    item.SEMESTER,
                    item.COURSE,
                    item.PERFORMANCE_INDICATOR,
                    item.TOPIC,
                    item.POOR,
                    item.DEVELOPED,
                    item.DEVELOPING,
                    item.EXEMPLARY
                ));
            }
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}
