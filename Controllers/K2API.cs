using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace K2DemoBot
{
    public class K2WebAPI : IDisposable
    {
        private string username;
        private string password;
        private Uri rootAPIPath;
        private Uri ODataBaseUri;
        private HttpClient myClient;
        private int iProcessInstanceID;

        public K2WebAPI()
        {
            username = Environment.GetEnvironmentVariable("K2WorkflowAPI_User");// "moises@Denallixtecheu.onmicrosoft.com";
            password = Environment.GetEnvironmentVariable("K2WorkflowAPI_Pass");//"K2passeu!";
            string workflowAPIURL = Environment.GetEnvironmentVariable("K2WorkflowAPI_URL");
            string sODataBaseUri = Environment.GetEnvironmentVariable("K2WorkflowAPI_OData");

            if (myClient == null)
            {
                myClient = new HttpClient();
                myClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Base64Encode(username + ":" + password));
            }

            //workflowAPIURL = @"https://denallixtecheu.onk2.com/Api/Workflow/preview/";
            //sODataBaseUri = @"https://denallixtecheu.onk2.com/api/odata/v3/"";
            rootAPIPath = new Uri(workflowAPIURL);
            ODataBaseUri = new Uri(sODataBaseUri, UriKind.Absolute);
        }

        public int StartWorkflow_RequestDiscount(string productName, int iDiscountRequested)
        {
            K2Workflow myWorkflowInstance = new K2Workflow
            {
                folio = "Started via Bot",
                dataFields = new Dictionary<string, object>(3)
            };
            myWorkflowInstance.dataFields.Add("api_CustomerName", "Shell");
            myWorkflowInstance.dataFields.Add("api_DiscountRequested", iDiscountRequested);
            myWorkflowInstance.dataFields.Add("api_ProductName", productName);

            System.Threading.Tasks.Task<int> myTask = StartWorkflowAsync(myWorkflowInstance, 32);

            myTask.Wait();

            iProcessInstanceID = myTask.Result;

            return iProcessInstanceID;
        }

        public double GetProductListPrize(string ProductName)
        {
            double productListPrice = 0;

            //https://denallixtecheu.onk2.com/api/odata/v3/com_K2_App_DiscountApprovalApp_SmartObject_ProductName?$filter=Title%20eq%20%27K2%20Cloud%27
            string sJSONPayload = string.Empty;
            string oDataQuery = @"com_K2_App_DiscountApprovalApp_SmartObject_ProductName";
            //string oDataQuery = string.Format(@"com_K2_App_DiscountApprovalApp_SmartObject_ProductName?$filter=Title eq '{0}'", ProductName);

            Uri ODataBaseUri = new Uri("https://denallixtecheu.onk2.com/api/odata/v3/", UriKind.Absolute);
            Uri queryProduct = new Uri(ODataBaseUri, System.Web.HttpUtility.UrlEncode(oDataQuery));

            // HTTTP GET
            System.Threading.Tasks.Task<string> myTask = myClient.GetStringAsync(queryProduct);

            // Wait
            myTask.Wait();

            ODataRespose oResponse =
                JsonConvert.DeserializeObject<ODataRespose>(myTask.Result);

            ProductName = SanitizeK2Products(ProductName);

            if ((oResponse != null) && (oResponse.Products != null) && (oResponse.Products.Count > 0))
            {
                foreach (var product_i in oResponse.Products)
                {
                    if (product_i.Title.Trim().ToUpperInvariant().Contains(ProductName.Replace("K2", "").Trim().ToUpperInvariant()))
                    {
                        productListPrice = product_i.List_Price;
                        break;
                    }
                }
            }
            return productListPrice;
        }

        private async System.Threading.Tasks.Task<int> StartWorkflowAsync(K2Workflow myWorkflowInstance, int workflowID)
        {
            string wfInstanceSerialized = JsonConvert.SerializeObject(myWorkflowInstance);

            // HTTP POST
            HttpResponseMessage myResponse = await myClient.PostAsync(
                new Uri(rootAPIPath, $"workflows/{workflowID}"),
                new StringContent(wfInstanceSerialized, Encoding.UTF8, "application/json"));

            var responseStatusCode = myResponse.StatusCode;
            var responseContents = await myResponse.Content.ReadAsStringAsync();

            int.TryParse(responseContents, out iProcessInstanceID);

            return iProcessInstanceID;
        }

        public static string Base64Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        public static string Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string SanitizeK2Products(string sRawInput)
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

        public void Dispose()
        {
            ((IDisposable)myClient).Dispose();
        }
    }
}