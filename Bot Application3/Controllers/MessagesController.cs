﻿using System;
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

namespace Bot_Application3
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        internal static IDialog<object> MakeRoot()
        {
            return Chain.From(() => new DefaultDialog());
        }
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                 ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                 // calculate something for us to return
                 int length = (activity.Text ?? string.Empty).Length;

                 var sentimentScore = await LUISAPI.getIntent(activity.Text);
                 //string message;// sentimentScore;

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
                 // Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                 Activity reply = activity.CreateReply($"key phrases are {sentimentScore.intents.First}..");
                 await connector.Conversations.ReplyToActivityAsync(reply);

                

            }
            else
            {
                HandleSystemMessage(activity);
            }
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
 