using TechTalk.SpecFlow;
using RestSharp;
using Newtonsoft.Json.Linq;
using ArgusRestaurentCheckoutAutomation.Pages;
// using ArgusRestaurentCheckoutAutomation.Utils;
using Xunit;


namespace ArgusRestaurentCheckoutAutomation.StepDefinitions
{
    [Binding]
    public class CheckoutSteps
    {
        private readonly CheckoutPage checkoutPage;
        private RestResponse? response;

        public CheckoutSteps()
        {
            checkoutPage = new CheckoutPage();
        }

        // [Given(@"a group of (.*) places an order")]
        // public void GivenGroupPlacesAnOrder(int groupSize)
        // {
        //    Logger.Info($"Group of {groupSize} customers has initiated an order.");
        // }

        [When(@"they order (.*) starters, (.*) mains, and (.*) drinks")]
        public async Task WhenTheyOrderItems(int starters, int mains, int drinks)
        {
            response = await checkoutPage.PlaceOrder(starters, mains, drinks);
        }

        [Then(@"the API should return the correct total with a 10% service charge")]
        public void ThenVerifyTotalWithServiceCharge()
        {
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            var content = response?.Content ?? "{}";
            var json = JObject.Parse(content);
            Assert.True(json["total"]?.Value<decimal>() > 0);
        }
    }
}
