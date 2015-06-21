using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeePiFiller.Properties;

namespace CoffeePiFiller
{
    public class LogWrapper
    {
        public static void Log(string format, params object[] args)
        {
            if (Settings.Default.DebugOutput)
            {
                LogWrapper.Log(format, args);
            }
        }

        public static void Log(string message)
        {
            if (Settings.Default.DebugOutput)
            {
                LogWrapper.Log(message);
            }
        }

        public static void Log(string format, object arg0)
        {
            if (Settings.Default.DebugOutput)
            {
                LogWrapper.Log(format, arg0);
            }
        }
    }
}
