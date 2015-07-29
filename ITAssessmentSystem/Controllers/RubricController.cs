using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    public class RubricController : Controller
    {
        //
        // GET: /Rubric/

        //Reference for Dropdownlist :https://stackoverflow.com/questions/16594958/how-to-use-a-viewbag-to-create-a-dropdownlist
        private readonly string link = "http://localhost:52704/Assessment/Reference?";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            DepartmentList departmentList = new DepartmentList();
            ViewBag.Departments = departmentList.getDepartmentList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(RUBRICS_DATA rubricData)
        {
            if (ModelState.IsValid)
            {
                using (var context = new assessmentEntities())
                {
                    try
                    {
                        context.RUBRICS_DATA.Add(rubricData);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        return View("Error");
                    }
                    finally
                    {
                        DepartmentList departmentList = new DepartmentList();
                        ViewBag.Departments = departmentList.getDepartmentList();
                    }
                }
                return View();
            }
            else
            {
                return View("Error");
            }
        }


        public ActionResult AllRubrics()
        {
            if (ModelState.IsValid)
            {
                using (var context = new assessmentEntities())
                {
                    DepartmentList departmentList = new DepartmentList();
                    ViewBag.Departments = departmentList.getDepartmentList();
                }
                return View();
            }
            return View("Error");
        }

        public ActionResult GetPerformanceIndicator(string dept)
        {
            PerformanceIndicator pIndicator = new PerformanceIndicator();
            ViewBag.PIndicator = pIndicator.getPerformanceIndicatorList(dept);
            Session["Dept"] = dept;
            return PartialView("_PerformanceIndicator");
        }

        public ActionResult Search(FormCollection f)
        {
            using (var context = new assessmentEntities())
            {
                string Outcomes = f["Outcomes"].ToString();
                string deptSelected = f["DEPARTMENT"].ToString();
                var result = context.spRUBRICGETSEARCHRESULTS(deptSelected, Outcomes).ToList();
                //saving variables for emailing
                Session["Outcome"] = Outcomes;
                Session["DEPARTMENT"] = deptSelected;

                return PartialView("_Search", result);
            }
        }

        public ActionResult Send()
        {
            ProfessorList professorList = new ProfessorList();
            ViewBag.professorList = professorList.getProfessorListList();
           
            return View();
        }

        [HttpPost]
        public ActionResult Send(FormCollection f)
        {
            string department = Session["DEPARTMENT"].ToString();
            string outcome = Session["Outcome"].ToString();
            string instructorSelected = f["InstructorList"].ToString();
            string urlParameters = "Dept=" + department + "&outcome=" + outcome + "&inst=" + instructorSelected;
            string finalLink = link + urlParameters;
            SendMail sendmail = new SendMail();
            if (sendmail.SendEmail(instructorSelected, "Assessment Form", sendmail.MailBody(finalLink)))
            {
                //mail sending currently commented out
                ViewBag.link = finalLink;
                return View("MailResult");
            }
            else
            {
                ViewBag.link = "Something went wrong. Retry";
                return View("Error");
            }
        }
    }
}
