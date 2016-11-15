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

       // <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                if (activity.Text.ToUpper().Contains("HI") || activity.Text.ToUpper().Contains("HELLO"))
                  {
                      Activity reply = activity.CreateReply($"Hi... How can I help you???");
                      await connector.Conversations.ReplyToActivityAsync(reply);
                      goto end;
                  }
                  else if (activity.Text.ToUpper().Contains("BYE") || activity.Text.ToUpper().Contains("THANK YOU"))
                  {
                      Activity reply = activity.CreateReply($"Bye... Keep in touch");
                      await connector.Conversations.ReplyToActivityAsync(reply);
                      goto end;
                  }

                  MessageHandler messageHandler = new MessageHandler();
                  dynamic response1 = messageHandler.isQuestion(activity.Text);
                  if (response1.Equals("QUESTION"))
                  {
                      //question

                     // await connector.Conversations.ReplyToActivityAsync(reply);
                      var keyPhrases = await TextAnalyticsAPI.getKeyPhrases(activity.Text);
                      dynamic intent = await LUISAPI.getIntent(activity.Text);
                    
                    //  dynamic entityType =ed.identifyType(keyPhrases);

                    // var questReply = InvokeRequestResponseService(keyPhrases, intent.toString(), entityType ,null);

  
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

                    // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                    // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                    // For instance, replace code such as:
                    //      result = await DoSomeTask()
                    // with the following:
                    //      result = await DoSomeTask().ConfigureAwait(false)


                    HttpResponseMessage responsem = await client.PostAsJsonAsync("", scoreRequest);

                    if (responsem.IsSuccessStatusCode)
                    {
                        string result = await responsem.Content.ReadAsStringAsync();
                        //Console.WriteLine("Result: {0}", result);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("The request failed with status code: {0}", responsem.StatusCode));

                        // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                       // Console.WriteLine(responsem.Headers.ToString());

                        string responseContent = await responsem.Content.ReadAsStringAsync();
                        //Console.WriteLine(responseContent);
                    }
                }
            




    /* if (questReply == null)
         {
             questReply = "Will revert back to you soon ! ";
         }
         questReply += keyPhrases;*/
    Activity reply = activity.CreateReply(keyPhrases);
                      //Activity reply = activity.CreateReply($"{intent.toString()} {questReply} ");
                      await connector.Conversations.ReplyToActivityAsync(reply);
                  }



                  else if (response1.Equals("PROBLEM"))
                  {
                    //problem statement

                    // await connector.Conversations.ReplyToActivityAsync(reply);
                    var keyPhrases = await TextAnalyticsAPI.getKeyPhrases(activity.Text);
                    dynamic intent = await LUISAPI.getIntent(activity.Text);

                    //  dynamic entityType =ed.identifyType(keyPhrases);

                    // var questReply = InvokeRequestResponseService(keyPhrases, intent.toString(), entityType ,null);


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

                        // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                        // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                        // For instance, replace code such as:
                        //      result = await DoSomeTask()
                        // with the following:
                        //      result = await DoSomeTask().ConfigureAwait(false)


                        HttpResponseMessage responsem = await client.PostAsJsonAsync("", scoreRequest);

                        if (responsem.IsSuccessStatusCode)
                        {
                            string result = await responsem.Content.ReadAsStringAsync();
                            //Console.WriteLine("Result: {0}", result);
                        }
                        else
                        {
                            Console.WriteLine(string.Format("The request failed with status code: {0}", responsem.StatusCode));

                            // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                            // Console.WriteLine(responsem.Headers.ToString());

                            string responseContent = await responsem.Content.ReadAsStringAsync();
                            //Console.WriteLine(responseContent);
                        }
                    }





                    /* if (questReply == null)
                         {
                             questReply = "Will revert back to you soon ! ";
                         }
                         questReply += keyPhrases;*/
                  //  Activity reply = activity.CreateReply(keyPhrases);
                    Activity reply = activity.CreateReply($"{intent.toString()} {keyPhrases} ");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                    //var keyPhrases = await TextAnalyticsAPI.getKeyPhrases(activity.Text);
                     // dynamic intent = await LUISAPI.getIntent(activity.Text);
                      //Activity reply = activity.CreateReply($"{intent.toString()} {keyPhrases} ");
                      //await connector.Conversations.ReplyToActivityAsync(reply);
                  } else
                  {
                      Activity reply = activity.CreateReply($"Key phrase is {response1}");
                      await connector.Conversations.ReplyToActivityAsync(reply);
                      //Activity reply = activity.CreateReply($"key phrases are {sentimentScore}..");

                  }


                  ////var POSScore = await LinguisticAnalytics.getPOS(activity.Text);

                  //dynamic intent = await LUISAPI.getIntent(activity.Text);
                  //var sentimentScore = await TextAnalyticsAPI.getKeyPhrases(activity.Text);


                  /*     if (sentimentScore > 0.7)
                       {
                           message = $"That's great to hear and sentment is{sentimentScore}!";
                       }
                       else if (sentimentScore < 0.3)
                       {
                           message = $"I'm sorry to hear that and sentment is{sentimentScore}...";
                       }
                       else
                       {
                           message = $"I see. and sentment is{sentimentScore}..";
                       } */

                //message = sentimentScore;

                // return our reply to the user

                //Activity reply = activity.CreateReply($"key phrases are {sentimentScore}..");
                //Activity reply = activity.CreateReply($"Intent is  {intent.toString()}..");

                //    await connector.Conversations.ReplyToActivityAsync(reply);

              //  dynamic POSScore = await LinguisticAnalytics.getPOS(activity.Text);

            }
            else
            {
                HandleSystemMessage(activity);
            }
            end:
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
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
 