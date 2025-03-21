using TechTalk.SpecFlow;
using RestSharp;
using Newtonsoft.Json.Linq;
using ArgusRestaurentCheckoutAutomation.Pages;
using ArgusRestaurentCheckoutAutomation.Utils;
using Xunit;


namespace ArgusRestaurentCheckoutAutomation.StepDefinitions
{
    [Binding]
    public class MultiOrderSteps
    {
        private readonly CheckoutPage checkoutPage;
        private RestResponse? response1;
        private RestResponse? response2;

        public MultiOrderSteps() => checkoutPage = new CheckoutPage();

        [Given(@"two people place an order")]
        public void GivenTwoPeoplePlaceAnOrder()
        {
            Logger.Info($"First two customers placing their order...");
        }

        [When(@"they place an order with (.*) mains and (.*) drinks")]
        public async Task WhenTheyOrderMainsAndDrinks(int mains, int drinks)
        {
            response1 = await checkoutPage.PlaceOrder(0, mains, drinks);
            Logger.Info($"First order placed.");
        }

        [Then(@"the API should calculate the total separately")]
        public void ThenApiShouldCalculateTotalSeparately()
        {
            Assert.Equal(System.Net.HttpStatusCode.OK, response1?.StatusCode);
            var content1 = response1?.Content ?? "{}";
            var json1 = JObject.Parse(content1);
            var total1 = json1["total"]?.Value<decimal>() ?? 0;
            Logger.Info($"First order total: £{total1}");
            Assert.True(total1 > 0);
        }

        [When(@"two more people place a separate order within the same group")]
        public async Task WhenTwoMorePeoplePlaceOrder()
        {
            response2 = await checkoutPage.PlaceOrder(0, 1, 1);
            Logger.Info($"First order placed.");
        }

        [Then(@"the API should clarify whether the new order is merged or handled separately")]
        public void ThenClarifyMergeOrSeparate()
        {
            var content1 = response1?.Content ?? "{}";
            var content2 = response2?.Content ?? "{}";

            var orderId1 = JObject.Parse(content1)["orderId"]?.ToString();
            var orderId2 = JObject.Parse(content2)["orderId"]?.ToString();

            Logger.Info($"Order ID 1: {orderId1}");
            Logger.Info($"Order ID 2: {orderId2}");

            Assert.NotNull(orderId1);
            Assert.NotNull(orderId2);
            Assert.NotEqual(orderId1, orderId2); // confirming that both orders are separate
        }
    }
}
