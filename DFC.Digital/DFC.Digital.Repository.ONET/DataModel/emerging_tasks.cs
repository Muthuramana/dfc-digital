//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DFC.Digital.Repository.ONET.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class emerging_tasks
    {
        public string onetsoc_code { get; set; }
        public string task { get; set; }
        public string category { get; set; }
        public Nullable<decimal> original_task_id { get; set; }
        public decimal writein_total { get; set; }
        public System.DateTime date_updated { get; set; }
        public string domain_source { get; set; }
    
        public virtual occupation_data occupation_data { get; set; }
        public virtual task_statements task_statements { get; set; }
    }
}
