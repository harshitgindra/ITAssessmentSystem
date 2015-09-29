using ITAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ITAssessmentSystem.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                using (var context = new assessmentEntities())
                {
                    var AdminRecord = context.spLOGIN(changePassword.UserName, changePassword.ExistingPassword).SingleOrDefault();
                    if (AdminRecord != null)
                    {
                        AdminRecord.ADMIN_PASSWORD = changePassword.NewPassword;
                        context.SaveChanges();
                        ModelState.AddModelError("", "Password Changed Successfully");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please enter correct password");
                    }
                }
            }
            return View(changePassword);
        }
    }
}