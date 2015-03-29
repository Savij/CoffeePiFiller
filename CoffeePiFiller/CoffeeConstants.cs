#region Using Statements

using System;
using System.Configuration;

#endregion

namespace CoffeePiFiller
{
    /// <summary>
    /// Constants from app.config
    /// </summary>
    public class CoffeeConstants
    {
   
        /// <summary>
        /// For ADC - Anything this number of lower should be a low (Light in tank is OFF)
        /// </summary>
        public static int LowOff
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["LOW_OFF"]);
                }
                catch (Exception)
                {
                    
                    return 0;
                }
                
            }
        }

        /// <summary>
        /// For ADC - Anything up to this number should still be a low (Light in tank is OFF)
        /// </summary>
        public static int HighOff
        {
            get {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["HIGH_OFF"]);
                }
                catch (Exception)
                {
                    return 30;
                }
            }
        }

        /// <summary>
        /// For ADC - Anything at this number or above is a high (Light in tank is ON)
        /// </summary>
        public static int LowOn
        {
            get {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["LOW_ON"]);
                }
                catch (Exception)
                {
                    return 200;
                }
            }
        }

        /// <summary>
        /// This is how many blinks you need before you are convinced it's blinking.
        /// The reason I put this in is because I was worried that someone could turn
        /// the coffee maker on and off which in effect turns the tank led on and off (a blink).
        /// I set mine to 6 blinks before it refills. Use at your own risk.
        /// </summary>
        public static int MinBlinksForPositive
        {
            get {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["MIN_BLINKS_FOR_POSITIVE"]);
                }
                catch (Exception)
                {
                    return 6;
                }
            }
        }

        // number of seconds plus an arbitrary buffer to count as a blink.
        /// <summary>
        /// This is the amount of time in seconds that a blink must occur after a previous blink
        /// to count as valid. In my case with MinBlinksForPositive set to 6 and this setting set to 2,
        /// each of the 6 blinks recorded MUST be within 2 second of each other to be considered valid.
        /// This way if a light comes on in the room, or a flashlight hits the sensor, it adds an additional 
        /// check to make sure the blinks came from the coffee maker.
        /// </summary>
        public static int BlinkTimePlusBuffer
        {
            get {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["BLINK_TIME_PLUS_BUFFER"]);
                }
                catch (Exception)
                {
                    return 2;
                }
            }
        }

        // number of total readings to compare before flushing and starting over.
        /// <summary>
        /// Max number of recorded blinks before clearing the list and starting over. This number will cause 
        /// a false positive condition to be met. The default in app.config is 8. If we record 8 blinks
        /// and there are not at least MinBlinksForPositive within BlinkTimePlusBuffer time, then consider all
        /// of the blinks bad and start over. This logic could be better and use a rolling count to look for 
        /// the last MinBlinksForPositive, but I have not gotten around to it....
        /// </summary>
        public static int MaxNumberOfSamples
        {
            get {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["MAX_NUM_OF_SAMPLES"]);
                }
                catch (Exception)
                {
                    return 8;
                }
            }
        }

        /// <summary>
        /// This is how long in milliseconds the solenoid will stay on. I was going to use a sensor to detect low tank and 
        /// filled water height, but I don't want wires in my water!
        /// </summary>
        public static int FillTime // in milliseconds
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["FILL_TIME"]);
                }
                catch (Exception)
                {
                    return 5000;
                }
            }
        }
    }
}