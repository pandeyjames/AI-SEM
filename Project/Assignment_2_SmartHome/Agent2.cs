using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome {
    /// <summary>
    /// this class controls the central heater and sets the minumum and maximum also temperature is based on the home/away signal recieved from agent 4
    /// </summary>
    public class Agent2 {

        private double setTemp_;
        private double tempOut_;
        private double slack_ = 1.49;

        private bool state_;
        Agent4 ag4 = new Agent4();

        public Agent2() {
            Slack_ = slack_;
        }
        //used to get signal from agent 4
        public bool stateIs(bool state) {
            return state_ = state; ;
        }
        //gets and sets the slack
        public double Slack_ {
            get; set;
        }
        //gets and sets the load for central heater
        public double LoadCentralHeater_ {
            get; private set;
        }
        //sets the outdoor temp
        public void setOutdoorTemperature(double temp) {
            tempOut_ = temp;
        }
        //the maximum consumption of the central heater based on whether anyone is home or not
        public double consumptionCenteralHeaterMax() {
            setTemp_ = (state_) ? 23 : 21;//max temp for home : away
            LoadCentralHeater_ = (tempOut_ < setTemp_) ? 0.8 + 1.0 * (setTemp_ - tempOut_) : 0.8;
            return LoadCentralHeater_;
        }
        //the minimum consumption of the central heater based on whether anyone is home or not
        public double consumptionCenteralHeaterMin() {
            setTemp_ = (state_) ? 21 : 18;//max temp for home : away
            LoadCentralHeater_ = (tempOut_ < setTemp_) ? 0.8 + 1.0 * (setTemp_ - tempOut_) : 0.8;
            return LoadCentralHeater_;
        }
        //returns the calculated load of the central heater
        public double calculateTheLoad() {
            return LoadCentralHeater_;
        }
    }
}
