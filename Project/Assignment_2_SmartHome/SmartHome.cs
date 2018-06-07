using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SmartHome {
    /// <summary>
    /// Start SmartHome with this class which initialized everything.
    /// </summary>
    public partial class SmartHome : Form {
        Agent1 ag1_;
        Agent2 ag2_;
        Agent3 ag3_;
        Agent4 ag4_;
        Agent5 ag5_;

        private int simSpeed_ = 5000; // takes five seconds before the labels are updated.
        private List<WeatherData> weatherList;
        LoadExcel loadFile;

        public SmartHome() {
            InitializeComponent();
            loadFile = new LoadExcel();
            weatherList = new List<WeatherData>();
            weatherList = loadFile.loadExcel();
            ag1_ = new Agent1();
            ag2_ = new Agent2();
            ag3_ = new Agent3();
            ag4_ = new Agent4();
            ag5_ = new Agent5(this);
        }

        public Agent1 getAgent1() {
            return ag1_;
        }
        public Agent2 getAgent2() {
            return ag2_;
        }
        public Agent3 getAgent3() {
            return ag3_;
        }
        private double getDayAndHour(int day, int hour) {
            return weatherList.FirstOrDefault(temp => temp.DayData_ == day && temp.HourData_ == hour).TemperatureData_;
        }
        private void buttonStartSmartHome_Click(object sender, EventArgs e) {
            Thread Threadiness = new Thread(runner);
            Threadiness.IsBackground = true;
            Threadiness.Start();
        }
       

        //to contiuously update the label deligate is used
        private void agentsLabels(int i, int j, bool state) {
            LabelDayAgent1.Invoke((MethodInvoker)(() => LabelDayAgent1.Text = "Day: " + i));
            LabelHourAgent1.Invoke((MethodInvoker)(() => LabelHourAgent1.Text = "Hour: " + j));
            LabelStateAgent1.Invoke((MethodInvoker)(() => LabelStateAgent1.Text = "State: " + currentStateIs(state)));
            LabelConsumptionHeatedFloorAgent1.Invoke((MethodInvoker)(() => LabelConsumptionHeatedFloorAgent1.Text = "Consumption heated floor: " + ag1_.consumptionHeatedFloorMax() + " kW/h"));
            LabelConsumptionBoilerAgent1.Invoke((MethodInvoker)(() => LabelConsumptionBoilerAgent1.Text = "Consumption boiler: " + ag1_.consumptionBoiler() + " kW/h"));

            LabelDayAgent2.Invoke((MethodInvoker)(() => LabelDayAgent2.Text = "Day: " + i));
            LabelHourAgent2.Invoke((MethodInvoker)(() => LabelHourAgent2.Text = "Hour: " + j));
            LabelStateAgent2.Invoke((MethodInvoker)(() => LabelStateAgent2.Text = "State: " + currentStateIs(state)));
            LabelConsumptionCentralHeatingAgent2.Invoke((MethodInvoker)(() => LabelConsumptionCentralHeatingAgent2.Text = "Consumption central heating: " + ag2_.consumptionCenteralHeaterMax() + " kW/h"));

            LabelDayAgent3.Invoke((MethodInvoker)(() => LabelDayAgent3.Text = "Day: " + i));
            LabelHourAgent3.Invoke((MethodInvoker)(() => LabelHourAgent3.Text = "Hour: " + j));
            LabelStateAgent3.Invoke((MethodInvoker)(() => LabelStateAgent3.Text = "State: " + currentStateIs(state)));
            LabelBatteryLevelAgent3.Invoke((MethodInvoker)(() => LabelBatteryLevelAgent3.Text = "Battery level: " + ag3_.batteryLevel() + " kW"));

            LabelDayAgent4.Invoke((MethodInvoker)(() => LabelDayAgent4.Text = "Day: " + i));
            LabelHourAgent4.Invoke((MethodInvoker)(() => LabelHourAgent4.Text = "Hour: " + j));
            LabelStateAgent4.Invoke((MethodInvoker)(() => LabelStateAgent4.Text = "State: " + currentStateIs(state)));

            LabelDayAgent5.Invoke((MethodInvoker)(() => LabelDayAgent5.Text = "Day: " + i));
            LabelHourAgent5.Invoke((MethodInvoker)(() => LabelHourAgent5.Text = "Hour: " + j));
            LabelStateAgent5.Invoke((MethodInvoker)(() => LabelStateAgent5.Text = "State: " + currentStateIs(state)));
            LabelSolarPanelAgent5.Invoke((MethodInvoker)(() => LabelSolarPanelAgent5.Text = "Solar Panel: " + ag5_.solarPanel(j) + " kW/h"));
            LabelActionAgent5.Invoke((MethodInvoker)(() => LabelActionAgent5.Text = "Action cost: " + ag5_.getBuyNOK(j) + " credit"));
        }
        //
        private string currentStateIs(bool state) {
            if (state) {
                return "Home";
            }
            else
                return "Away";
        }

        private void SimulationSpeedUpDown_ValueChanged(object sender, EventArgs e) {
            simSpeed_ = (int)SimulationSpeedUpDown.Value - (int)SimulationSpeedUpDown.Increment;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        //the program runs for every hour for every day with this thread
        private void runner()
        {
            for (int day = 1; day < 101; day++)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    bool state = ag4_.stateIs(hour);
                    ag1_.stateIs_(state);
                    ag1_.setOutdoorTemperature(getDayAndHour(day, hour));
                    ag2_.stateIs(state);
                    ag2_.setOutdoorTemperature(getDayAndHour(day, hour));
                    ag3_.stateIs(state);
                    ag5_.stateIs(state);
                    agentsLabels(day, hour, state);
                    ag5_.setHour(hour);
                    ag5_.fillingQMatrix(hour);
                    Thread.Sleep(simSpeed_);
                }
            }
            ag5_.printQMatrix();
        }
    }
}
