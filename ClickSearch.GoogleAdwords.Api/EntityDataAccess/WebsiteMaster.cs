namespace ClickSearch.GoogleAdwords.Api.EntityDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WebsiteMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WebsiteMaster()
        {
            AdwordsPerformances = new HashSet<AdwordsPerformance>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        public string WebsiteAddress { get; set; }

        [Required]
        public string WebsiteStatus { get; set; }

        [Required]
        public string WebsiteName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public int DeleteStatus { get; set; }

        public string WebsiteFromEmail { get; set; }

        public string WebsiteFromText { get; set; }

        public string WebsiteEmailSubject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdwordsPerformance> AdwordsPerformances { get; set; }
    }
}
