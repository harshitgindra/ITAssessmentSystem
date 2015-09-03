using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Controllers
{
    [AuthorizeUser(Users = "hgindra")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        
	}
}