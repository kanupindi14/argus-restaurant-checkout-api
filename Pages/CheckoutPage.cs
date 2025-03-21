using RestSharp;

namespace ArgusRestaurentCheckoutAutomation.Pages
{
    public class CheckoutPage : BasePage
    {
        public CheckoutPage() : base() { }

        public async Task<RestResponse> PlaceOrder(int starters, int mains, int drinks, string orderTime = null)
        {
            return await PostAsync("/checkout", new
            {
                starters,
                mains,
                drinks,
                orderTime = orderTime ?? "20:00"
            });
        }
    }
}
