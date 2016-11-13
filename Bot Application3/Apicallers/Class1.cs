using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application3.Apicallers
{

    public class Class1
    {
        public const string apiKey = "07db8c70ffb04568b68a46f511b014c6";
        public static string queryUri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/";

        public static async Task<dynamic> callSentiment(string message)
        {
            queryUri += "sentiment";
            // Http Request 
            var client = new HttpClient
            {
                DefaultRequestHeaders = { {"Ocp-Apim-Subscription-Key", apiKey},
                                                                    {"Accept", "application/json"}
                                                                  }
            };
            //Requst Body
            var sentimentInput = new BatchInput
            {
                documents = new List<DocumentInput> {
                                                        new DocumentInput { langugae="en" ,id = "1",  text = ""+message}}
            };

            var json = JsonConvert.SerializeObject(sentimentInput);
            var sentimentPost = await client.PostAsync(queryUri, new StringContent(json, Encoding.UTF8, "application/json"));
            var sentimentRawResponse = await sentimentPost.Content.ReadAsStringAsync();
            var sentimentJsonResponse = JsonConvert.DeserializeObject<BatchResult>(sentimentRawResponse);
            dynamic sentimentScore = sentimentJsonResponse?.documents?.FirstOrDefault()?.score ?? 0;

            return sentimentScore;
        }
    }
}