using TechTalk.SpecFlow;
using RestSharp;
using Newtonsoft.Json.Linq;
using ArgusRestaurentCheckoutAutomation.Pages;
using ArgusRestaurentCheckoutAutomation.Utils;
using Xunit;


namespace ArgusRestaurentCheckoutAutomation.StepDefinitions
{
    [Binding]
    public class CancellationSteps : BasePage
    {
        private readonly CheckoutPage checkoutPage;
        private string? orderId;
        private RestResponse? response;

        public CancellationSteps() => checkoutPage = new CheckoutPage();

        // [Given(@"a group of (.*) places an order")]
        // public void GivenAGroupPlacesAnOrder(int groupSize)
        // {
        //    Logger.Info($"Group of {groupSize} customers has initiated an order.");
        // }

        [When(@"they placed order (.*) starters, (.*) mains, and (.*) drinks.")]
        public async Task WhenOrderIsPlaced(int s, int m, int d)
        {
            var result = await checkoutPage.PlaceOrder(s, m, d);
            var content = result.Content ?? "{}";
            var json = JObject.Parse(content);
            orderId = json["orderId"]?.ToString();
            if (string.IsNullOrEmpty(orderId))
            {
                Logger.Error($"orderId is null or missing in API response:{content}");
                // Console.WriteLine("orderId is null or missing in API response:");
                // Console.WriteLine(content);
                throw new Exception("orderId should not be null or empty");
            }
        }

        [Then(@"the API should return the correct total.")]
        public void ThenOriginalTotal()
        {
            Assert.False(string.IsNullOrWhiteSpace(orderId), "Order ID should not be null or empty");
        }

        [When(@"they cancel the entire order")]
        public async Task CancelOrder()
        {
            var request = new RestRequest("/cancel-order", Method.Delete);
            request.AddJsonBody(new { orderId = orderId });

            response = await Client.ExecuteAsync(request);
        }

        [Then(@"the API should return ""(.*)"" and total should be (.*)")]
        public void ThenCancelledWithZero(string message, int total)
        {
            var content = response?.Content ?? "{}";
            var json = JObject.Parse(content);

            Assert.Equal(message, json["message"]?.ToString());
            Assert.Equal(total, json["total"]?.Value<int>());
        }
    }
}
