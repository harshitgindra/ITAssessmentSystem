using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Models
{
    public class ProfessorList
    {
        public List<SelectListItem> getProfessorListList()
        {
            List<SelectListItem> departmentList = new List<SelectListItem>();
            using (var context = new AssessmentEntities())
            {
                //PerformanceIndicator pIndicators = new PerformanceIndicator();
                var result = context.USER_INFO.ToList();
                foreach (var item in result)
                {
                    departmentList.Add(new SelectListItem() { Text = item.INSTRUCTOR_NAME, Value = item.INSTRUCTOR_EMAILID });
                }
                return departmentList;
            }
        }
    }
}