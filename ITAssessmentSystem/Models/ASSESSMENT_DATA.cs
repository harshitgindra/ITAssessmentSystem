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
    
    public partial class ASSESSMENT_DATA
    {
        public int DATA_ROWID { get; set; }
        public Nullable<int> RUBRIC_ROWID { get; set; }
        public string OUTCOMES { get; set; }
        public string DEPARTMENT { get; set; }
        public string PROF_EMAILID { get; set; }
        public string SEMESTER { get; set; }
        public string COURSE { get; set; }
        public string PERFORMANCE_INDICATOR { get; set; }
        public string TOPIC { get; set; }
        public Nullable<int> POOR { get; set; }
        public Nullable<int> DEVELOPING { get; set; }
        public Nullable<int> DEVELOPED { get; set; }
        public Nullable<int> EXEMPLARY { get; set; }
    
        public virtual USER_INFO USER_INFO { get; set; }
        public virtual RUBRICS_DATA RUBRICS_DATA { get; set; }
    }
}
