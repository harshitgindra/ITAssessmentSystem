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
    
    public partial class RUBRICS_DATA
    {
        public RUBRICS_DATA()
        {
            this.ASSESSMENT_DATA = new HashSet<ASSESSMENT_DATA>();
        }
    
        public int RUBRIC_ROWID { get; set; }
        public string OUTCOMES { get; set; }
        public string DEPARTMENT { get; set; }
        public string PERFORMANCE_INDICATOR { get; set; }
        public string TOPIC { get; set; }
        public string POOR { get; set; }
        public string DEVELOPING { get; set; }
        public string DEVELOPED { get; set; }
        public string EXEMPLARY { get; set; }
    
        public virtual ICollection<ASSESSMENT_DATA> ASSESSMENT_DATA { get; set; }
        public virtual departments departments { get; set; }
    }
}
