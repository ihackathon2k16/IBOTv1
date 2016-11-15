using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application3.Apicallers
{
    public class LUISAPI
    {
        public static string queryUri = "https://api.projectoxford.ai/luis/v2.0/apps/b5d15805-8d03-4411-846f-034b5852ad9f?subscription-key=78cfd1713db84941a9c0aba03fac04cf&q=";
        public static async Task<dynamic> getIntent(string message)
        {
            message = Uri.EscapeDataString(message);
            try
            {
                LUISResult intent = new LUISResult();
                using (HttpClient client = new HttpClient())
                {
                    string RequestURI = "https://api.projectoxford.ai/luis/v2.0/apps/b5d15805-8d03-4411-846f-034b5852ad9f?subscription-key=78cfd1713db84941a9c0aba03fac04cf&q=" + message;
                    HttpResponseMessage msg = await client.GetAsync(RequestURI);

                    if (msg.IsSuccessStatusCode)
                    {
                        var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                        intent = JsonConvert.DeserializeObject<LUISResult>(JsonDataResponse);
                    }
                }

                return intent;


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

        }
    }
}