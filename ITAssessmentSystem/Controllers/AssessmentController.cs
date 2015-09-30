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
            string linkid;
            using (var context = new AssessmentEntities())
            {
                try
                {
                    linkid = Request.QueryString["linkid"].ToString();
                    var VerifyLink = context.ASSESSMENT_LINK.Where(link => link.RANDOM_STRING == linkid).ToList();

                    if (VerifyLink.Count == 1)
                    {
                        var linkDetails = VerifyLink.First();

                        var result = context.spRUBRICGETSEARCHRESULTS(linkDetails.DEPARTMENT_CD, linkDetails.OUTCOMES).ToList();
                        var rubRowID = result.Select(rubrowID => rubrowID.RUBRIC_ROWID.ToString()).ToList();
                        Session["RubRowIDS"] = rubRowID;
                        foreach (var item in rubRowID)
                        {
                            Session[item] = "0-0-0-0";
                        }
                        result.First().Instructor_EmailID = linkDetails.INSTRUCTOR_EMAILID;
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

        public ActionResult Reference(ASSESSMENT_DATA assessedData)
        {
            bool status = false;
            using (var context = new AssessmentEntities())
            {
                try
                {
                    if (context.ASSESSMENT_DATA.Where(rdmstring => rdmstring.RANDOM_STRING == assessedData.RANDOM_STRING).ToList().Count != 0)
                    {
                        //link is already been used
                        //delete old records
                        context.spASSESSMENT_DELETE_OLD_RECORDS(assessedData.RANDOM_STRING);
                        context.SaveChanges();
                    }

                    foreach (var item in Session["RubRowIDS"] as List<String>)
                    {
                        string sessionData = Session[item].ToString();
                        String[] data = sessionData.Split('-');
                        assessedData.RUBRIC_ROWID = Int16.Parse(item);
                        assessedData.POOR = Int16.Parse(data[0]);
                        assessedData.DEVELOPING = Int16.Parse(data[1]);
                        assessedData.DEVELOPED = Int16.Parse(data[2]);
                        assessedData.EXEMPLARY = Int16.Parse(data[3]);
                        if (assessedData.POOR + assessedData.DEVELOPING + assessedData.DEVELOPED + assessedData.EXEMPLARY != 0)
                        {
                            context.ASSESSMENT_DATA.Add(assessedData);
                            context.SaveChanges();

                            context.spLINK_UPDATESTATUS(assessedData.RANDOM_STRING, false);
                            context.SaveChanges();

                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Please input at least one record for assessment";
                            return View("Error");
                        }
                    }
                    return View("Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewBag.ErrorMsg = "Please complete all form fields.";
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


        [HttpPost]
        
        public ActionResult InsertRecord(FormCollection f)
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