#region Using Statements

using System;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.Components.Converters.Mcp3008;
using Raspberry.IO.Components.Sensors;
using Raspberry.IO.GeneralPurpose;

#endregion

namespace CoffeePiFiller
{
    internal class MainClass
    {


        private static void Main(string[] args)
        {
            bool isDebugMode = args.Length > 0; // only supports debug switch right now, so assume thats the flag for now.

            IShell piShell;
            //piShell = new PiShell(); // This is for an ADC instead of comparator, not tested!!!
            piShell = new LM339Shell {IsDebugMode = isDebugMode};
            piShell.Process();
            LogWrapper.Log("Started Success!");
            Console.ReadLine();
        }


    }
}