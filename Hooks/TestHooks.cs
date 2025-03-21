using System.Diagnostics;
using TechTalk.SpecFlow;

namespace ArgusRestaurentCheckoutAutomation.StepDefinitions
{
    [Binding]
    public class Hooks
    {
        private static Process? mockServerProcess;

        [BeforeTestRun]
        public static void StartMockServer()
        {
            Console.WriteLine("[HOOK] Starting new Mock Test Server...");
            mockServerProcess = new Process();
            mockServerProcess.StartInfo.FileName = "node";
            mockServerProcess.StartInfo.Arguments = "Mock API/restaurentServer.js";
            mockServerProcess.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            mockServerProcess.StartInfo.UseShellExecute = false;
            mockServerProcess.StartInfo.CreateNoWindow = true;
            mockServerProcess.Start();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Console.WriteLine("[HOOK] Starting new scenario...");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine("[HOOK] Scenario completed.");
        }

        [AfterTestRun]
        public static void StopMockServer()
        {
            Console.WriteLine("[HOOK] Stopping Mock Test Server...");
            if (mockServerProcess != null && !mockServerProcess.HasExited)
            {
                mockServerProcess.Kill(true);
                mockServerProcess.Dispose();
            }
        }
    }
}
