using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    [AuthorizeUser(Users = "hgindra")]
    public class InstructorController : Controller
    {
        //
        // GET: /Instructor/
        
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
                    var result = context.spInstructorAddNew(userinfo.INSTRUCTOR_NAME, userinfo.INSTRUCTOR_EMAILID);
                    return RedirectToAction("AllInstructors");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewBag.ErrorMsg = "Instructor Email ID already exist in the Database. Please use different one.";
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
                    ViewBag.ErrorMsg = "Something went wrong while fetching. Please retry after some time.";
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
                    ViewBag.ErrorMsg = "Unable to fetch the record. Please retry again.";
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
                    var i = context.spINSTRUCTOREDIT(userinfo.INSTRUCTOR_EMAILID, userinfo.INSTRUCTOR_NAME);
                    return RedirectToAction("AllInstructors");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewBag.ErrorMsg = "Unable to edit the record. Please retry again.";
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
                    ViewBag.ErrorMsg = "The Record does not exist or has already been deleted.";
                    return View("Error");
                }
               
            }
        }
    }
}