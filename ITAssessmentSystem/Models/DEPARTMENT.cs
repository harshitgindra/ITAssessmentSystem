//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ITAssessmentSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DEPARTMENT
    {
        public DEPARTMENT()
        {
            this.RUBRICS_DATA = new HashSet<RUBRICS_DATA>();
            this.ASSESSMENT_LINK = new HashSet<ASSESSMENT_LINK>();
        }
    
        public string DEPARTMENT_CD { get; set; }
        public string DEPARTMENT_DESC { get; set; }
    
        public virtual ICollection<RUBRICS_DATA> RUBRICS_DATA { get; set; }
        public virtual ICollection<ASSESSMENT_LINK> ASSESSMENT_LINK { get; set; }
    }
}
