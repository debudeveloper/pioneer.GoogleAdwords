using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Pioneer.Sherlock.Service.Model
{
    public class Repository : IRepository
    {
        public int SubmitAdwordsPerformance(object lead, string url)
        {
            try
            {
                int returnId = 0;
                var client = new HttpClient();
                var task = client.PostAsJsonAsync(url, lead)
                  .ContinueWith((taskwithresponse) =>
                  {
                      var response = taskwithresponse.Result;
                      var jsonString = response.Content.ReadAsStringAsync();
                      jsonString.Wait();
                      var jsonResult = JsonConvert.DeserializeObject<dynamic>(jsonString.Result);
                      string status = jsonResult?.result;
                      if (status == "success")
                      {
                          returnId = Convert.ToInt32(jsonResult.ref_id);
                      }
                  });
                task.Wait();

                return returnId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}