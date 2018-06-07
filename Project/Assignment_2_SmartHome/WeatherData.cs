using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome {
    /// <summary>
    /// we can get the weather data from this class
    /// </summary>
    public class WeatherData {
        public WeatherData(int hour_, int day_, /*int cloud_,*/ double temperature_) {
            HourData_ = hour_;
            DayData_ = day_;
            //CloudData_ = cloud_;
            TemperatureData_ = temperature_;
        }
        public int HourData_ {
            get; private set;
        }
        public int DayData_ {
            get; private set;
        }
        public double CloudData_ {
            get; private set;
        }
        public double TemperatureData_ {
            get; private set;
        }
    }
}
