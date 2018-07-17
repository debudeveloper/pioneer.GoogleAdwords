using ClickSearch.GoogleAdwords.Api.EntityDataAccess;
using ClickSearch.GoogleAdwords.Api.Models;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util.Reports;
using Google.Api.Ads.AdWords.v201802;
using Google.Api.Ads.Common.Lib;
using Google.Api.Ads.Common.Util.Reports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;

namespace ClickSearch.GoogleAdwords.Api.Controllers
{
    [RoutePrefix("adwords/clicksearch")]
    [EnableCors(headers:"*",origins: "*",methods: "*")]
    public class GoogleAdwordController : ApiController
    {
        EntityDataContext _dbContext = new EntityDataContext();
        [HttpGet]
        [Route("Google", Name = "GetGoogleCampignPerformance")]
        // public IHttpActionResult GetGoogleCampignPerformance(int ProductMasterId,GoogleAdwordCredential credential)
        public IHttpActionResult GetGoogleCampignPerformance(int productMasterId)
        {
            List<AdwordsPerformance> _adwordsPerformance = new List<AdwordsPerformance>();
            if (productMasterId == 0) return Content(HttpStatusCode.BadRequest, "Bad Request");

            GoogleAdwordCredential credential = getGoogleAdwordsCredentialByProductId(productMasterId);

            if (credential == null) return Content(HttpStatusCode.NotFound, "Invalid");


            AdWordsUser user = new AdWordsUser();
            (user.Config as AdWordsAppConfig).ClientCustomerId = credential.ClientCustomerId;// credential.ClientCustomerId;// "740-435-6551";// "278-414-1536";
            (user.Config as AdWordsAppConfig).DeveloperToken = credential.DeveloperToken;//"0Wf3sFhDmmahNnTxbynJfg"; //credential.DeveloperToken;
            (user.Config as AdWordsAppConfig).OAuth2ClientId = credential.OAuth2ClientId;//"215226543458-nd2eg32u4udlskt3ab8r1437mpim21is.apps.googleusercontent.com"; //credential.OAuth2ClientId;
            (user.Config as AdWordsAppConfig).OAuth2ClientSecret = credential.OAuth2ClientSecret;// "LFzx_vtGuRP-Fn_Mk9b6S1DU"; // credential.OAuth2ClientSecret;
            (user.Config as AdWordsAppConfig).OAuth2Mode = OAuth2Flow.APPLICATION;
            (user.Config as AdWordsAppConfig).OAuth2RefreshToken = credential.OAuth2RefreshToken;//"1/hmq7O1c8pObwH376sev5F0PZyu_X5pvLbLbcpVF7xbA";  //credential.OAuth2RefreshToken;

            //TODO (Add more configuration settings here.

            //   CampaignService campaignService = (CampaignService)user.GetService(AdWordsService.v201802.CampaignService);
            //TODO (Add your code here to use the service.)
            //string startDate = DateTime.Now.Date.ToString("yyyyMMdd");
            //string endDate = DateTime.Now.Date.Subtract(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 5)).ToString("yyyyMMdd");

            // ReportDefinitionDateRangeType dateRangeType = GetReportDateTimeRange(startDate, endDate);


            ReportDefinition definition = new ReportDefinition()
            {
                reportName = DateTime.Now.ToString("dd-mm-yyyy"),
                reportType = ReportDefinitionReportType.CRITERIA_PERFORMANCE_REPORT,
                downloadFormat = DownloadFormat.XML,
                dateRangeType = ReportDefinitionDateRangeType.YESTERDAY,
                dateRangeTypeSpecified = true,

                selector = new Selector()
                {
                    fields = new string[] {

                        "CampaignId",    // get campaign id
                        "CampaignName",  // get campaign Name
                        "AdGroupId",// adwords id
                        "AdGroupName",
                        "Id",           // kewords id
                        "CriteriaType", // type of search 
                        "Criteria",      // get keywords name
                        "CriteriaDestinationUrl",  // ads url
                        "FinalUrls",    // 
                        "CpcBid",          // max cpc
                        "Clicks",         // total click per ad
                        "Impressions",    // no of ads shows on google netwoks
                        "Cost",           // cost per ads
                        "Ctr",            // total click per total impression
                        "AverageCpc",
                        "CampaignStatus",  // campaign status 
                        "Conversions",     // no of leads convert
                        "CostPerConversion", // cpa
                        "ValuePerConversion",
                        "AveragePosition",
                        "ConversionRate" // (convertion/click) *100,                     
                    },
                    //dateRange = new DateRange()
                    //{
                    //    min = "20150201",
                    //    max = "20150201"
                    //},
                    predicates = new Predicate[] {
                    Predicate.In("Status", new string[] { "PAUSED","ENABLED" })
                    }
                }
            };

            // Optional: Include zero impression rows.
            AdWordsAppConfig config = (AdWordsAppConfig)user.Config;
            config.IncludeZeroImpressions = false;

            string filePath = Path.GetTempPath();


            try
            {

                ReportUtilities utilities = new ReportUtilities(user, "v201802", definition);
                using (ReportResponse response = utilities.GetResponse())
                {
                    //  response.Save(filePath + "respp.csv");
                    var downloadedStream = response.Stream;
                    XmlDocument doc = new XmlDocument();
                    doc.Load(downloadedStream);
                    string json = JsonConvert.SerializeObject(doc);
                    var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);
                    var rows = jsonObject.report.table.row;
                    string dateRange = jsonObject.report["date-range"]["@date"];
                    DateTime parsedDateRange = DateTime.Parse(dateRange);
                    if (rows == null) return Ok();
                    foreach (var row in rows)
                    {
                        string url = row["@finalURL"];
                        Tuple<int, string> prodVal = getFilterProdValue(url, credential);
                        int websitemasterId = prodVal.Item1;
                        string productName = prodVal.Item2;

                        removeDuplicateData(DateTime.Today.AddDays(-1), websitemasterId, productName);


                        string _avgCpc = row["@avgCPC"];
                        double avgCpc = FilterRateTypeValue(_avgCpc);

                        string _conversionRate = row["@convRate"];
                        double conversionRate = FilterRateTypeValue(_conversionRate);

                        string _ctr = row["@ctr"];
                        double ctr = FilterRateTypeValue(_ctr);

                        AdwordsPerformance adwordsPerformance = new AdwordsPerformance()
                        {
                            AccountId = credential.ClientCustomerId,// credential.ClientCustomerId, // will chnage, get from user param

                            AdGroupId = row["@adGroupID"],
                            AdGroupName = row["@adGroup"],
                            AvgCpc = avgCpc,
                            AvgPosition = row["@avgPosition"],
                            CampaignId = row["@campaignID"],
                            CampaignName = row["@campaign"],
                            Impression = row["@impressions"],
                            Cost = row["@cost"],
                            Click = row["@clicks"],
                            Convertion = row["@conversions"],
                            ConvertionRate = conversionRate,
                            Cpa = row["@costConv"],
                            Ctr = ctr,
                            GenerateDateTime = DateTime.Now.AddDays(-1),

                            KeywordId = row["@keywordID"],
                            KeywordName = row["@keywordPlacement"],
                            AdwordType = 1,// google
                            OrganizationMasterId = 1,
                            ProductName = productName,
                            WebsiteMasterId = websitemasterId
                        };

                        _adwordsPerformance.Add(adwordsPerformance);
                    }
                    response.Dispose();

                    saveToDataBase(_adwordsPerformance);

                }
            }
            catch (Exception e)
            {
                throw new System.ApplicationException("Failed to download report.", e);
            }


