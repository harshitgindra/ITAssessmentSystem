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
        IEnumerable<ASSESSMENT_DATA> results = null;
        //Reference: https://stackoverflow.com/questions/12844779/search-on-all-fields-of-an-entity
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AllData(string param, int? page, string colPram, string sortOrder)
        {

            param = string.IsNullOrEmpty(param) ? "" : param;
            ViewBag.Order = "";
            if (String.IsNullOrEmpty(colPram) && String.IsNullOrWhiteSpace(sortOrder))
                results = getAllRecords(param);
            else
                results = SortTable(param, colPram, sortOrder);

            var pageView = results.ToPagedList(page ?? 1, 5);
            return View("_AllData", "AllData", pageView);
        }

        public IEnumerable<ASSESSMENT_DATA> getAllRecords(string param)
        {
            using (var context = new assessmentEntities())
            {
                /*var stringProperties = typeof(ASSESSMENT_DATA).GetProperties().Where(prop =>
                    prop.PropertyType == param.GetType());
                var results = context.ASSESSMENT_DATA.Where(customer => 
                        stringProperties.Any(prop =>
                           prop.GetValue(customer, null) == param)).ToList();*/

                results = context.ASSESSMENT_DATA.Where(s => s.COURSE.Contains(param)
                    || s.DEPARTMENT.Contains(param)
                    || s.PERFORMANCE_INDICATOR.Contains(param)
                    || s.PROF_EMAILID.Contains(param)
                    || s.SEMESTER.Contains(param)
                    || s.TOPIC.Contains(param)
                    || s.USER_INFO.PROF_NAME.Contains(param))
                    .OrderBy(s => s.PERFORMANCE_INDICATOR)
                    .ToList();
                return results;
            }

        }

        public IEnumerable<ASSESSMENT_DATA> SortTable(string param, string colPram, string sortOrder)
        {
            using (var context = new assessmentEntities())
            {
                ViewBag.Order = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
                string colAndOrder = colPram + " " + sortOrder;
                results = context.ASSESSMENT_DATA.Where(s => s.COURSE.Contains(param)
                        || s.DEPARTMENT.Contains(param)
                        || s.PERFORMANCE_INDICATOR.Contains(param)
                        || s.PROF_EMAILID.Contains(param)
                        || s.SEMESTER.Contains(param)
                        || s.TOPIC.Contains(param)
                        || s.USER_INFO.PROF_NAME.Contains(param))
                        .OrderBy(colAndOrder)

                        .ToList();
                return results;
            }

        }


        public void Export(string param, string colPram, string sortOrder)
        {
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
                    item.PROF_EMAILID,
                    item.DEPARTMENT,
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
