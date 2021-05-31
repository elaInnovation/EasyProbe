using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EasyProbe.Tools
{
    public static class Converter
    {
        public static float CelciusToFaren(string celcius)
        {
            float value = float.Parse(celcius, CultureInfo.InvariantCulture.NumberFormat);
            return (value*9/5)+32;
        }

        public static float StringToCelcius(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
