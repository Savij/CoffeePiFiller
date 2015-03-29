using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.Components.Converters.Mcp3008;
using Raspberry.IO.Components.Sensors;
using Raspberry.IO.GeneralPurpose;

namespace CoffeePiFiller
{
    /// <summary>
    /// Main class for use with ADC - not tested, use at your own risk (or hack code changes)
    /// Should be close though.
    /// <important>SETUP YOUR PINS IN THE FIELDS BELOW IF DIFFERNET!!!!</important>
    /// </summary>
    public class MCP3008Shell : IShell
    {

        #region private fields

        private ConnectorPin SCLK = ConnectorPin.P1Pin23;
        private ConnectorPin MISO = ConnectorPin.P1Pin21;
        private ConnectorPin MOSI = ConnectorPin.P1Pin19;
        private ConnectorPin CE0 = ConnectorPin.P1Pin24;
        private OutputPinConfiguration solenoid = ConnectorPin.P1Pin15.Output();
        private GpioConnection _solenoidConnection = null;
        private bool _isFilling = false;

        #endregion

        public bool IsDebugMode { get; set; }

        public void Process()
       {
          
           _solenoidConnection = new GpioConnection(solenoid);
           var blinkDetection = new BlinkDetection();
           blinkDetection.BlinkDetection_IsBlinking += BlinkDetectionIsBlinking;


           var driver = GpioConnectionSettings.DefaultDriver;
           using (var adcConnection = new Mcp3008SpiConnection(
               driver.Out(SCLK),
               driver.Out(CE0),
               driver.In(MISO),
               driver.Out(MOSI)))
           {
               using (var lightConnection = new VariableResistiveDividerConnection(
                   adcConnection.In(Mcp3008Channel.Channel0),
                   ResistiveDivider.ForLowerResistor(10000)))
               {
                   Console.CursorVisible = false;

                   while (!Console.KeyAvailable)
                   {                       
                       decimal resistor = lightConnection.GetResistor(); //ohms
                       var lux = resistor.ToLux();
                       if (!_isFilling)
                       {
                           blinkDetection.AddReading(lux);
                       }
                       
                       Console.WriteLine("Light = {0,5:0.0} Lux ({1} ohms)", lux, (int)resistor);

                       Console.CursorTop--;

                       Thread.Sleep(250);
                   }
               }
           }

           Console.CursorTop++;
           Console.CursorVisible = true;
       }

       private  void BlinkDetectionIsBlinking(object sender, EventArgs e)
       {
           _isFilling = true;
           // if this fires, we are blinking
           FillTank();
           _isFilling = false;
       }

       private async void FillTank()
        {
           await ActivateSolenoid();
           _solenoidConnection[solenoid] = false;
        }

       public async Task ActivateSolenoid()
       {
           await Task.Delay(CoffeeConstants.FillTime);
           _solenoidConnection[solenoid] = true;
           
       }
    }
}
