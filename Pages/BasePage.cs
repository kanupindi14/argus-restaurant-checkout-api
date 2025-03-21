using RestSharp;
using Newtonsoft.Json.Linq;
using ArgusRestaurentCheckoutAutomation.Utils;

namespace ArgusRestaurentCheckoutAutomation.Pages
{
    public class BasePage
    {
        protected readonly RestClient Client;

        public BasePage()
        {
            var configContent = File.ReadAllText("Utilities/Config.json");
            var json = JObject.Parse(configContent);
            var baseUrl = json["baseUrl"]?.ToString() ?? "http://localhost:5000";
            Client = new RestClient(baseUrl);
        }

        protected async Task<RestResponse> PostAsync(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(body);

            Logger.Info($"POST Request to {endpoint} with body: {JObject.FromObject(body)}");

            var response = await Client.ExecuteAsync(request);

            Logger.Info($"Response Status: {response.StatusCode}, Body: {response.Content}");

            return response;
        }
    }
}
