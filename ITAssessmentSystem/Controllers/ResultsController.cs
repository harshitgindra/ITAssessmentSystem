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
using System.Linq.Expressions;

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
        public ActionResult AllData(string param, int? page, string column, string sortOrder)
        {
            using (var context = new assessmentEntities())
            {
                IEnumerable<spASSESSMENT_GETSEARCHRESULTS_Result> results = null;
                param = string.IsNullOrEmpty(param) ? "" : param;
                //sortOrder = string.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
                ViewBag.Order = "";
                if (String.IsNullOrEmpty(column) && String.IsNullOrWhiteSpace(sortOrder))
                    results = getAllRecords(context, param).ToList();
                else
                    results = SortTable(context, param, column, sortOrder).ToList();

                var pageView = results.ToPagedList(page ?? 1, 5);
                return View("_AllData", "AllData", pageView);
            }
        }

        public IQueryable<spASSESSMENT_GETSEARCHRESULTS_Result> getAllRecords(assessmentEntities context, string param)
        {
            return context.spASSESSMENT_GETSEARCHRESULTS(param).AsQueryable();
        }

        public IQueryable<spASSESSMENT_GETSEARCHRESULTS_Result> SortTable(assessmentEntities context, string param, string column, string sortOrder)
        {

            ViewBag.Order = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            var results = context.spASSESSMENT_GETSEARCHRESULTS(param).AsQueryable();
            Func<IQueryable<spASSESSMENT_GETSEARCHRESULTS_Result>, Expression<Func<spASSESSMENT_GETSEARCHRESULTS_Result, string>>, IOrderedQueryable<spASSESSMENT_GETSEARCHRESULTS_Result>> orderBy;

            if (!sortOrder.Equals("desc"))
            {
                orderBy = Queryable.OrderBy;
            }
            else
            {
                orderBy = Queryable.OrderByDescending;
            }
            results = orderBy(results, spASSESSMENT_GETSEARCHRESULTS_Result.Order(column))
               .ThenBy(searchResults => searchResults.OUTCOMES);
            return results;
        }

        /*public void Export(string param, string colPram, string sortOrder)
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
        }*/
    }
}
