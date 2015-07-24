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


        public ActionResult Edit(string id)
        {
            using (var context = new assessmentEntities())
            {
                USER_INFO instructor = context.USER_INFO.Where(inst => inst.PROF_EMAILID.Equals(id)).SingleOrDefault();
                return View(instructor);
            }
            
        }

        [HttpPost]
        public ActionResult Edit(USER_INFO userinfo)
        {
            using (var context = new assessmentEntities())
            {
                 context.USER_INFO.Where(c => c.PROF_EMAILID.Equals(userinfo.PROF_EMAILID))
                    .ToList().ForEach(x => x.PROF_NAME = userinfo.PROF_NAME);
                 var results = context.SaveChanges();
            }
            return View();
        }

       

	}
}