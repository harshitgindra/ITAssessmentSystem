using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    [Authorize]
    public class RubricController : Controller
    {
        //
        // GET: /Rubric/

        //private readonly string link = "http://localhost:52704/Assessment/Reference?";
        private readonly string link = "http://iis.it.ilstu.edu/Assessment/Assessment/Reference?linkid=";

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Create()
        {
            using (var context = new AssessmentEntities())
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
                using (var context = new AssessmentEntities())
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
                using (var context = new AssessmentEntities())
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
            using (var context = new AssessmentEntities())
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
            using (var context = new AssessmentEntities())
            {
                
                string department = Session["DEPARTMENT"].ToString();
                string outcome = Session["Outcome"].ToString();
                string instructorSelected = f["InstructorList"].ToString();
                string urlParameters = "Dept=" + department + "&outcome=" + outcome + "&inst=" + instructorSelected;
                string rdm_string = RandomString(30);
                context.spLINK_INSERT(rdm_string, outcome, department, instructorSelected, true);
                context.SaveChanges();
                string finalLink = link + rdm_string;
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

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet]
        public ActionResult Edit(int rubID)
        {
            using (var context = new AssessmentEntities())
            {
                var results = context.RUBRICS_DATA.Where(rubid => rubid.RUBRIC_ROWID == rubID).SingleOrDefault();
                return PartialView("_EditRubric", results);
            }
        }

        [HttpPost]
        public ActionResult SaveRubric(RUBRICS_DATA rubricData)
        {
            using (var context = new AssessmentEntities())
            {
                context.spUPDATERUBRICDATA(rubricData.RUBRIC_ROWID, rubricData.OUTCOMES, rubricData.PERFORMANCE_INDICATOR, rubricData.TOPIC, rubricData.POOR, rubricData.DEVELOPING, rubricData.DEVELOPED, rubricData.EXEMPLARY, rubricData.EXPECTATION_LEVEL);
                context.SaveChanges();
                var rubricRecord = context.spRUBRICSGETRECORD_RUBID(rubricData.RUBRIC_ROWID).SingleOrDefault();
                return EditCancel(rubricData.RUBRIC_ROWID);
            }
        }


        public PartialViewResult getRubricDetails(spRUBRICSGETRECORD_RUBID_Result rubricRecord, AssessmentEntities context)
        {
            var results = context.spRUBRICGETSEARCHRESULTS(rubricRecord.DEPARTMENT_CD, rubricRecord.OUTCOMES).SingleOrDefault();
            return PartialView("_Search", results);
        }

        public PartialViewResult EditCancel(int rowID)
        {
            using (var context = new AssessmentEntities())
            {
                var rubricRecord = context.spRUBRICSGETRECORD_RUBID(rowID).SingleOrDefault();
                spRUBRICGETSEARCHRESULTS_Result rubDetails = new spRUBRICGETSEARCHRESULTS_Result();
                rubDetails.RUBRIC_ROWID = rubricRecord.RUBRIC_ROWID;
                rubDetails.PERFORMANCE_INDICATOR = rubricRecord.PERFORMANCE_INDICATOR;
                rubDetails.TOPIC = rubricRecord.TOPIC;
                rubDetails.OUTCOMES = rubricRecord.OUTCOMES;
                rubDetails.POOR = rubricRecord.POOR;
                rubDetails.DEVELOPING = rubricRecord.DEVELOPING;
                rubDetails.DEVELOPED = rubricRecord.DEVELOPED;
                rubDetails.EXEMPLARY = rubricRecord.EXEMPLARY;
                rubDetails.EXPECTATION_LEVEL = rubricRecord.EXPECTATION_LEVEL;
                return PartialView("_RubricRows", rubDetails);
            }
        }

        public void DeleteRubricRecord(int rubID)
        {
            using (var context = new AssessmentEntities())
            {
                context.spRUBRIC_DELETE_RUBRICROW(rubID);
                context.SaveChanges();
            }
        }
    }
}
