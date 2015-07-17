using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    public class RubricController : Controller
    {
        //
        // GET: /Rubric/

        //Reference for Dropdownlist :https://stackoverflow.com/questions/16594958/how-to-use-a-viewbag-to-create-a-dropdownlist


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RUBRICS_DATA rub)
        {
            if (ModelState.IsValid)
            {
                assessmentEntities assessment = new assessmentEntities();
                try
                {
                    assessment.RUBRICS_DATA.Add(rub);
                    if (assessment.SaveChanges() == 1)
                        Response.Write("Rubric inserted Successfully");

                    else
                        Response.Write("Something went wrong. Please retry");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
            return View();
        }

        public ActionResult AllRubrics()
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            return View("Error");
        }

        public ActionResult GetPerformanceIndicator(string id)
        {
            PerformanceIndicator o = new PerformanceIndicator();
            assessmentEntities assessment = new assessmentEntities();
            List<string> res = assessment.RUBRICS_DATA.Where(x => x.DEPARTMENT.Equals(id)).Select(x => x.PERFORMANCE_INDICATOR).Distinct().ToList();
            foreach (var item in res)
            {
                o.selection.Add(new SelectListItem() { Text = item, Value = item });
            }
            ViewBag.PIndicator = o.selection;
            Session["Dept"] = id;
            return PartialView("_PerformanceIndicator", o);
        }

        public ActionResult Search(FormCollection f)
        {
            assessmentEntities assessment = new assessmentEntities();
           
            string PIndicator = f["PIndicators"].ToString();
            string deptSelected = f["DEPARTMENT"].ToString();
            
            
            var result = assessment.RUBRICS_DATA
                .Where(x => x.DEPARTMENT.Equals(deptSelected))
                .Where(x => x.PERFORMANCE_INDICATOR.Equals(PIndicator))
                .ToList();
            Session["Outcome"] = result[0].OUTCOMES;
            Session["DEPARTMENT"] = deptSelected;
            return PartialView("_Search", result);
        }

        public ActionResult Send()
        {
            string x = Session["DEPARTMENT"].ToString();
            string y = Session["Outcome"].ToString();
            string link = "http://localhost:52704/Assessment/Reference?Dept=" + x + "&outcome=" + y;
            ProfessorList professorList = new ProfessorList();
            foreach (var item in professorList.getProfList())
            {
                professorList.selection.Add(new SelectListItem() { Text = item.PROF_NAME, Value = item.PROF_EMAILID });
            }
            ViewBag.professorList = professorList.selection;
            ViewBag.link = link;

            return View();
        }

        [HttpPost]
        public ActionResult Send(FormCollection f)
        {
            string instructorSelected = f["InstructorList"].ToString();
            string rubricLink = f["Link"].ToString();
            SendMail sendmail = new SendMail();
            if (sendmail.SendEmail(instructorSelected, "Assessment Form", sendmail.MailBody(rubricLink)))
            {
                return View();
            }
            else
            {
                return View("Index");
            }
            
        }


    }
}
