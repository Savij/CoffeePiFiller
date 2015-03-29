using System;

namespace CoffeePiFiller
{
	public static class Conversions
	{
        #region Methods

        /// <summary>
        /// Stole this from the adafruit example for ADC conversions to Lux.
        /// See http://learn.adafruit.com/photocells/using-a-photocell
        /// and http://www.emant.com/316002.page
        /// </summary>
        /// <param name="variableResistor"></param>
        /// <returns></returns>
        public static decimal ToLux(this decimal variableResistor)
        {
            const decimal luxRatio = 500000;
            return luxRatio / variableResistor;
        }
        #endregion
    }
}


