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
                      dynamic entityType = new EntityDetector().identifyType(keyPhrases);

                    var questReply = QuestionRecommender.quest(keyPhrases,intent,entityType ,null);
                    if(questReply == null)
                    {
                        questReply = "Will revert back to you soon ! ";
                    }
                    questReply += keyPhrases;
                      //   Activity reply = activity.CreateReply(keyPhrases);
                      Activity reply = activity.CreateReply($"{intent.toString()} {questReply} ");
                      await connector.Conversations.ReplyToActivityAsync(reply);
                  }
                  else if (response1.Equals("PROBLEM"))
                  {
                      //problem statement
                      var keyPhrases = await TextAnalyticsAPI.getKeyPhrases(activity.Text);
                      dynamic intent = await LUISAPI.getIntent(activity.Text);
                      Activity reply = activity.CreateReply($"{intent.toString()} {keyPhrases} ");
                      await connector.Conversations.ReplyToActivityAsync(reply);
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
 