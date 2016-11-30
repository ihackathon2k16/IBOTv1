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
using System.Threading;

namespace Bot_Application3
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        EntityDetector ed = new EntityDetector();
        
        public static string queryUri = "https://api.projectoxford.ai/linguistics/v1.0/analyze/";
        public static string apiKey = "02c5a614172d48a0af2517a3063f0063";
        public static string info = "";
        public static bool skipDirect = false;

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var DbDistribution = "";
            var sysArchitecture = "";
            var count = 0;
            dynamic intent = null;
            dynamic POS = null;
            dynamic keyPhrases = null;
            bool dontcall = false;
            if (activity.Type == ActivityTypes.Message)
            {
                  
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));


                //Greeting part
                

                MessageHandler messageHandler = new MessageHandler();

                dynamic isQuestion = messageHandler.isQuestion(activity.Text);

                if(skipDirect)
                {
                    var replySentiment = await TextAnalyticsAPI.getSentiment(activity.Text);
                    skipDirect = false;
                    if (replySentiment < 0.4)
                    {
                        Activity askFlow = activity.CreateReply($"Okay, Could you please mention the flow name");
                        await connector.Conversations.ReplyToActivityAsync(askFlow);

                    }
                    else
                    {
                        Activity askFlow = activity.CreateReply($"Thank you for confirming the flow. checking for solutions");
                        await connector.Conversations.ReplyToActivityAsync(askFlow);
                    }
                }

                if (activity.Text.ToUpper().Equals("HI") || activity.Text.ToUpper().Contains("HI ") || activity.Text.ToUpper().Contains("HELLO"))
                {
                    Activity reply = activity.CreateReply($"Hi... I am AMAPS...How can I help you???");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                    dontcall = true;
                    var Okresponse = Request.CreateResponse(HttpStatusCode.OK);
                    return Okresponse;
                }

                //Good Bye part
                else if (activity.Text.ToUpper().Contains("BYE") || activity.Text.ToUpper().Contains("THANK"))
                {
                    Activity reply = activity.CreateReply($"Bye... Keep in touch");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                    dontcall = true;
                    var Okresponse = Request.CreateResponse(HttpStatusCode.OK);
                    return Okresponse;
                }

                if (activity.Text.ToUpper().Contains("POR"))
                {
                    Activity reply = activity.CreateReply("Please follow these steps to enable POR logs. Go to the link http://manishde16v/phpbb2/viewtopic.php?t=410&highlight=enable+por");
                    Thread.Sleep(1000);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                    dontcall = true;
                    var Okresponse = Request.CreateResponse(HttpStatusCode.OK);
                    return Okresponse;
                }

           /*     if (activity.Text.ToUpper().Contains("NORMAL")|| activity.Text.ToUpper().Contains("SPLIT"))
                {
                    Activity reply = activity.CreateReply("Are you running change cycle Flow or Rate Change Flow?");
                   
                    await connector.Conversations.ReplyToActivityAsync(reply);
                        dontcall = true;
                }


                if (activity.Text.ToUpper().Contains("CHANGE CYCLE"))
                {
                    Activity reply = activity.CreateReply("Probable related CR:67734 incorrect rate while running change cycle flow");

                    await connector.Conversations.ReplyToActivityAsync(reply);
                    dontcall = true;
                }

                if (activity.Text.ToUpper().Contains("RATECHANGE"))
                {
                    Activity reply = activity.CreateReply("Prabable related CR:65132 incorrect rate while running rate change flow");

                    await connector.Conversations.ReplyToActivityAsync(reply);
                    dontcall = true;
                }

                if (activity.Text.ToUpper().Contains("INCORRECT"))
                {
                    
                   
                    dontcall = false;
                }

                */
                if (isQuestion.Equals("QUESTION"))
                {
                    //Key Phrases
                    keyPhrases = await TextAnalyticsAPI.getKeyPhrases(activity.Text);

                    //Intetn
                    intent = await LUISAPI.getIntent(activity.Text);

                    dontcall = true;
                    

                    Activity reply = activity.CreateReply("DEMO DEBUG: You have asked a question");
                    //Activity reply = activity.CreateReply($"{intent.toString()} {questReply} ");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                }
                //If block for quetion is finished

                else if (isQuestion.Equals("PROBLEM")) //|| count == 2)
                {
                    //problem statement
                    var sentiment = await TextAnalyticsAPI.getSentiment(activity.Text);

                    if (sentiment < 0.4)
                    {
                        keyPhrases = await TextAnalyticsAPI.getKeyPhrases(activity.Text);
                        intent = await LUISAPI.getIntent(activity.Text);

                        //Activity reply = activity.CreateReply($"DEMO DEBUG: You seemed to have stated a problem and I have gathered that your problem is: {keyPhrases} {intent.toString()} ");
                        //await connector.Conversations.ReplyToActivityAsync(reply);

                        //POC Part
                        /*  var posclient = new HttpClient
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
                          //End of POC*/

                        string flowName = "DEFAULT";
                        string answerTag;

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
                                char[] flowId = new char[10];
                                answerTag = result.Substring(result.LastIndexOf("[["), (result.LastIndexOf("]]") - result.LastIndexOf("[[")));
                                //answerTag.CopyTo(answerTag.LastIndexOf(",") + 2, flowId, answerTag.LastIndexOf("/"),
                                //(answerTag.LastIndexOf("/"))-(answerTag.LastIndexOf(",") + 2));

                                int lastComma = answerTag.LastIndexOf(",");
                                int lastSlash = answerTag.LastIndexOf("\"");
                                lastComma += 2;
                                int lengthFlow = lastSlash - lastComma;

                                flowName = answerTag.Substring(lastComma, lengthFlow);
                            }
                            else
                            {
                                string responseContent = await responsem.Content.ReadAsStringAsync();
                            }


                        }
                        //End of ML
                        skipDirect = true;
                        Activity flowSuggestion = activity.CreateReply($"You seem to be facing an issue with {flowName} flow");
                        await connector.Conversations.ReplyToActivityAsync(flowSuggestion);


                        
                    }

                    else
                    {
                        Activity reply = activity.CreateReply($"DEMO DEBUG: You have provided an informative statement");
                        await connector.Conversations.ReplyToActivityAsync(reply);


                    }



                    /*if (count==0 && !dontcall)
                    {
                        Activity reply = activity.CreateReply($"How is your Db Architecture ? Split or normal?");
                        await connector.Conversations.ReplyToActivityAsync(reply);

                        count++;
                    }
                    */



                    //End of Problem Statement
                }
                //Start of Simple statement

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
 