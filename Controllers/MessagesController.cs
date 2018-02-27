using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;

namespace K2DemoBot
{

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                string msgString = activity.Text.ToLower();

                if (msgString.ToLower().Contains("hi"))
                {
                    ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                    string replystring = "Welcome to the Price Enquiry system! Ask the 'price' of any of our products: K2 Cloud, K2 Five or K2 Connect.";
                    Activity reply = activity.CreateReply(replystring);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }

                if (msgString.ToLower().Contains("price"))
                {
                    ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                    Activity reply = activity.CreateReply($"The list price");

                    string K2Product = "K2 Cloud";
                    if (msgString.ToLower().Trim().Contains("cloud"))
                        K2Product = "K2 Cloud";
                    if (msgString.ToLower().Trim().Contains("five"))
                        K2Product = "K2 Five";
                    if (msgString.ToLower().Trim().Contains("connect"))
                        K2Product = "K2 Connect";

                    using (K2WebAPI myAPI = new K2WebAPI())
                    {
                        double price = 50000;
                        myAPI.GetProductListPrize(K2WebAPI.SanitizeK2Products(K2Product));
                        reply = activity.CreateReply($"The list price of {K2Product} is {price.ToString()}.");
                    }

                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                else
                    await Conversation.SendAsync(activity, () => new EchoDialog(activity));
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
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