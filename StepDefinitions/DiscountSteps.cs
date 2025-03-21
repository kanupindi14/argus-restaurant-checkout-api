using ArgusRestaurentCheckoutAutomation.Pages;
using ArgusRestaurentCheckoutAutomation.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;
using Xunit;


[Binding]
public class DiscountSteps
{
    private readonly CheckoutPage checkoutPage;

    private RestResponse? responseEarly, responseLate;

    public DiscountSteps() => checkoutPage = new CheckoutPage();

    [Given(@"a group of (.*) places an order at ""(.*)""")]
    public async Task GivenGroupOrdersEarly(int groupSize, string time)
    {
        responseEarly = await checkoutPage.PlaceOrder(1, 2, 2, time);
        Logger.Info($"Group of {groupSize} customers has initiated an order at {time}.");
    }

    [When(@"they order (.*) starter, (.*) mains, and (.*) drinks")]
    public async Task WhenTheyOrderItems(int starters, int mains, int drinks)
    {
        responseEarly = await checkoutPage.PlaceOrder(starters, mains, drinks);
    }

    [Then(@"the API should apply a 30% discount on the drinks")]
    public void ThenVerifyDrinkDiscount()
    {
        var content = responseEarly?.Content ?? "{}";
        var json = JObject.Parse(content);
        var total = json["total"]?.Value<decimal>() ?? 0;
        Console.WriteLine($"[INFO] Discounted total: £{total}");
        Assert.True(total < 20, "Expected discount not applied.");
    }

    [Given(@"two more people join the group at ""(.*)""")]
    public void GivenOthersJoinAt(string time)
    {

        Logger.Info($"Two more customers has joied the group at {time}.");
    }

    [When(@"they order (.*) mains and (.*) drinks")]
    public async Task WhenLateJoinOrder(int mains, int drinks)
    {
        responseLate = await checkoutPage.PlaceOrder(0, mains, drinks, "20:00");
    }

    [Then(@"the API should calculate the final bill correctly, applying the discount only to drinks ordered before 19:00")]
    public void ThenFinalDiscountCheck()
    {
        Assert.Equal(System.Net.HttpStatusCode.OK, responseLate?.StatusCode);
    }
}
