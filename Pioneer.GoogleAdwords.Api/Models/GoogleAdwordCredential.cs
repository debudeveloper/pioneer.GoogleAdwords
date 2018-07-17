using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer.GoogleAdwords.Api.Models
{
   public class GoogleAdwordCredential
    {
        public int Id { get; set; }
        public int OrganizationMasterId { get; set; }
        public int ProductMasterId { get; set; }
        public int WebSiteMasterId { get; set; }
        public string ProductName { get; set; } 
        public string ClientCustomerId { get; set; }
        public string DeveloperToken { get; set; }
        public string OAuth2ClientId { get; set; }
        public string OAuth2ClientSecret { get; set; }
        public string OAuth2RefreshToken { get; set; }
    }
}
