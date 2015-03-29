#region Using Statements

using System;
using System.Collections.Generic;

#endregion

namespace CoffeePiFiller
{

    public class BlinkDetection
    {
        public enum BLINK_STATES
        {
            LOW,
            HI
        }

        private bool _isLow = false;
        private List<DateTime> _blinkList = new List<DateTime>();
        // event for blinking
        public delegate void IsBlinkingHandler(object sender, EventArgs e);
        public event IsBlinkingHandler BlinkDetection_IsBlinking;

        // event for false positives
        public delegate void FalsePositivesHandler(object sender, EventArgs e);
        public event FalsePositivesHandler BlinkDetection_ReadingFalsePositives;

        /// <summary>
        /// call this when you know you have a blink,
        /// good for use with LM393 or LM339 (Comparator)
        /// </summary>
        /// <param name="reading"></param>
        public void AddReading(BLINK_STATES reading)
        {
            switch (reading)
            {
                case BLINK_STATES.LOW:
                    _isLow = true;
                    break;
                case BLINK_STATES.HI:
                    if (_isLow)
                    {
                        _blinkList.Add(DateTime.Now);
                        _isLow = false;
                    }
                    else
                    {
                        _isLow = false;
                    }
                    break;
            }
            CheckIfBlinking();
        }


        /// <summary>
        /// Call this with each reading of an LCM3008 ADC
        /// </summary>
        /// <param name="currentReading"></param>
        public void AddReading(decimal currentReading)
        {
            var reading = RateReading(currentReading);
            AddReading(reading);
        }

        /// <summary>
        /// This is the logic to see if we have valid blinks in our list. 
        /// It compares the Times from DateTime stamps to make sure the blinks
        /// happen within some range set in app.config (BLINK_TIME_PLUS_BUFFER).
        /// If there are (MIN_BLINKS_FOR_POSITIVE) of them, then the keurig is 
        /// flashing!
        /// <important>MAKE SURE YOU HAVE THE LATEST MONO WITH HARD FLOATING POINT SUPPORT IF NOT THIS WONT WORK!</important>
        /// </summary>
        private void CheckIfBlinking()
        {
            if (_blinkList.Count < CoffeeConstants.MinBlinksForPositive)
            {
                return;
            }
            // mine blinks about once per second.
            // if any of the times in the list are more than that plus some buffer,
            // they are false positives. We need 6 good blinks
            DateTime lastTime = _blinkList[0].Add(new TimeSpan(0,0,-1));
            var positiveCount = 0;

            for (int i = 0; i < _blinkList.Count; i++)
            {
                var blinkTime = _blinkList[i];

                if (blinkTime.Subtract(lastTime).TotalSeconds <= CoffeeConstants.BlinkTimePlusBuffer)
                {
                    positiveCount ++;
                }
                lastTime = blinkTime;
            }
            if (positiveCount >= CoffeeConstants.MinBlinksForPositive)
            {
                // We are reading a blink, fire an event
                _blinkList.Clear();
                OnBlinkDetectionIsBlinking();
            }

            if (_blinkList.Count >= CoffeeConstants.MaxNumberOfSamples)
            {
                _blinkList.Clear();
                OnBlinkDetectionReadingFalsePositives();
            }
            
        }

        /// <summary>
        /// This method is for rating a reading from an ADC. It has NOT been tested, but *should* work.
        /// It checks for ranges that you set in the app.config for what is a valid range of off and on
        /// The idea is that some number between x and y should be considered 'Off' and anything above z
        /// is on. So, LowOff to HighOff should be considered off. Get it?
        /// </summary>
        /// <param name="currentReading"></param>
        /// <returns></returns>
        private BLINK_STATES RateReading(decimal currentReading)
        {
            if (currentReading >= CoffeeConstants.LowOff && currentReading <= CoffeeConstants.HighOff)
            {
                return BLINK_STATES.LOW;
            }

            if (currentReading >= CoffeeConstants.LowOn)
            {
                return BLINK_STATES.HI;
            }

            // just in case of weird values, just return low
            return BLINK_STATES.LOW;
        }

        /// <summary>
        /// Fires Event for blinking
        /// </summary>
        protected virtual void OnBlinkDetectionIsBlinking()
        {
            IsBlinkingHandler handler = BlinkDetection_IsBlinking;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires Event for false positives
        /// </summary>
        protected virtual void OnBlinkDetectionReadingFalsePositives()
        {
            FalsePositivesHandler handler = BlinkDetection_ReadingFalsePositives;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }

    
}