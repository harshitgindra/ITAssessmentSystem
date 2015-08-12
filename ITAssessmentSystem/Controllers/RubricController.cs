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
            using (var context = new assessmentEntities())
            {
                DepartmentList departmentList = new DepartmentList();
                ViewBag.Departments = departmentList.getDepartmentList(context);
                return View();
            }

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
                        ViewBag.InsertionResult = "Rubric Successfully inserted.";
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        ViewBag.ErrorMsg = "Performance Indicator already Exist. Please check the input again";
                        return View("Error");
                    }
                    finally
                    {
                        DepartmentList departmentList = new DepartmentList();
                        ViewBag.Departments = departmentList.getDepartmentList(context);
                    }
                }
                return PartialView("_InsertResult");
            }
            else
            {
                ViewBag.ErrorMsg = "Invalid Input entered.";
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
                    ViewBag.Departments = departmentList.getDepartmentList(context);
                }
                return View();
            }
            ViewBag.ErrorMsg = "Invalid option selected. No such record exist";
            return View("Error");
        }

        public ActionResult GetPerformanceIndicator(string dept)
        {
            PerformanceIndicator pIndicator = new PerformanceIndicator();
            ViewBag.PIndicator = pIndicator.getPerformanceIndicatorList(dept);
            Session["Dept"] = dept;
            return PartialView("_PerformanceIndicator");
        }

        [HttpPost]
        public ActionResult Search(FormCollection f)
        {
            using (var context = new assessmentEntities())
            {
                try
                {
                    string Outcomes = f["Outcomes"].ToString();
                    string deptSelected = f["DEPARTMENT"].ToString();
                    var result = context.spRUBRICGETSEARCHRESULTS(deptSelected, Outcomes).ToList();
                    //saving variables for emailing
                    Session["Outcome"] = Outcomes;
                    Session["DEPARTMENT"] = deptSelected;
                    return PartialView("_Search", result);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    ViewBag.ErrorMsg = "Bad selection. No such rubric exist.";
                    return View("Error");
                }
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
                ViewBag.ErrorMsg = "Unable to send the link to the Instructor. Please check the network connections or else try sending the link personally. Here it the link below \n" +
                    "<h3>" + finalLink + "</h3>";
                return View("Error");
            }
        }
    }
}
