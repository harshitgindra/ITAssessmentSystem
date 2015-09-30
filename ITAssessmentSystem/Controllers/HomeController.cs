using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;

namespace ITAssessmentSystem.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new AssessmentEntities())
                {
                    var userExist = context.spLOGIN(model.UserName, model.Password).ToList();
                    if (userExist.Count == 1)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        return RedirectToAction("Index", "admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Log in credentials invalid");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Unauthorised()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View("Index");
        }

    }
}