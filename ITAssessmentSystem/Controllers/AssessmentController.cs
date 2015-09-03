using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            ViewBag.total = 0;
            string dept, outcome, instructor;
            using (var context = new assessmentEntities())
            {
                try
                {
                    dept = Request.QueryString["Dept"].ToString();
                    outcome = Request.QueryString["outcome"].ToString();
                    instructor = Request.QueryString["inst"].ToString();

                    if ((context.spASSESSMENT_VERIFYINSTRUCTOR(instructor).SingleOrDefault() != 0) && (context.spASSESSMENT_VERIFYOUTCOME_DEPT(outcome, dept).SingleOrDefault() != 0))
                    {
                        var result = context.spRUBRICGETSEARCHRESULTS(dept, outcome).ToList();
                        var rubRowID = result.Select(rubrowID => rubrowID.RUBRIC_ROWID.ToString()).ToList();
                        Session["RubRowIDS"] = rubRowID;
                        foreach (var item in rubRowID)
                        {
                            Session[item] = "0-0-0-0";

                        }
                        return View(result);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invalid URL. Please check the Link again";
                        return View("Error");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMsg = "Unable to fetch data. Please contact the Assessment officer";
                    return View("Error");
                }
            }

        }

        [HttpPost]
        public ActionResult Reference(FormCollection f)
        {

            bool status = false;
            using (var context = new assessmentEntities())
            {

                try
                {
                    ASSESSMENT_DATA assessmentData = new ASSESSMENT_DATA();
                    assessmentData.COURSE = f["Course"].ToString();
                    assessmentData.DEPARTMENT_CD = f["Department"].ToString();
                    assessmentData.SEMESTER = f["Semester"].ToString();
                    assessmentData.INSTRUCTOR_EMAILID = f["Instructor_Email"].ToString();
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
                        var rubricRecord = context.spRUBRICSGETRECORD_RUBID(rubID).ToList();
                        assessmentData.PERFORMANCE_INDICATOR = rubricRecord.SingleOrDefault().PERFORMANCE_INDICATOR;
                        assessmentData.TOPIC = rubricRecord.SingleOrDefault().TOPIC;
                        var result = context.ASSESSMENT_DATA.Add(assessmentData);
                        context.SaveChanges();
                    }
                    return View("Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewBag.ErrorMsg = "Unable to save the data. Please retry again";
                    return View("Error");
                }
            }
        }

        public bool VerifyInstructorInput(FormCollection formCollection, List<String> RubRowIDS)
        {
            bool isValid = true;
            if (RubRowIDS != null)
            {
                foreach (var item in RubRowIDS)
                {
                    if (String.IsNullOrEmpty(formCollection[item]))
                    {
                        isValid = false;
                    }
                }
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }


        public ActionResult Submit(FormCollection f)
        {
            if (VerifyInstructorInput(f, Session["RubRowIDS"] as List<String>))
            {
                foreach (var item in Session["RubRowIDS"] as List<String>)
                {
                    try
                    {
                        var userInput = Int16.Parse(f[item]);
                        string sessionData = Session[item].ToString();
                        String[] data = sessionData.Split('-');
                        var result = Int16.Parse(data[userInput]) + 1;
                        data[userInput] = result.ToString();
                        sessionData = data[0] + "-" + data[1] + "-" + data[2] + "-" + data[3];
                        Session[item] = sessionData;
                        ViewBag.total = Int16.Parse(data[0]) + Int16.Parse(data[1]) + Int16.Parse(data[2]) + Int16.Parse(data[3]);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        ViewBag.ErrorMsg = "Something went wrong. Please retry.";
                        return View("Error");
                    }
                }
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