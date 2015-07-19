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
            var rubRowID = result.Select(x=>x.RUBRIC_ROWID.ToString()).ToList();
            Session["RubRowIDS"] = rubRowID;
           
            foreach (var item in rubRowID)
            {
                Session[item] = "0-0-0-0";
            }
            return View(result);
        }

        [HttpPost]
        public ActionResult Reference(FormCollection f)
        {
           // string x = f["Course"].ToString();
            bool status = false;
            assessmentEntities assessment = new assessmentEntities();
            ASSESSMENT_DATA assessmentData = new ASSESSMENT_DATA();
            assessmentData.COURSE = f["Course"].ToString();
            assessmentData.DEPARTMENT = f["Department"].ToString();
            assessmentData.SEMESTER = f["Semester"].ToString();
            assessmentData.PROF_EMAILID = f["Instructor_Email"].ToString();
            assessmentData.OUTCOMES = f["Outcome"].ToString();

            foreach (var item in Session["RubRowIDS"] as List<String>)
            {
                string sessionData = Session[item].ToString();
                String[] data = sessionData.Split('-');
                assessmentData.RUBRIC_ROWID = Int16.Parse(item);
                assessmentData.POOR = Int16.Parse(data[0]);
                assessmentData.DEVELOPING = Int16.Parse(data[1]);
                assessmentData.DEVELOPED = Int16.Parse(data[2]);
                assessmentData.EXEMPLARY = Int16.Parse(data[3]);
                var rubID = Int16.Parse(item);
                var res = assessment.RUBRICS_DATA.Where(x => x.RUBRIC_ROWID == rubID).ToList();
                assessmentData.PERFORMANCE_INDICATOR = res.SingleOrDefault().PERFORMANCE_INDICATOR;
                assessmentData.TOPIC = res.SingleOrDefault().TOPIC;
                var save = assessment.ASSESSMENT_DATA.Add(assessmentData);
                if (assessment.SaveChanges() == 1)
                {
                    status = true;
                }
                else
                {
                    status = false; ;
                }
            }
            if (status)
            {
                return View("Success");
            }
            else
            {
                return View("error");
            }
            return View();
        }

        


        public ActionResult Submit(FormCollection f)
        {
            foreach (var item in Session["RubRowIDS"] as List<String>)
            {
                var userInput = Int16.Parse(f[item]);
                string sessionData = Session[item].ToString();
                String[] data = sessionData.Split('-');
                var result = Int16.Parse(data[userInput]) + 1;
                data[userInput] = result.ToString();
                sessionData = data[0] + "-" + data[1] + "-" + data[2] + "-" + data[3];
                Session[item] = sessionData;
            }
            return PartialView("_InstructorInput");
        }

        public ActionResult Reset()
        {
            foreach (var item in Session["RubRowIDS"] as List<String>)
            {
                Session[item] = "0-0-0-0";
            }
            return PartialView("_InstructorInput");
        }
	}
}