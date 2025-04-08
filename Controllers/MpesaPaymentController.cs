using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectManagementSystem.Controllers
{
    public class MpesaPaymentController
    {
        private readonly string _consumerKey = "YOUR_CONSUMER_KEY"; // Replace with your Safaricom API key
        private readonly string _consumerSecret = "YOUR_CONSUMER_SECRET"; // Replace with your Safaricom API secret
        private readonly string _passKey = "YOUR_PASS_KEY"; // Replace with your pass key
        private readonly string _callbackUrl = "https://yourdomain.com/api/mpesa/callback"; // Your callback URL
        private readonly string _businessShortCode = "174379"; // Replace with your business short code
        private readonly string _transactionType = "CustomerPayBillOnline";

        public string InitiatePayment(string phoneNumber, decimal amount, string description)
        {
            try
            {
                // Generate a unique transaction reference
                string transactionRef = "PMS" + DateTime.Now.ToString("yyMMddHHmmss");

                // In a production environment, you would make the actual API call to M-Pesa
                // For this example, we'll simulate the API call

                /*
                // Real implementation would look something like this:
                var token = GetAccessToken().Result;
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var password = GeneratePassword(timestamp);
                
                var requestPayload = new 
                {
                    BusinessShortCode = _businessShortCode,
                    Password = password,
                    Timestamp = timestamp,
                    TransactionType = _transactionType,
                    Amount = (int)amount,
                    PartyA = phoneNumber,
                    PartyB = _businessShortCode,
                    PhoneNumber = phoneNumber,
                    CallBackURL = _callbackUrl,
                    AccountReference = transactionRef,
                    TransactionDesc = description
                };
                
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    
                    var jsonContent = JsonConvert.SerializeObject(requestPayload);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    
                    var response = client.PostAsync("https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest", content).Result;
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    
                    // Process response
                }
                */

                // Log the transaction in your database
                LogTransaction(phoneNumber, amount, description, transactionRef);

                return transactionRef;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error initiating M-Pesa payment: " + ex.Message);
                return string.Empty;
            }
        }

        private void LogTransaction(string phoneNumber, decimal amount, string description, string transactionRef)
        {
            // In a real implementation, save this to your database
            // For this example, we're just simulating the logging
            Console.WriteLine($"Transaction logged: {transactionRef} - {phoneNumber} - {amount} - {description}");
        }

        private async Task<string> GetAccessToken()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(_consumerKey + ":" + _consumerSecret));
                    client.DefaultRequestHeaders.Add("Authorization", "Basic " + auth);

                    var response = await client.GetAsync("https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials");
                    var content = await response.Content.ReadAsStringAsync();

                    dynamic result = JsonConvert.DeserializeObject(content);
                    return result.access_token;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting access token: " + ex.Message);
                return string.Empty;
            }
        }

        private string GeneratePassword(string timestamp)
        {
            string str = _businessShortCode + _passKey + timestamp;
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        // This method would be called by your API endpoint that receives M-Pesa callbacks
        public void ProcessCallback(string callbackJson)
        {
            try
            {
                // Parse the callback JSON
                dynamic callback = JsonConvert.DeserializeObject(callbackJson);

                // Extract the transaction details
                string resultCode = callback.Body.stkCallback.ResultCode;
                string resultDesc = callback.Body.stkCallback.ResultDesc;
                string merchantRequestID = callback.Body.stkCallback.MerchantRequestID;
                string checkoutRequestID = callback.Body.stkCallback.CheckoutRequestID;

                if (resultCode == "0")
                {
                    // Transaction was successful
                    // Update your database with the payment information
                    string mpesaReceiptNumber = callback.Body.stkCallback.CallbackMetadata.Item[1].Value;
                    string transactionDate = callback.Body.stkCallback.CallbackMetadata.Item[3].Value;
                    string phoneNumber = callback.Body.stkCallback.CallbackMetadata.Item[4].Value;
                    decimal amount = callback.Body.stkCallback.CallbackMetadata.Item[0].Value;

                    UpdateTransactionStatus(merchantRequestID, "Completed", mpesaReceiptNumber);
                }
                else
                {
                    // Transaction failed
                    UpdateTransactionStatus(merchantRequestID, "Failed", resultDesc);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing M-Pesa callback: " + ex.Message);
            }
        }

        private void UpdateTransactionStatus(string transactionRef, string status, string details)
        {
            // In a real implementation, update your database with the transaction status
            // For this example, we're just simulating the update
            Console.WriteLine($"Transaction {transactionRef} updated: {status} - {details}");
        }
    }
}