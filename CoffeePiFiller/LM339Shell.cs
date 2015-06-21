#region Using Statements

using System;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;

#endregion

namespace CoffeePiFiller
{
    /// <summary>
    /// Main class for use with LM339 comparator circuit
    /// <important>SET YOUR PINS, I used 18 for the CDS cell and 13 for solenoid! These are PI numbers NOT GPIO numbers!</important>
    /// </summary>
    public class LM339Shell : IShell
    {
        #region private fields
        // SET THESE TWO PINS TO WHAT YOU USE!!
        private InputPinConfiguration _lightSensor = ConnectorPin.P1Pin18.Input();
        private OutputPinConfiguration _solenoid = ConnectorPin.P1Pin13.Output();
        private BlinkDetection _blinkDetection = new BlinkDetection();        
        private GpioConnection _solenoidConnection;
        private bool _isDebugMode = false;
        private bool _isFilling;
        private Timer _timer = null;
        #endregion

        #region properties

        public bool IsDebugMode { get; set; }

        #endregion

        /// <summary>
        /// Ok so I dont like the method name either, but it is what it is.
        /// </summary>
        public void Process()
        {
            _solenoidConnection = new GpioConnection(_solenoid);
            _blinkDetection.BlinkDetection_IsBlinking += BlinkDetectionIsBlinking;
            _blinkDetection.BlinkDetection_ReadingFalsePositives += OnReadingFalsePositives;
            var connection = new GpioConnection(_lightSensor);
            connection.PinStatusChanged += ConnectionOnPinStatusChanged;
        }

        /// <summary>
        /// When Comparator sends us a value we read it and record the reading.
        /// This is why I like comparators for this better than ADC. Get the event
        /// and DONE!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionOnPinStatusChanged(object sender, PinStatusEventArgs e)
        {
            if (!_isFilling)
            {
                if (e.Enabled)
                {
                    LogWrapper.Log("Pin Enabled...");
                    _blinkDetection.AddReading(BlinkDetection.BLINK_STATES.HI);
                }
                else
                {
                    LogWrapper.Log("Pin Disabled...");
                    _blinkDetection.AddReading(BlinkDetection.BLINK_STATES.LOW);
                }
            }
        }

        /// <summary>
        /// Blink detected - This event is the result of an analyzed list of blinks.
        /// Time to fill the tank!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BlinkDetectionIsBlinking(object sender, EventArgs e)
        {
            LogWrapper.Log("POT IS BLINKING!!!!!");
            _isFilling = true;
            LogWrapper.Log("ACTIVATING SOLENOID!!!!!");
            if (IsDebugMode)
            {
                Thread.Sleep(6000); // simulate something hapening for debug purposes.
            }
            else
            {
                var task = ActivateSolenoid();
                var result = await task;
                LogWrapper.Log(result);
            }
            _isFilling = false;
        }

        /// <summary>
        /// False positive detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReadingFalsePositives(object sender, EventArgs e)
        {
            LogWrapper.Log("READING FALSE POSITIVES, CLEARING READINGS!!!");
        }

        /// <summary>
        /// Turn solenoid on, fill er' up!
        /// </summary>
        /// <returns></returns>
        public async Task<string> ActivateSolenoid()
        {
            _solenoidConnection[_solenoid] = true;
            LogWrapper.Log("WAITING FOR {0} MS...", CoffeeConstants.FillTime);
            var delayTask = Task.Delay(CoffeeConstants.FillTime);
            await delayTask;
            _solenoidConnection[_solenoid] = false;
            LogWrapper.Log("DEACTIVATING SOLENOID!!!!!");
            return "...Tank Filled...";
        }
    }
}