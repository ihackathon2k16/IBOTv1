using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application3.Controllers
{
    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
    public class QuestionRecommender
    {
        public static String quest(dynamic keyPhrases, dynamic intent, dynamic entityType, dynamic Process)
        {
            return null;
        }
         public   static async Task<dynamic> InvokeRequestResponseService(dynamic keyPhrases, dynamic intent, dynamic entityType, dynamic Process)
            {
                using (var client = new HttpClient())
                {
                    var scoreRequest = new
                    {

                        Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"Solution ID", "Problem Entity Type", "Problem Type", "Problem Entity Name", "Process", "Flow", "Architecture", "DB Distribution", "ProblemAttrib", "ProblemAttrib2", "A3"},
                                Values = new string[,] {  { "0", "value", entityType, intent, keyPhrases, "", "", "", "", "", "" },  { "0", "", "", "", "", "", "", "", "", "", "" },  }
                            }
                        },
                    },
                        GlobalParameters = new Dictionary<string, string>()
                        {
                        }
                    };
                    const string apiKey = "eTU0ijE7tlY9abp67EsADsu+Rzc+fL9rAa0ExVQ50aHbPjIowb4lk+CutsPFZawoT21NtcrfBr3XZ5SaSD5bXA=="; // Replace this with the API key for the web service
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/fcea84a4be97428298fa19193d6f700b/services/33f4109919be45d0aba6f8ee48cf51d6/execute?api-version=2.0&details=true");

                    // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                    // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                    // For instance, replace code such as:
                    //      result = await DoSomeTask()
                    // with the following:
                    //      result = await DoSomeTask().ConfigureAwait(false)


                    HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                    //  Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    // Console.WriteLine(response.Headers.ToString());

                    //string responseContent = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(responseContent);
                    return "";
                    }
                }
            }
        }
    }
