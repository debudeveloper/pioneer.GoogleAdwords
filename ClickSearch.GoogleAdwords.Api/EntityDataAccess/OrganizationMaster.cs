namespace ClickSearch.GoogleAdwords.Api.EntityDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrganizationMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationMaster()
        {
            AdwordsPerformances = new HashSet<AdwordsPerformance>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(40)]
        public string OrganizationName { get; set; }

        public int State { get; set; }

        [Required]
        public string OrganizationHashCode { get; set; }

        public string OrganizationBrandName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdwordsPerformance> AdwordsPerformances { get; set; }
    }
}
