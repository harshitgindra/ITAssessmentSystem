using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    [Authorize]
    public class LinkController : Controller
    {
        //
        // GET: /Link/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            using (var context = new AssessmentEntities())
            {
                var linkList = context.spLink_Details().ToList();
                return View(linkList);
            }
        }

        public ActionResult Delete(string id)
        {
            using (var context = new AssessmentEntities())
            {
                try
                {
                    context.spLink_DELETE(id);
                    context.SaveChanges();
                }

                catch (Exception ex)
                {
                    ViewBag.ErrorMsg = "This link cannot be deleted";
                }

                var linkList = context.spLink_Details().ToList();
                return View("List", linkList);

            }
        }
    }
}