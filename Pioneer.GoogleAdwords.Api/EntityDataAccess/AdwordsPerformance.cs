namespace Pioneer.GoogleAdwords.Api.EntityDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdwordsPerformance
    {
        public int ID { get; set; }

        public DateTime GenerateDateTime { get; set; }

        public int WebsiteMasterId { get; set; }

        public int OrganizationMasterId { get; set; }

        public string ProductName { get; set; }

        public int AdwordType { get; set; }

        public string AccountId { get; set; }

        public long CampaignId { get; set; }

        public string CampaignName { get; set; }

        public long AdGroupId { get; set; }

        public string AdGroupName { get; set; }

        public long KeywordId { get; set; }

        public string KeywordName { get; set; }

        public int Impression { get; set; }

        public int Click { get; set; }

        public double Ctr { get; set; }

        public double Cost { get; set; }

        public double AvgCpc { get; set; }

        public double Convertion { get; set; }

        public double Cpa { get; set; }

        public double ConvertionRate { get; set; }

        public double AvgPosition { get; set; }

        public virtual OrganizationMaster OrganizationMaster { get; set; }

        public virtual WebsiteMaster WebsiteMaster { get; set; }
    }
}
