using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITAssessmentSystem.Models
{
    public class ProfessorList
    {
        public USER_INFO users { set; get; }
        public List<SelectListItem> selection = new List<SelectListItem>();

        public List<USER_INFO> getProfList()
        {
            ProfessorList o = new ProfessorList();
            assessmentEntities assessment = new assessmentEntities();
            List<USER_INFO> res = assessment.USER_INFO                
                .ToList();
            return res;
        }
    }
}