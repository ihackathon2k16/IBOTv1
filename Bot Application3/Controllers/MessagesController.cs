using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Bot_Application3.Apicallers;
using Microsoft.Bot.Builder.Dialogs;
using Bot_Application3.Dialogs;
using Bot_Application3.Controllers;
using ConsoleApplication1;

namespace Bot_Application3
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        EntityDetector ed = new EntityDetector();

        public static string queryUri = "https://api.projectoxford.ai/linguistics/v1.0/analyze/";
        public static string apiKey = "02c5a614172d48a0af2517a3063f0063";


        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));


                //Greeting part
                if (activity.Text.ToUpper().Contains("HI") || activity.Text.ToUpper().Contains("HELLO"))
                  {
                      Activity reply = activity.CreateReply($"Hi... How can I help you???");
                      await connector.Conversations.ReplyToActivityAsync(reply);
                      
                  }

                //Good Bye part
                  else if (activity.Text.ToUpper().Contains("BYE") || activity.Text.ToUpper().Contains("THANK YOU"))
                  {
                      Activity reply = activity.CreateReply($"Bye... Keep in touch");
                      await connector.Conversations.ReplyToActivityAsync(reply);
                   
                  }

                  MessageHandler messageHandler = new MessageHandler();

                  dynamic isQuestion = messageHandler.isQuestion(activity.Text);

                if (isQuestion.Equals("QUESTION"))
                  {
                    //Key Phrases
                    var keyPhrases = await TextAnalyticsAPI.getKeyPhrases(activity.Text);
                   
                    //Intetn
                    dynamic intent = await LUISAPI.getIntent(activity.Text);

                    //POS
                    var posclient = new HttpClient
                    {
                        DefaultRequestHeaders = { {"Ocp-Apim-Subscription-Key", apiKey},
                                                                    {"Accept", "application/json"}
                                                                  }
                    };

                    var jsonPOS = getJSON(activity.Text);
                    var POSPost = await posclient.PostAsync(queryUri, new StringContent(jsonPOS, Encoding.UTF8, "application/json"));
                    var POSRawResponse = await POSPost.Content.ReadAsStringAsync();
                    var POSJsonResponse = JsonConvert.DeserializeObject<LINGResp>(POSRawResponse);
                    // dynamic sentimetScore = sentimentJsonResponse?.documents?.FirstOrDefault<DocumentResult>()?.score ?? 0;
                    
                    //End Of POC


                    //ML PART
                    using (var mlclient = new HttpClient())
                    {
                        var scoreRequest = new
                        {
                            Inputs = new Dictionary<string, StringTable>() {
                            {
                                "input1",
                                new StringTable()
                                {
                                ColumnNames = new string[] {"Problem Entity Type", "Problem Type", "Problem Entity Name", "Process", "Flow", "Architecture", "DB Distribution", "ProblemAttrib"},
                                Values = new string[,] {  { "", intent.toString(), keyPhrases, "", "", "", "", "" },  }
                                }
                            },
                        },
                        GlobalParameters = new Dictionary<string, string>()
                        {
                        }
                    };
                    const string apiKey = "eTU0ijE7tlY9abp67EsADsu+Rzc+fL9rAa0ExVQ50aHbPjIowb4lk+CutsPFZawoT21NtcrfBr3XZ5SaSD5bXA=="; // Replace this with the API key for the web service
                    mlclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    mlclient.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/fcea84a4be97428298fa19193d6f700b/services/a1ff2b527eaa4bb2a9dc66d4e640ac52/execute?api-version=2.0&details=true");

                    HttpResponseMessage responsem = await mlclient.PostAsJsonAsync("", scoreRequest);

                    if (responsem.IsSuccessStatusCode)
                    {
                        string result = await responsem.Content.ReadAsStringAsync();
                  
                    }
                    else
                    {
                        Console.WriteLine(string.Format("The request failed with status code: {0}", responsem.StatusCode));
                        string responseContent = await responsem.Content.ReadAsStringAsync();
                    }
                }
                    Activity reply = activity.CreateReply(keyPhrases);
                      //Activity reply = activity.CreateReply($"{intent.toString()} {questReply} ");
                      await connector.Conversations.ReplyToActivityAsync(reply);

                }
            //If block for quetion is finished

                  else if (isQuestion.Equals("PROBLEM"))
                  {
                    //problem statement
                    
                    var keyPhrases = await TextAnalyticsAPI.getKeyPhrases(activity.Text);
                    dynamic intent = await LUISAPI.getIntent(activity.Text);

                    //POC Part
                    var posclient = new HttpClient
                    {
                        DefaultRequestHeaders = { {"Ocp-Apim-Subscription-Key", apiKey},
                                                                    {"Accept", "application/json"}
                                                                  }
                    };

                    var jsonPOS = getJSON(activity.Text);
                    var POSPost = await posclient.PostAsync(queryUri, new StringContent(jsonPOS, Encoding.UTF8, "application/json"));
                    var POSRawResponse = await POSPost.Content.ReadAsStringAsync();
                    var POSJsonResponse = JsonConvert.DeserializeObject<LINGResp>(POSRawResponse);
                    // dynamic sentimetScore = sentimentJsonResponse?.documents?.FirstOrDefault<DocumentResult>()?.score ?? 0;
                    //End of POC

                    //ML PART
                    using (var client = new HttpClient())
                    {
                        var scoreRequest = new
                        {

                            Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"Problem Entity Type", "Problem Type", "Problem Entity Name", "Process", "Flow", "Architecture", "DB Distribution", "ProblemAttrib"},
                                Values = new string[,] {  { "", intent.toString(), keyPhrases, "", "", "", "", "" },  }
                            }
                        },
                    },
                            GlobalParameters = new Dictionary<string, string>()
                            {
                            }
                        };
                        const string apiKey = "eTU0ijE7tlY9abp67EsADsu+Rzc+fL9rAa0ExVQ50aHbPjIowb4lk+CutsPFZawoT21NtcrfBr3XZ5SaSD5bXA=="; // Replace this with the API key for the web service
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                        client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/fcea84a4be97428298fa19193d6f700b/services/a1ff2b527eaa4bb2a9dc66d4e640ac52/execute?api-version=2.0&details=true");
                        
                        HttpResponseMessage responsem = await client.PostAsJsonAsync("", scoreRequest);

                        if (responsem.IsSuccessStatusCode)
                        {
                            string result = await responsem.Content.ReadAsStringAsync();
                            
                        }
                        else
                        {
                            Console.WriteLine(string.Format("The request failed with status code: {0}", responsem.StatusCode));
                            string responseContent = await responsem.Content.ReadAsStringAsync();
                        }
                        //End of ML

                    }

                    //End of Problem Statement
                 }
                else
                {
                      Activity reply = activity.CreateReply($"OK..");
                      await connector.Conversations.ReplyToActivityAsync(reply);
                }

            }
            else
            {
                HandleSystemMessage(activity);
            }
            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }


        private static dynamic getJSON(string message)
        {
            return JsonConvert.SerializeObject(new LINGINPUT { language = "en", analyzerIds = new List<string> { "22a6b758-420f-4745-8a3c-46835a67c0d2", "4fa79af1-f22c-408d-98bb-b7d7aeef7f04" }, text = "" + message });

        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
 