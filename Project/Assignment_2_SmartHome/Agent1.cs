using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome {
    /// <summary>
    /// this class controls the heated floor in the bathroom and the boiler. and set the 
    /// minumu and maximum temperature based on the some/away signal recieved from agent 4
    /// </summary>
    public class Agent1 {
        private double slack_ = 1.49;
        private Random randy = new Random();
        private double setTemp_;
        private double tempOut_;

        private bool isBoilerOn_ = true;
        private bool hasBoilerDisconnected_ = false;

        private bool state_;
        Agent4 ag4 = new Agent4();

        public Agent1() {
            Slack = slack_;
        }
        //used to get signal from agent 4
        public bool stateIs_(bool state) {
            return state_ = state; ;
        }
        //gets and sets the load for heated floor
        public double LoadHeatedFloor {
            get; private set;
        }
        //gets and sets the load for boiler
        public double LoadBoiler {
            get; private set;
        }
        //sets the outdoor temp
        public void setOutdoorTemperature(double temp) {
            tempOut_ = temp;
        }
        //gets and sets the slack
        public double Slack {
            get; set;
        }
        //the consumption of the boiler either on or off
        public double consumptionBoiler() {
            LoadBoiler = (isBoilerOn_) ? ((hasBoilerDisconnected_) ? 2.0 : 0.5) : 0.5;
            hasBoilerDisconnected_ = (isBoilerOn_ && hasBoilerDisconnected_) ? false : hasBoilerDisconnected_;
            return LoadBoiler;
        }
        //the maximum consumption of the heated floor based on whether anyone is home or not
        public double consumptionHeatedFloorMax() {
            setTemp_ = (state_) ? 25 : 23;//max temp for home : away
            LoadHeatedFloor = (tempOut_ < setTemp_) ? 0.2 + 0.5 * (setTemp_ - tempOut_) : 0.2;
            return LoadHeatedFloor;
        }
        //the minimum consumption of the heated floor  based on whether anyone is home or not
        public double consumptionHeatedFloorMin() {
            setTemp_ = (state_) ? 23 : 20;//min temp for home : away
            LoadHeatedFloor = (tempOut_ < setTemp_) ? 0.2 + 0.5 * (setTemp_ - tempOut_) : 0.2;
            return LoadHeatedFloor;
        }
        //calculates the load of the boiler and the heated floor
        public double calculateTheLoad() {
            return LoadBoiler + LoadHeatedFloor;
        }
    }
}
