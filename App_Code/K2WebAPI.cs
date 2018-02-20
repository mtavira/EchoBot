using System;
using System.Text;
using K2DemoBot.Models;
using System.Net.Http;

using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;

namespace K2DemoBot.App_Code
{
    public class MyLogger
    {
        public string Log { get; set; }

        public MyLogger()
        {
            Log = string.Empty;
        }
        public void LogLine(string line)
        {
            Log += line + Environment.NewLine;
        }
    }

    public class K2WebAPI
    {
        private static string username = "moises@Denallixtecheu.onmicrosoft.com";
        private static string password = "K2passeu!";
        private Uri rootAPIPath;
        private MyLogger log = new MyLogger();
        //System.Net.Http.http myClient;

        public K2WebAPI()
        {
            
        

        }

        //private async Task<IOutputSpeech> ProcessIntentRequestAsync(IntentRequest theIntent, Session session, Context context)
        //{
        //    if (theIntent.Intent.Name.Trim().ToUpper().Equals("HELP"))
        //    {
        //        this.innerResponse = new PlainTextOutputSpeech
        //        {
        //            Text = "Sorry, this functionality is under construction."
        //        };
        //    }
        //    if (theIntent.Intent.Name.Trim().ToUpper().Equals("GETTASKS"))
        //    {
        //        int count = 0;
        //        using (System.Net.Http.httHttpClient client = new HttpClient())
        //        {
        //            string value = "Basic " + Function.Base64Encode(Function.username + ":" + Function.password);
        //            client.DefaultRequestHeaders.Add("Authorization", value);
        //            TaskList taskList = JsonConvert.DeserializeObject<TaskList>(await client.GetStringAsync(new Uri(rootAPIPath, @"tasks")));
        //            if (taskList != null)
        //            {
        //                count = taskList.itemCount;
        //            }
        //        }
        //        this.innerResponse = new PlainTextOutputSpeech
        //        {
        //            Text = string.Format("You have {0} pending tasks.", count)
        //        };
        //    }

        //    theIntent.Intent.Name.Trim().ToUpper().Equals("OPENFIRSTTASK"); //TODO

        //    if (theIntent.Intent.Name.Trim().ToUpper().Equals("PRODUCTLISTPRICE"))
        //    {
        //        double dListPrice = 0;
        //        string sProductName = Function.TryGetSlotValueString(theIntent.Intent.Slots, "ProductName");

        //        log.LogLine("Identified slot 'ProductName' with value " + sProductName);

        //        sProductName = SanitizeK2Products(sProductName);

        //        // Call OData endpoint to get the price
        //        dListPrice = GetProductListPrize(sProductName);

        //        this.innerResponse = new PlainTextOutputSpeech
        //        {
        //            Text = string.Format("The list price of {0} is £{1}.", sProductName, dListPrice.ToString())
        //        };
        //    }

        //    theIntent.Intent.Name.Trim().ToUpper().Equals("PRODUCTSTOCK"); //TODO
        //    if (theIntent.Intent.Name.Trim().ToUpper().Equals("REQUESTHOLIDAYS"))
        //    {
        //        this.log.LogLine(string.Format("REQUESTHOLIDAYS INTENT from={0} to={1}", theIntent.Intent.Slots["startDate"].Value, theIntent.Intent.Slots["endDate"].Value));
        //        DateTime dateTime = Function.TryGetSlotValueDateTime(theIntent.Intent.Slots, "startDate");
        //        DateTime dateTime2 = Function.TryGetSlotValueDateTime(theIntent.Intent.Slots, "endDate");
        //        this.log.Log(string.Format("Identified slots fromDate={0}, toDate={1}", dateTime.ToString("dd/MM/yyyy"), dateTime2.ToString("dd/MM/yyyy")));
        //        this.innerResponse = new PlainTextOutputSpeech
        //        {
        //            Text = string.Format("Your holidays request has been submitted with id {0}.", 124)
        //        };
        //    }
        //    if (theIntent.Intent.Name.Trim().ToUpper().Equals("DISCOUNTREQUEST"))
        //    {
        //        this.log.LogLine("DISCOUNTREQUEST INTENT =" + theIntent.ToString());

        //        int iProcessInstanceID = 0;
        //        int iDiscountRequested = Function.TryGetSlotValueInt(theIntent.Intent.Slots, "DiscountRequested");
        //        string productName = Function.TryGetSlotValueString(theIntent.Intent.Slots, "ProductName");

        //        log.LogLine("Identified slot 'ProductName' with value " + productName);
        //        log.LogLine("Identified slot 'DiscountRequested' with value " + iDiscountRequested.ToString());

        //        productName = SanitizeK2Products(productName);

        //        this.log.LogLine(string.Format("DISCOUNTREQUEST Found slots  DiscountRequested={0} and ProductName={1} ", iDiscountRequested, productName));

        //        using (HttpClient myHttpClient = new HttpClient())
        //        {
        //            string value2 = "Basic " + Function.Base64Encode(Function.username + ":" + Function.password);
        //            myHttpClient.DefaultRequestHeaders.Add("Authorization", value2);
        //            Workflow workflow = new Workflow();
        //            workflow.folio = "Test from Alexa";
        //            workflow.dataFields = new Dictionary<string, object>(3);
        //            workflow.dataFields.Add("api_CustomerName", "Shell");
        //            workflow.dataFields.Add("api_DiscountRequested", iDiscountRequested);
        //            workflow.dataFields.Add("api_ProductName", productName);
        //            string wfInstanceSerialized = JsonConvert.SerializeObject(workflow);

