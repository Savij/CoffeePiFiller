using System;

namespace CoffeePiFiller
{
    public class LogWrapper
    {
        public static void Log(string format, params object[] args)
        {
            if (CoffeeConstants.DebugOutput)
            {
                Console.WriteLine(format, args);
            }
        }

        public static void Log(string message)
        {
            if (CoffeeConstants.DebugOutput)
            {
                Console.WriteLine(message);
            }
        }

        public static void Log(string format, object arg0)
        {
            if (CoffeeConstants.DebugOutput)
            {
                Console.WriteLine(format, arg0);
            }
        }
    }
}