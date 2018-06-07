using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome {
    /// <summary>
    ///  security of the house is controlled here with true/false as signal 
    /// </summary>
    public class Agent4 {

        private Random rand_;
        private bool state_;

        private double[] probabilityValues = {0.92,  0.891, 0.859, 0.823, 0.784, 0.741,
                                              0.696, 0.649, 0.6,   0.5,   0.55,  0.599,
                                              0.646, 0.691, 0.734, 0.773, 0.809, 0.841,
                                              0.87,  0.894, 0.92,  0.9,   0.9,   0.96 };
        public Agent4() {
            rand_ = new Random();
        }
        public bool stateIs(int hour) {
            double rand = rand_.NextDouble();
            state_ = (rand < probabilityValues[hour]) ? true : false;
            return state_;
        }
    }
}
