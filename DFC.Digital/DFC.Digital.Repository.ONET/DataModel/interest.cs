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
    
    public partial class interest
    {
        public string onetsoc_code { get; set; }
        public string element_id { get; set; }
        public string scale_id { get; set; }
        public decimal data_value { get; set; }
        public System.DateTime date_updated { get; set; }
        public string domain_source { get; set; }
    
        public virtual content_model_reference content_model_reference { get; set; }
        public virtual occupation_data occupation_data { get; set; }
        public virtual scales_reference scales_reference { get; set; }
    }
}