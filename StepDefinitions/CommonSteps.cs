using ArgusRestaurentCheckoutAutomation.Utils;
using TechTalk.SpecFlow;


namespace ArgusRestaurentCheckoutAutomation.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        private readonly ScenarioContext scenarioContext;

        public CommonSteps(ScenarioContext scenarioContext_)
        {
            scenarioContext = scenarioContext_;
        }

        [Given(@"a group of (.*) places an order")]
        public void GivenGroupPlacesAnOrder(int groupSize)
        {
            // Common GIVEN statement implementation
            scenarioContext["groupSize"] = groupSize;
            Logger.Info($"Group of {groupSize} customers has initiated an order.");
        }
    }
}
