using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application3.Apicallers
{
    public class LinguisticAnalytics
    {
        public static string queryUri = "https://api.projectoxford.ai/linguistics/v1.0/analyze/";
        public static string apiKey = "02c5a614172d48a0af2517a3063f0063";
        public static async Task<dynamic> getPOS(string message)
        {

                  var client = new HttpClient
                  {
                      DefaultRequestHeaders = { {"Ocp-Apim-Subscription-Key", apiKey},
                                                                      {"Content-Type", "application/json"}
                                                                    }
                  };

                  var json = getJSON(message);
                  var POSPost = await client.PostAsync(queryUri, new StringContent(json, Encoding.UTF8, "application/json"));
                  var POSRawResponse = await POSPost.Content.ReadAsStringAsync();
                  var POSJsonResponse = JsonConvert.DeserializeObject<LINGResp>(POSRawResponse);
                  dynamic POSResult = POSJsonResponse?.result;
                  return POSResult ;


    
            

        }

        public static dynamic getJSON(string message)
        {
            return JsonConvert.SerializeObject(new LINGINPUT { language = "en", analyzerIds = new List<string> {"4fa79af1-f22c-408d-98bb-b7d7aeef7f04" }, text = "" + message });
            
        }

    }
}