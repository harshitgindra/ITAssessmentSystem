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
    [Authorize]
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
            using (var context = new AssessmentEntities())
            {
                try
                {
                    IEnumerable<spASSESSMENT_RECORDS_Result> results = null;
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewBag.ErrorMsg = "Unable to fetch the data due to bad url";
                    return View("Error");
                }
            }
        }

        public IQueryable<spASSESSMENT_RECORDS_Result> getAllRecords(AssessmentEntities context, string param)
        {
            return context.spASSESSMENT_RECORDS(param).AsQueryable();
        }

        public IQueryable<spASSESSMENT_RECORDS_Result> SortTable(AssessmentEntities context, string param, string column, string sortOrder)
        {

            ViewBag.Order = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            var results = context.spASSESSMENT_RECORDS(param).AsQueryable();
            Func<IQueryable<spASSESSMENT_RECORDS_Result>, Expression<Func<spASSESSMENT_RECORDS_Result, string>>, IOrderedQueryable<spASSESSMENT_RECORDS_Result>> orderBy;

            if (!sortOrder.Equals("desc"))
            {
                orderBy = Queryable.OrderBy;
            }
            else
            {
                orderBy = Queryable.OrderByDescending;
            }
            results = orderBy(results, spASSESSMENT_RECORDS_Result.Order(column))
               .ThenBy(searchResults => searchResults.OUTCOMES);
            return results;
        }
    }
}