using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace K2DemoBot.Dialogs
{
    [Serializable]
    public class K2ProcessesDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(K2MessageReceivedAsync);
        }

        private async Task K2MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterConfirmProductName,
                    "Confirm product name:",
                    "Didn't get that!",
                    promptStyle: PromptStyle.Auto);
            }

            await context.PostAsync($"hello these are trhe processes you can start: Discount Request");

            context.Wait(K2MessageReceivedAsync);
        }

        private async Task AfterConfirmProductName(IDialogContext context, IAwaitable<bool> result)
        {
            var confirm = await result;

            if (confirm)
            {
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(K2MessageReceivedAsync);
        }
    }
}