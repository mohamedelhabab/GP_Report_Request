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
    
    public partial class country
    {
        public country()
        {
            this.area_range = new HashSet<area_range>();
            this.cities = new HashSet<city>();
            this.users = new HashSet<user>();
        }
    
        public int country_id { get; set; }
        public string country_name { get; set; }
    
        public virtual ICollection<area_range> area_range { get; set; }
        public virtual ICollection<city> cities { get; set; }
        public virtual ICollection<user> users { get; set; }
    }
}
