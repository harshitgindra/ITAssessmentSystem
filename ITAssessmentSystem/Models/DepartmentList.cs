﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Models
{
    public class DepartmentList
    {

       
            //Reference: http://highoncoding.com/Articles/770_Implementing_Dynamic_DropDownList_in_ASP_NET_MVC_3_Framework.aspx

        public List<SelectListItem> getDepartmentList()
        {
            List<SelectListItem> departmentList = new List<SelectListItem>();
            using (var context = new assessmentEntities())
            {
                //PerformanceIndicator pIndicators = new PerformanceIndicator();
                departmentList.Add(new SelectListItem() { Text = "Please select Department", Value = "default" });
                var result = context.DEPARTMENTS.ToList();
                foreach (var item in result)
                {
                    departmentList.Add(new SelectListItem() { Text = item.department_desc, Value = item.DEPARTMENT_CD });
                }
                return departmentList;
            }
        }

        
    }
}