using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;

namespace Bot_Application3.Dialogs
{
    [LuisModel("78cfd1713db84941a9c0aba03fac04cf", "78cfd1713db84941a9c0aba03fac04cf")]
    [Serializable]
    public class DefaultDialog : LuisDialog<object>
    {
        [LuisIntent("crashing")]
        public async Task crashing(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("The intent is crashing");
            context.Wait(MessageReceived);
        }

        [LuisIntent("incorrect")]
        public async Task incorrect(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("The intent is incorrect");
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task nullIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("The intent is incorrect");
            context.Wait(MessageReceived);
        }

    }
}