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


        public List<SelectListItem> getPerformanceIndicatorList(string dept)
        {
            List<SelectListItem> performanceIndicators = new List<SelectListItem>();
            using (var context = new AssessmentEntities())
            {
                //var result = context.RUBRICS_DATA.Where(x => x.DEPARTMENT_CD.Equals(id)).Select(x => x.PERFORMANCE_INDICATOR).Distinct().ToList();
                var result = context.spRUBRICGETPERFORMANCEINDICATORS(dept).OrderBy(order=>order.outcomes);
                foreach (var item in result)
                {
                    performanceIndicators.Add(new SelectListItem() { Text = item.outcomes +"-" +item.performance_indicator, Value = item.outcomes });
                }
                return performanceIndicators;
            }
        }
        
    }

    

}