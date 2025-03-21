namespace ArgusRestaurentCheckoutAutomation.Utils
{
    public static class Logger
    {
        public static void Info(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        public static void Error(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }

        public static void Debug(string message)
        {
            Console.WriteLine($"[DEBUG] {message}");
        }
    }
}