        //            this.log.LogLine("DISCOUNTREQUEST Request = " + wfInstanceSerialized);

        //            HttpResponseMessage myResponse = await myHttpClient.PostAsync(
        //                new Uri(rootAPIPath, "workflows/32"),
        //                new StringContent(wfInstanceSerialized, Encoding.UTF8, "application/json"));

        //            var responseStatusCode = myResponse.StatusCode;
        //            var responseContents = await myResponse.Content.ReadAsStringAsync();
        //            int.TryParse(responseContents, out iProcessInstanceID);

        //            log.LogLine("DISCOUNTREQUEST Process started successfully with ID" + iProcessInstanceID.ToString());
        //        }
        //        this.innerResponse = new PlainTextOutputSpeech
        //        {
        //            Text = string.Format("Your discount of {0} percent for {1} has been requested with Request ID {2}",
        //            iDiscountRequested, productName, iProcessInstanceID)
        //        };
        //    }
        //    return this.innerResponse;
        //}

        public static string Base64Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        public static string Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        //public static Slot TryGetSlotValue(Dictionary<string, Slot> lstSlots, string slotName)
        //{
        //    Slot slotValue = null;
        //    if (lstSlots != null && lstSlots.Count > 0 && lstSlots.ContainsKey(slotName))
        //    {
        //        slotValue = lstSlots[slotName];
        //    }
        //    return slotValue;
        //}

        //public static string TryGetSlotValueString(Dictionary<string, Slot> lstSlots, string slotName)
        //{
        //    string slotValue = string.Empty;
        //    if (lstSlots != null && lstSlots.Count > 0 && lstSlots.ContainsKey(slotName) && lstSlots[slotName].Value != null)
        //    {
        //        slotValue = lstSlots[slotName].Value;
        //    }
        //    return slotValue;
        //}

        //public static DateTime TryGetSlotValueDateTime(Dictionary<string, Slot> lstSlots, string slotName)
        //{
        //    DateTime slotValueDateTime = default(DateTime);
        //    if (DateTime.TryParse(Function.TryGetSlotValueString(lstSlots, slotName), out slotValueDateTime))
        //    {
        //        return slotValueDateTime;
        //    }
        //    return DateTime.MinValue;
        //}

        //public static int TryGetSlotValueInt(Dictionary<string, Slot> lstSlots, string slotName)
        //{
        //    int slotValueInt = default(int);
        //    if (int.TryParse(Function.TryGetSlotValueString(lstSlots, slotName), out slotValueInt))
        //    {
        //        return slotValueInt;
        //    }
        //    return -2147483648;
        //}

        //public double GetProductListPrize(string ProductName)
        //{
        //    log.LogLine("Entering GetProductListPrize for '" + ProductName + "'");

        //    double productListPrice = 0;

        //    using (System.Net.Http.HttpClientFactory.Create( .HttpClient myClient = new HttpClient())
        //    {
        //        //https://denallixtecheu.onk2.com/api/odata/v3/com_K2_App_DiscountApprovalApp_SmartObject_ProductName?$filter=Title%20eq%20%27K2%20Cloud%27
        //        string sJSONPayload = string.Empty;
        //        string oDataQuery = @"com_K2_App_DiscountApprovalApp_SmartObject_ProductName";
        //        //string oDataQuery = string.Format(@"com_K2_App_DiscountApprovalApp_SmartObject_ProductName?$filter=Title eq '{0}'", ProductName);

        //        Uri ODataBaseUri = new Uri("https://denallixtecheu.onk2.com/api/odata/v3/", UriKind.Absolute);
        //        Uri queryProduct = new Uri(ODataBaseUri, System.Web.HttpUtility.UrlEncode(oDataQuery));

        //        myClient.DefaultRequestHeaders.Add("Authorization",
        //            "Basic " + Base64Encode(username + ":" + password));

        //        System.Threading.Tasks.Task<string> myTask = myClient.GetStringAsync(queryProduct);

        //        myTask.Wait();

        //        log.LogLine("The OData query result is " + myTask.Result);

        //        Models.ODataRespose oResponse =
        //            JsonConvert.DeserializeObject<ODataRespose>(myTask.Result);

        //        if ((oResponse != null) && (oResponse.Products != null) && (oResponse.Products.Count > 0))
        //        {
        //            foreach (var product_i in oResponse.Products)
        //            {
        //                if (product_i.Title.Trim().ToUpperInvariant().Contains(ProductName.Replace("K2", "").Trim().ToUpperInvariant()))
        //                {
        //                    productListPrice = product_i.List_Price;
        //                    log.LogLine("Found a matching entry: " + product_i.Title + " with price " + product_i.List_Price.ToString());
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    log.LogLine("Exiting GetProductListPrize with result = " + productListPrice.ToString());

        //    return productListPrice;
        //}

        public string SanitizeK2Products(string sRawInput)
        {
            string productName = "K2 Five";

            if (sRawInput.Trim().ToUpper().Contains("CLOUD"))
            {
                productName = "K2 Cloud";
            }
            if (sRawInput.Trim().ToUpper().Contains("FIVE") || sRawInput.Trim().ToUpper().Contains("5"))
            {
                productName = "K2 Five";
            }
            if (sRawInput.Trim().ToUpper().Contains("CONNECT"))
            {
                productName = "K2 Connect";
            }
            return productName;
        }
    }
}