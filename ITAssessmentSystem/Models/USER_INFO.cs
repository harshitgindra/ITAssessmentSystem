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
    
    public partial class USER_INFO
    {
        public USER_INFO()
        {
            this.ASSESSMENT_DATA = new HashSet<ASSESSMENT_DATA>();
        }
    
        public string PROF_NAME { get; set; }
        public string PROF_EMAILID { get; set; }
    
        public virtual ICollection<ASSESSMENT_DATA> ASSESSMENT_DATA { get; set; }
    }
}
