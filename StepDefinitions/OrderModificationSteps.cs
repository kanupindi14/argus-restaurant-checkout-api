using TechTalk.SpecFlow;
using RestSharp;
using Newtonsoft.Json.Linq;
using ArgusRestaurentCheckoutAutomation.Pages;
using Xunit;
using ArgusRestaurentCheckoutAutomation.Utils;

namespace ArgusRestaurentCheckoutAutomation.StepDefinitions
{
    [Binding]
    public class OrderModificationSteps
    {
        private readonly CheckoutPage checkoutPage;
        private RestResponse? response;
        private RestResponse? modifiedResponse;
        private string? orderId; 

        public OrderModificationSteps() => checkoutPage = new CheckoutPage();

        // [Given(@"a group of (.*) places an order")]
        // public void GivenGroupPlacesAnOrder(int groupSize)
        // {
            
        //     Logger.Info($"Group of {groupSize} initiated an order.");
        // }

        [When(@"they place an order (.*) starters, (.*) mains, and (.*) drinks_")]
        public async Task WhenTheyOrder(int s, int m, int d)
        {
            response = await checkoutPage.PlaceOrder(s, m, d);
            var content = response?.Content ?? "{}";
            var json = JObject.Parse(content);
            orderId = json["orderId"]?.ToString();

            Logger.Info($"Original order ID: {orderId}");

        }

        [Then(@"the API should return the correct total")]
        public void ThenInitialTotalReturned()
        {
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
        }

        [When(@"one member cancels their order")]
        public async Task WhenOneCancels()
        {
            if (string.IsNullOrEmpty(orderId))
                throw new InvalidOperationException("Order ID is null, cannot cancel.");

            var client = new RestClient("http://localhost:5000");
            var request = new RestRequest("/modify-order", Method.Patch);
            request.AddJsonBody(new
            {
                orderId = orderId,
                cancelItems = new { starters = 1, mains = 1, drinks = 1 }
            });

            modifiedResponse = await client.ExecuteAsync(request);
            
            Logger.Info($"Order modified. Response status: {modifiedResponse?.StatusCode}");

        }

        [Then(@"the API should recalculate the total correctly and adjust the service charge accordingly")]
        public void ThenTotalIsRecalculated()
        {
            Assert.Equal(System.Net.HttpStatusCode.OK, modifiedResponse?.StatusCode);
        }
    }
}
