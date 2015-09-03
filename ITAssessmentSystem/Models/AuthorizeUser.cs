using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Routing;

namespace ITAssessmentSystem.Models
{
    public class AuthorizeUser : AuthorizeAttribute
    {

       
        public string Users { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (Users.Equals(Environment.UserName))
                return true;
            else
                return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Home",
                                action = "Unauthorised"
                            })
                        );
        }


    }
}