            return Ok();
        }
        private void removeDuplicateData(DateTime dateTime, int websitemasterId, string productName)
        {
            var existData = _dbContext.AdwordsPerformances.Where(m => m.WebsiteMasterId == websitemasterId & (DbFunctions.TruncateTime(m.GenerateDateTime) <= dateTime && dateTime <= DbFunctions.TruncateTime(m.GenerateDateTime)));
            foreach (var item in existData)
            {
                _dbContext.AdwordsPerformances.Remove(item);
            }
            _dbContext.SaveChanges();
        }
        private void saveToDataBase(List<AdwordsPerformance> _adwordsPerformance)
        {
            foreach (var item in _adwordsPerformance)
            {
                _dbContext.AdwordsPerformances.Add(item);
            }
            _dbContext.SaveChanges();
        }

        private GoogleAdwordCredential getGoogleAdwordsCredentialByProductId(int productMasterId)
        {
            GoogleAdwordCredential credential = new GoogleAdwordCredential();
            switch (productMasterId)
            {
                case 60: // life quote
                    return credential = new GoogleAdwordCredential
                    {
                        WebSiteMasterId = 2,
                        ProductName = "Life",
                        ClientCustomerId = "452-504-3771",
                        DeveloperToken = "0Wf3sFhDmmahNnTxbynJfg",
                        OAuth2ClientId = "942848704706-0bcf6mtvt92fc2o12aq8f9f3noksvcfj.apps.googleusercontent.com",
                        OAuth2ClientSecret = "AWOt5cVFsrQ3uZccUOf_NvJN",
                        OAuth2RefreshToken = "1/eBcJvSQZnllTkN8IHQP4i-XAGI5WosBkLvvWQfcb2GZRdzzP3e9dEJz5QpsBIhkk"
                    };
                case 55: // true mortgage
                    return credential = new GoogleAdwordCredential
                    {
                        WebSiteMasterId = 19,
                        ProductName = "Mortgage",
                        ClientCustomerId = "890-651-2724",
                        DeveloperToken = "0Wf3sFhDmmahNnTxbynJfg",
                        OAuth2ClientId = "942848704706-0bcf6mtvt92fc2o12aq8f9f3noksvcfj.apps.googleusercontent.com",
                        OAuth2ClientSecret = "AWOt5cVFsrQ3uZccUOf_NvJN",
                        OAuth2RefreshToken = "1/eBcJvSQZnllTkN8IHQP4i-XAGI5WosBkLvvWQfcb2GZRdzzP3e9dEJz5QpsBIhkk"
                    };
                case 64: // true equity release
                    return credential = new GoogleAdwordCredential
                    {
                        WebSiteMasterId = 20,
                        ProductName = "Equity Release",
                        ClientCustomerId = "596-807-1840",
                        DeveloperToken = "0Wf3sFhDmmahNnTxbynJfg",
                        OAuth2ClientId = "942848704706-0bcf6mtvt92fc2o12aq8f9f3noksvcfj.apps.googleusercontent.com",
                        OAuth2ClientSecret = "AWOt5cVFsrQ3uZccUOf_NvJN",
                        OAuth2RefreshToken = "1/eBcJvSQZnllTkN8IHQP4i-XAGI5WosBkLvvWQfcb2GZRdzzP3e9dEJz5QpsBIhkk"
                    };
                case 65: // health quote
                    return credential = new GoogleAdwordCredential
                    {
                        WebSiteMasterId = 4,
                        ProductName = "Health",
                        ClientCustomerId = "347-467-2118",
                        DeveloperToken = "0Wf3sFhDmmahNnTxbynJfg",
                        OAuth2ClientId = "942848704706-0bcf6mtvt92fc2o12aq8f9f3noksvcfj.apps.googleusercontent.com",
                        OAuth2ClientSecret = "AWOt5cVFsrQ3uZccUOf_NvJN",
                        OAuth2RefreshToken = "1/eBcJvSQZnllTkN8IHQP4i-XAGI5WosBkLvvWQfcb2GZRdzzP3e9dEJz5QpsBIhkk"
                    };
                case 102: // true bridging loan
                    return credential = new GoogleAdwordCredential
                    {
                        WebSiteMasterId = 51,
                        ProductName = "Bridging Loan",
                        ClientCustomerId = "804-874-7894",
                        DeveloperToken = "0Wf3sFhDmmahNnTxbynJfg",
                        OAuth2ClientId = "942848704706-0bcf6mtvt92fc2o12aq8f9f3noksvcfj.apps.googleusercontent.com",
                        OAuth2ClientSecret = "AWOt5cVFsrQ3uZccUOf_NvJN",
                        OAuth2RefreshToken = "1/eBcJvSQZnllTkN8IHQP4i-XAGI5WosBkLvvWQfcb2GZRdzzP3e9dEJz5QpsBIhkk"
                    };
            }

            return null;
        }

        public double FilterRateTypeValue(string val)
        {
            if (val.Contains("%"))
            {
                val = val.Remove(val.Length - 1);
            }
            return Convert.ToDouble(val);
        }
        public Tuple<int, string> getFilterProdValue(string url,GoogleAdwordCredential credential)
        {
            
            if (url.Contains("www.bestlifequote.uk.com/asu-insurance"))
            {
                return new Tuple<int, string>(9, "ASU");
            }
            if (url.Contains("www.bestlifequote.uk.com/prepaid-funeral-plan"))
            {
                return new Tuple<int, string>(34, "Prepaid Funeral");
            }
                    
            return new Tuple<int, string>(credential.WebSiteMasterId, credential.ProductName);
        }
    }
}

