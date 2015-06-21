namespace CoffeePiFiller
{
    public class LogWrapper
    {
        public static void Log(string format, params object[] args)
        {
            if (CoffeeConstants.DebugOutput)
            {
                Log(format, args);
            }
        }

        public static void Log(string message)
        {
            if (CoffeeConstants.DebugOutput)
            {
                Log(message);
            }
        }

        public static void Log(string format, object arg0)
        {
            if (CoffeeConstants.DebugOutput)
            {
                Log(format, arg0);
            }
        }
    }
}