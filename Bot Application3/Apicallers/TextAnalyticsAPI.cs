using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bot_Application3.utilities;

namespace Bot_Application3.Apicallers
{

    public class TextAnalyticsAPI
    {
       // public static string queryUri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/";
        public static string apiKey = "07db8c70ffb04568b68a46f511b014c6";

        public static  async Task<dynamic> getSentiment(string message)
        {
            string queryUri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
            
             try
             {
                
                var client = new HttpClient
                {
                    DefaultRequestHeaders = { {"Ocp-Apim-Subscription-Key", apiKey},
                                                                    {"Accept", "application/json"}
                                                                  }
                };

                var json = getJSON(message);
                 var sentimentPost = await client.PostAsync(queryUri, new StringContent(json, Encoding.UTF8, "application/json"));
                  var sentimentRawResponse = await sentimentPost.Content.ReadAsStringAsync();
                  var sentimentJsonResponse= JsonConvert.DeserializeObject<BatchResult>(sentimentRawResponse);
                  dynamic sentimetScore = sentimentJsonResponse?.documents?.FirstOrDefault<DocumentResult>()?.score ?? 0;
                return sentimetScore;
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

         }

        public static async Task<dynamic> getKeyPhrases(string message)
        {
            string queryUri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";

            var client = new HttpClient
            {
                DefaultRequestHeaders = { {"Ocp-Apim-Subscription-Key", apiKey},
                                                                    {"Accept", "application/json"}
                                                                  }
            };

            var json = getJSON(message);
            var sentimentPost = await client.PostAsync(queryUri, new StringContent(json, Encoding.UTF8, "application/json"));
            var sentimentRawResponse = await sentimentPost.Content.ReadAsStringAsync();
            var sentimentJsonResponse = JsonConvert.DeserializeObject<KeyPhraseResult>(sentimentRawResponse);
            var keyPhrases = sentimentJsonResponse?.documents?.FirstOrDefault<KeyDocument>()?.keyPhrases; //.FirstOrDefault<String>() ?? "null";
            string combindedString = string.Join(",", keyPhrases);
            return combindedString;

            // string combindedString = string.Join(",", colors);
           // var json = getJSON(message);

        }

        private static dynamic getJSON(string message)
        {
            return JsonConvert.SerializeObject(new BatchInput
            {
                documents = new List<DocumentInput> { new DocumentInput { language = "en", id = "1", text = "" + message } }
            });
        }


    }
}