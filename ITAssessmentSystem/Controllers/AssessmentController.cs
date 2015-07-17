using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    public class AssessmentController : Controller
    {
        //
        // GET: /Assessment/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reference()
        {
            string dept, outcome;
            try
            {
                dept = Request.QueryString["Dept"].ToString();
                outcome = Request.QueryString["outcome"].ToString();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            
            assessmentEntities assessment = new assessmentEntities();
            var result = assessment.RUBRICS_DATA
                .Where(x => x.DEPARTMENT.Equals(dept))
                .Where(y => y.OUTCOMES.Equals(outcome))
                .ToList();
            return View(result);
        }

        public ActionResult Insert()
        {
            string dept = "CS", outcome = "C-2";
            assessmentEntities assessment = new assessmentEntities();
            var result = assessment.RUBRICS_DATA
                .Where(x => x.DEPARTMENT.Equals(dept))
                .Where(y => y.OUTCOMES.Equals(outcome))
                .Select(z=>z.TOPIC)
                .ToList();
            Session["Topics"] = result;

            foreach (var item in result)
            {
                Session[item] = "0-0-0-0";
            }

            return PartialView("_InsertData");

        }

       
        public ActionResult Submit(FormCollection f)
        {
            
            foreach (var item in Session["Topics"] as List<String>)
            {
                var userInput = Int16.Parse(f[item]);
                string sessionData = Session[item].ToString();
                String[] data = sessionData.Split('-');
                var result = Int16.Parse(data[userInput]) + 1;
                data[userInput] = result.ToString();
                sessionData = data[0] + "-" + data[1] + "-" + data[2] + "-" + data[3];
                Session[item] = sessionData;
            }

            
            return View();
        }
	}
}