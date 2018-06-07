using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Prediction_and_classification
{
    /// <summary>
    /// This is the public class Weather. Taking the variables date, location, temperature and year
    /// </summary>
    public class Weather : IComparable<Weather>
    {

        public string DateWeather
        {
            get; private set;
        }
        public string LocationWeather
        {
            get; private set;
        }
        
        public double TemperatureWeather
        {
            get; private set;
        }
        public int YearWeather
        {
            get; private set;
        }
        public Weather(string date_, string location_, double temp_, int year_)
        {
            DateWeather = date_;
            LocationWeather = location_;
            TemperatureWeather = temp_;
            YearWeather = year_;

        }
        // Method for sorting highest and lowest temperature. 
        public int CompareTo(Weather cmp)
        {
            if (cmp.TemperatureWeather.CompareTo(TemperatureWeather) >= 1)
            {
                return -1;
            }
            else if (cmp.TemperatureWeather.CompareTo(TemperatureWeather) < 1)
            {
                return 1;
            }
            else
                return 0;
        }


    }
}
