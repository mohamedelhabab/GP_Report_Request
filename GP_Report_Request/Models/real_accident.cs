//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GP_Report_Request.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class real_accident
    {
        public int accident_ID { get; set; }
        public string location { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public System.DateTime happened_time { get; set; }
        public bool solved { get; set; }
        public int area_id { get; set; }
    
        public virtual area_range area_range { get; set; }
    }
}
