using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Routing;
using System.Web.Security;

namespace ITAssessmentSystem.Models
{
    public class AuthorizeUser : AuthorizeAttribute
    {

        public string Users { get; set; }

    }
}