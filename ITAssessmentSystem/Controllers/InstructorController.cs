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
            using (var context = new assessmentEntities())
            {
                try
                {
                    var result = context.spInstructorAddNew(userinfo.PROF_NAME, userinfo.PROF_EMAILID);
                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View("Error");
                }
            }
        }

        public ActionResult AllInstructors()
        {

            using (var context = new assessmentEntities())
            {
                try
                {
                    var result = (IEnumerable<spINSTRUCTORGETALLRECORDS_Result>)context.spINSTRUCTORGETALLRECORDS().ToList();
                    return View(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View("Error");
                }
            }

        }


        public ActionResult Edit(string email)
        {
            using (var context = new assessmentEntities())
            {
                try
                {
                    var result = context.spINSTRUCTORGETDETAILS(email).SingleOrDefault();
                    return View(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View("Error");
                }
            }

        }

        [HttpPost]
        [ActionName("EditRecord")]
        public ActionResult Edit_Save(USER_INFO userinfo)
        {
            using (var context = new assessmentEntities())
            {
                try
                {
                    var i = context.spINSTRUCTOREDIT(userinfo.PROF_EMAILID, userinfo.PROF_NAME);
                    return RedirectToAction("AllInstructors");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View("Error");
                }
            }
        }

        [HttpPost]
        [ActionName("DeleteRecord")]
        public ActionResult Delete(string email)
        {
            using (var context = new assessmentEntities())
            {
                try
                {
                    var result = context.spINSTRUCTORDELETE(email);
                    return RedirectToAction("AllInstructors");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View("Error");
                }
                finally
                {

                }
            }
        }
    }
}