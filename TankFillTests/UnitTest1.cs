using System;
using System.Threading.Tasks;
using CoffeePiFiller;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TankFillTests
{
    [TestClass]
    public class UnitTest1
    {
        private bool _isBlinking = false;
        private BlinkDetection _blinkDetection = null;

        [TestInitialize]
        public void Initialize()
        {
            _blinkDetection = new BlinkDetection();
            _blinkDetection.BlinkDetection_IsBlinking += BlinkDetectionIsBlinking;
        }

        [TestMethod]
        public void Light_Is_On()
        {
            
            for (int i = 0; i < 9; i++)
            {
                _blinkDetection.AddReading(300);
                System.Threading.Thread.Sleep(1000);
                _blinkDetection.AddReading(300);
            }
            Assert.IsFalse(_isBlinking);

        }

        [TestMethod]
        public void Light_Is_Off()
        {
           
            for (int i = 0; i < 9; i++)
            {
                _blinkDetection.AddReading(25);
                System.Threading.Thread.Sleep(1000);
                _blinkDetection.AddReading(25);
            }
            Assert.IsFalse(_isBlinking);

        }

        [TestMethod]
        public  void Light_Is_Blinking()
        {

            for (int i = 0; i < 9; i++)
            {
                _blinkDetection.AddReading(25);
                System.Threading.Thread.Sleep(1000);
                _blinkDetection.AddReading(250);
            }
            Assert.IsTrue(_isBlinking);

        }

        [TestMethod]
        public void False_Positives()
        {

            for (int i = 0; i < 9; i++)
            {
                var random = new Random();

                _blinkDetection.AddReading(25);
                System.Threading.Thread.Sleep(random.Next(2500,3500));
                _blinkDetection.AddReading(250);
            }
            Assert.IsFalse(_isBlinking);

        }

        [TestMethod]
        public void Blinking_Outside_Window()
        {

            for (int i = 0; i < 3; i++)
            {
                _blinkDetection.AddReading(25);
                System.Threading.Thread.Sleep(1000);
                _blinkDetection.AddReading(250);
            }
            for (int i = 0; i < 3; i++)
            {              
                _blinkDetection.AddReading(25);
                System.Threading.Thread.Sleep(3000);
                _blinkDetection.AddReading(250);
            }
            Assert.IsFalse(_isBlinking);

        }

        private void BlinkDetectionIsBlinking(object sender, EventArgs e)
        {
            _isBlinking = true;
        }
    }
}
