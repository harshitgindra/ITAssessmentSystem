using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    public class InstructorController : Controller
    {
        //
        // GET: /Instructor/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Insert()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Insert(USER_INFO userinfo)
        {
            assessmentEntities assessment = new assessmentEntities();
            try
            {
                assessment.USER_INFO.Add(userinfo);
                if (assessment.SaveChanges() == 1)
                    Response.Write("Professor Record inserted Successfully");

                else
                    Response.Write("Something went wrong. Please retry");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View();
        }

        public ActionResult AllInstructors()
        {
            assessmentEntities assessment = new assessmentEntities();
            var result = assessment.USER_INFO
                .ToList();

            return View(result);
        }

       

	}
}