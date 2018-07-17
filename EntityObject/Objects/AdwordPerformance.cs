using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityObject.Objects
{
    public class AdwordPerformance
    {
        public int Id { get; set; }
        public DateTime SearchDataFor { get; set; }
        public DateTime GenerateDateTime { get; set; }
        public int ProductMastersId { get; set; }

        public long AccountId { get; set; }
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

    }
}
