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
        public static string queryUri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/";

        public static  async Task<dynamic> getSentiment(string message)
        {
             queryUri += "sentiment";
             // Http Request 
             //var client = utilities.HttpUtility.getHttpClientForTextAnalytics();
             try
             {
                 var client = new HttpClient
                 {
                     DefaultRequestHeaders = { {"Ocp-Apim-Subscription-Key", "07db8c70ffb04568b68a46f511b014c6"},
                                                                     {"Accept", "application/json"} }
                  };

             //Requst Body
             //var json = getJSON(message);

             var json = JsonConvert.SerializeObject(new BatchInput
            {
                documents = new List<DocumentInput> {
                                                                     new DocumentInput { language="en" ,id = "1",  text = ""+message}}

            });

         //   dynamic sentimentJsonResponse = await getResponse(queryUri, json, client,"sentiment");
             var sentimentPost = await client.PostAsync(queryUri, new StringContent(json, Encoding.UTF8, "application/json"));
             var sentimentRawResponse = await sentimentPost.Content.ReadAsStringAsync();
             var sentimentJsonResponse= JsonConvert.DeserializeObject<BatchResult>(sentimentRawResponse);
             return sentimentJsonResponse?.documents?.FirstOrDefault()?.score ?? 0; 
        }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

        }

        public static async Task<dynamic> getKeyPhrases(string message)
        {
            queryUri += "keyPhrases";
            // Http Request 
            var client = utilities.HttpUtility.getHttpClientForTextAnalytics();

            var json = getJSON(message);
            dynamic data = await getResponse(queryUri, json, client,"key");
            dynamic sentimentScore = data?.documents?.FirstOrDefault()?.keyPhrases?? 0;
            return sentimentScore;
        }

        private static dynamic getJSON(string message)
        {
            return JsonConvert.SerializeObject(new List < DocumentInput > {
                new DocumentInput { language = "en", id = "1", text = "" + message }});
        }

        private async static Task<dynamic> getResponse(string queryUri,dynamic json,dynamic client,string result)
        {
            var sentimentPost = await client.PostAsync(queryUri, new StringContent(json, Encoding.UTF8, "application/json"));
            var sentimentRawResponse = await sentimentPost.Content.ReadAsStringAsync();
            if(result.Equals("sentiment"))
            return JsonConvert.DeserializeObject<BatchResult>(sentimentRawResponse);
            else
                return JsonConvert.DeserializeObject<KeyPhraseResult>(sentimentRawResponse);
        }

    }
}