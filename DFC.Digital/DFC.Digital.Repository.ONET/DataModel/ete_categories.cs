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
    
    public partial class ete_categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ete_categories()
        {
            this.education_training_experience = new HashSet<education_training_experience>();
        }
    
        public string element_id { get; set; }
        public string scale_id { get; set; }
        public decimal category { get; set; }
        public string category_description { get; set; }
    
        public virtual content_model_reference content_model_reference { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<education_training_experience> education_training_experience { get; set; }
        public virtual scales_reference scales_reference { get; set; }
    }
}
