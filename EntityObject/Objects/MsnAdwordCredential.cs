using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityObject.Objects
{
    public class MsnAdwordCredential
    {
        public int Id { get; set; }
        public int AdwordId { get; set; }
        public int ProductMasterId { get; set; }

        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public string Developer { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }

    }
}
