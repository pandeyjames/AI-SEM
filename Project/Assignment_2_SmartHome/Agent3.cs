using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome {
    /// <summary>
    /// controls the battery, this class charges or discharges the battery
    /// the battery level can never go below 20% of its 7kW capacity.
    /// </summary>
    public class Agent3 {

        private double batteryLevel_ = 7.0;
        private bool state_;
        Agent4 ag4 = new Agent4();
        public bool stateIs(bool state) {
            return state_ = state;
        }
        //returns the battery level
        public double batteryLevel() {
            return batteryLevel_;
        }
        //charges the battery if the battery level is lower than 7.0
        public double charging() {
            if (batteryLevel_ < 7.0)
                batteryLevel_ += 1.0;

            return batteryLevel_;
        }
        //discharges the battery aslong as the battery level is greater than 2.0
        public double discharging() {
            if (batteryLevel_ > 2.0) {
                batteryLevel_ -= 1.0;
            }
            return batteryLevel_;
        }
    }
}
