namespace Pioneer.GoogleAdwords.Api.EntityDataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EntityDataContext : DbContext
    {
        public EntityDataContext()
            : base("name=EntityDataContext")
        {
        }

        public virtual DbSet<AdwordsPerformance> AdwordsPerformances { get; set; }
        public virtual DbSet<OrganizationMaster> OrganizationMasters { get; set; }
        public virtual DbSet<WebsiteMaster> WebsiteMasters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
