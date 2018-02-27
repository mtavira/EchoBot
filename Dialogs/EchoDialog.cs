using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace K2DemoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        private Activity _activity;
        protected int count = 1;

        public EchoDialog(Activity activity)
        {
            _activity = activity;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.Auto);
            }
            if (message.Text == "activity")
            {
                var msg = "";
                if (_activity != null)
                {
                    msg = string.Format("activity details {0}", _activity.Summary);

                }
                await context.PostAsync(msg);
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync($"{this.count++}: You said {message.Text}");
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }
        
    }
}