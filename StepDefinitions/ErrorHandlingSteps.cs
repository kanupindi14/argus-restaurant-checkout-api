using TechTalk.SpecFlow;
using RestSharp;
using Newtonsoft.Json.Linq;
using ArgusRestaurentCheckoutAutomation.Pages;
using ArgusRestaurentCheckoutAutomation.Utils;
using Xunit;


namespace ArgusRestaurentCheckoutAutomation.StepDefinitions
{
    [Binding]
    public class ErrorHandlingSteps
    {
        private readonly CheckoutPage _checkoutPage;
        private RestResponse? _response;

        public ErrorHandlingSteps()
        {
            _checkoutPage = new CheckoutPage();
        }

        // [Given(@"a group of (.*) places an order")]
        // public void GivenAGroupPlacesAnOrder(int groupSize)
        // {
        //     Logger.Info($"Group of {groupSize} initiated an order.");
        // }

        [When(@"they order (.*) starters, (.*) mains, and (.*) drink")]
        public async Task WhenTheyOrderInvalidOrder(int starters, int mains, int drinks)
        {
            Logger.Info($"Attempting invalid order: Starters={starters}, Mains={mains}, Drinks={drinks}");
            _response = await _checkoutPage.PlaceOrder(starters, mains, drinks);
        }

        [Then(@"the API should return an error message ""(.*)""")]
        public void ThenTheApiShouldReturnAnErrorMessage(string expected)
        {
            var content = _response?.Content ?? "{}";
            var json = JObject.Parse(content);
            var actualError = json["error"]?.ToString();

            Logger.Error($"API Error Message: {actualError}");

            Assert.Equal(expected, actualError);
        }
    }
}
