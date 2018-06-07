using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace AI_Prediction_and_classification
{
    /// <summary>
    /// This class has the main components to draw in the window. Telling what each button or dropdown box should do.
    /// </summary>
    public partial class TimeseriesGraph : Form
    {
        private List<Price> priceList_;
        private List<Weather> weatherList_;
       

        public void writeToExcel(List<Price> plist_)
        {
            _Application excel = new _Excel.Application();
            Workbook wb;
            Worksheet ws;
            wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            //wb.SaveAs(@"testc");
            

        }
        public TimeseriesGraph()
        {
            InitializeComponent();
            LoadXLFiles load = new LoadXLFiles();
            this.gridGraph1.setParent(this);
            priceList_ = load.loadExcelFiles();
            weatherList_ = load.loadWeather();
            this.gridGraph1.setPrice(priceList_);

            double highestValue = priceList_.Max().PriceData_;
            double lowestValue = priceList_.Min().PriceData_;
            double highestValueWeather = weatherList_.Max().TemperatureWeather;
            double lowestValueWeather = weatherList_.Min().TemperatureWeather;

            highest.Text = highestValue.ToString();
            lowest.Text = lowestValue.ToString();
            secondHighest.Text = (highestValue * 2 / 3).ToString();
            middle.Text = (highestValue / 2).ToString();
            secondLowest.Text = (highestValue * 1 / 3).ToString();

            highest.Text = highestValueWeather.ToString();
            lowest.Text = lowestValueWeather.ToString();
            secondHighest.Text = (highestValueWeather * 2 / 3).ToString();
            middle.Text = (highestValueWeather / 2).ToString();
            secondLowest.Text = (highestValueWeather * 1 / 3).ToString();

            this.gridGraph1.setLocationAndSheet(Location.Text, (int)Year.Value);

        }
    
        // Actions when clicking on Price button
        private void Price_Click_1(object sender, EventArgs e)
        {

            Location.Visible = true;
            Year.Visible = true;
            gridGraph1.Visible = true;
            January.Visible = true;
            February.Visible = true;
            March.Visible = true;
            April.Visible = true;
            May.Visible = true;
            June.Visible = true;
            July.Visible = true;
            August.Visible = true;
            September.Visible = true;
            October.Visible = true;
            November.Visible = true;
            December.Visible = true;
        }
         // THe components in the window has the same size, if we resize it
        private void TimeseriesGraph_Resize(object sender, EventArgs e)
        {
            gridGraph1.Invalidate();
        }
        // For price location
        private void Location_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            this.gridGraph1.setLocationAndSheet(Location.Text, (int)Year.Value);

        }
        // For price year
        private void Year_ValueChanged(object sender, EventArgs e)
        {
            this.gridGraph1.setLocationAndSheet(Location.Text, (int)Year.Value);
        }

        private void Weather_Click_1(object sender, EventArgs e)
        {
            LocationWeather.Visible = true;
            YearWeather.Visible = true;
            gridGraph1.Visible = true;
            January.Visible = true;
            February.Visible = true;
            March.Visible = true;
            April.Visible = true;
            May.Visible = true;
            June.Visible = true;
            July.Visible = true;
            August.Visible = true;
            September.Visible = true;
            October.Visible = true;
            November.Visible = true;
            December.Visible = true;

            if (this.gridGraph1.IsWeather_)
            {
                weatherButton.Text = "Display weather";
            }
            else
            {
                weatherButton.Text = "Hide weather";
                // Predict the one day ahead
                PredictWeather test = new PredictWeather();
                List<Price> futureList = test.predictFutureWeather(weatherList_.Where(p => p.LocationWeather == Location.Text && p.YearWeather == (int)YearWeather.Value).ToList(), (priceList_.Where(p => p.Location_ == Location.Text && p.Year_ == (int)Year.Value).ToList()));
                this.gridGraph1.setWeather(futureList);
                this.gridGraph1.setLocationAndSheetWeather(LocationWeather.Text, (int)YearWeather.Value);

                 // Predict the 3 months ahead
                PredictWeather month = new PredictWeather();
                List<Price> futureMonthList = month.predictionFutureWeatherMonthly(weatherList_.Where(p => p.LocationWeather == Location.Text && p.YearWeather == (int)YearWeather.Value).ToList(), (priceList_.Where(p => p.Location_ == Location.Text && p.Year_ == (int)Year.Value).ToList()));
                this.gridGraph1.setWeatherMonth(futureMonthList);
                this.gridGraph1.setLocationAndSheetWeatherMonth(LocationWeather.Text, (int)YearWeather.Value);
            }
            this.gridGraph1.IsWeather_ = !this.gridGraph1.IsWeather_;
            gridGraph1.Invalidate();
        }
        // For weather location
        private void LocationWeather_SelectedIndexChanged_1(object sender, EventArgs e)
        { 
            this.gridGraph1.setLocationAndSheetWeather(LocationWeather.Text, (int)YearWeather.Value);
            gridGraph1.Invalidate();
        }
        // For weather year
        private void YearWeather_ValueChanged_1(object sender, EventArgs e)
        {
            this.gridGraph1.setLocationAndSheetWeather(LocationWeather.Text, (int)YearWeather.Value);
            gridGraph1.Invalidate();
        }
         // Actions when clicking on the Ensemble button
        private void ensambleButton_Click(object sender, EventArgs e)
        {
            if (this.gridGraph1.IsEnsemble_)
            {
                ensambleButton.Text = "Display ensemble";

            }
            else
            {
                ensambleButton.Text = "Hide ensemble";
                // Predict one day ahead
                this.gridGraph1.setLocationAndSheet(Location.Text, (int)Year.Value);
                Ensemble ensemble = new Ensemble();
                List<Price> futureList = ensemble.predictFuturePrice(priceList_.Where(p => p.Location_ == Location.Text && p.Year_ == (int)Year.Value).ToList());
                this.gridGraph1.setEnsemble(futureList);

                // Predict one week ahead
                Ensemble ensembleWeek = new Ensemble();
                List<Price> futureWeekList = ensembleWeek.predictionFuturePriceWeek(priceList_.Where(p => p.Location_ == Location.Text && p.Year_ == (int)Year.Value).ToList());
                this.gridGraph1.setFuturePriceWeek(futureWeekList);
            }
            this.gridGraph1.IsEnsemble_ = !this.gridGraph1.IsEnsemble_;
            gridGraph1.Invalidate();
        }


        // Actions when clicking on the Ensemble button
        private void tradeButton_Click(object sender, EventArgs e)
        {

            Form f1 = new Trade();
            f1.Show();
            writeToExcel(priceList_);
            //if (this.gridGraph1.IsEnsemble_)
            //{
            //    ensambleButton.Text = "Display ensemble";

            //}
            //else
            //{
            //    ensambleButton.Text = "Hide ensemble";
            //    // Predict one day ahead
            //    this.gridGraph1.setLocationAndSheet(Location.Text, (int)Year.Value);
            //    Ensemble ensemble = new Ensemble();
            //    List<Price> futureList = ensemble.predictFuturePrice(priceList_.Where(p => p.Location == Location.Text && p.Year == (int)Year.Value).ToList());
            //    this.gridGraph1.setEnsemble(futureList);

            //    // Predict one week ahead
            //    Ensemble ensembleWeek = new Ensemble();
            //    List<Price> futureWeekList = ensembleWeek.predictionFuturePriceWeek(priceList_.Where(p => p.Location == Location.Text && p.Year == (int)Year.Value).ToList());
            //    this.gridGraph1.setFuturePriceWeek(futureWeekList);
            //}
            //this.gridGraph1.IsEnsemble_ = !this.gridGraph1.IsEnsemble_;
            //gridGraph1.Invalidate();
        }

        // Actions when clicking on the Show prediction button
        private void predictionButton_Click(object sender, EventArgs e)
        {
            if (this.gridGraph1.IsPredictedPrice_)
            {
                predictionButton.Text = "Display prediction";
            }
            else
            {
                this.gridGraph1.setLocationAndSheet(Location.Text, (int)Year.Value);
                predictionButton.Text = "Hide prediction";
                // Prediction one day ahead
                Prediction pred = new Prediction();
                List<Price> futureList = pred.predictFuturePrice(priceList_.Where(p => p.Location_ == Location.Text && p.Year_ == (int)Year.Value).ToList());
                Console.WriteLine(" ", futureList.ElementAt(0).ToString()," ", futureList.ElementAt(2).ToString()," ", futureList.ElementAt(3).ToString());
                this.gridGraph1.setFuturePrice(futureList);
                // Prediction one week ahead
                Prediction predWeek = new Prediction();
                List<Price> futureWeekList = predWeek.predictionFuturePriceWeek(priceList_.Where(p => p.Location_ == Location.Text && p.Year_ == (int)Year.Value).ToList());
                this.gridGraph1.setFuturePriceWeek(futureWeekList);
                // Prediction 3 months ahead
                Prediction predMonth = new Prediction();
                List<Price> futureMonthList = predMonth.predictionFuturePriceMonthly(priceList_.Where(p => p.Location_ == Location.Text && p.Year_ == (int)Year.Value).ToList());
                this.gridGraph1.setFuturePriceMonth(futureMonthList);
            }
            this.gridGraph1.IsPredictedPrice_ = !this.gridGraph1.IsPredictedPrice_;
            gridGraph1.Invalidate();
        }
    }
}
