using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Models
{
    public class PerformanceIndicator
    {
        //Reference: http://highoncoding.com/Articles/770_Implementing_Dynamic_DropDownList_in_ASP_NET_MVC_3_Framework.aspx
        public RUBRICS_DATA rub { set; get; }
        public List<SelectListItem> selection = new List<SelectListItem>();
        
    }

    

}