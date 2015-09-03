using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizeUser(Users = "hgindra")]
        public ActionResult Index()
        {
            return View();
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