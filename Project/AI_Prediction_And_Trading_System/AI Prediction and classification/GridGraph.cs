using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AI_Prediction_and_classification
{
    /// <summary>
    ///  This class draws all the graphs, the grid and sets the location and year. 
    /// </summary>
    public partial class GridGraph : Panel
    {
        private bool isEnsemble_ = false;
        private bool isPredictedPrice_ = false;
        private bool isWeather_ = false;
        private static int MAX_ROWS = 30;
        private static int MAX_COLS = 30;
        private TimeseriesGraph parent_;
        private List<Price> priceList_;
        private List<Price> locationList_;
        private List<Price> predictionFutureList_;
        private List<Price> predictionFutureLocationList_;
        private List<Price> ensembleList_;
        private List<Price> predictionFutureWeekList_;
        private List<Price> predictionFutureLocationWeekList_;
        private List<Price> predictionFutureMonthList_;
        private List<Price> weatherList_;
        private List<Price> locationListWeather_;
        private List<Price> weatherListMonth_;
        private List<Price> locationListWeatherMonth_;

        public bool IsEnsemble_
        {
            get
            {
                return isEnsemble_;
            }

            set
            {
                isEnsemble_ = value;
            }
        }
        public bool IsPredictedPrice_
        {
            get
            {
                return isPredictedPrice_;
            }

            set
            {
                isPredictedPrice_ = value;
            }
        }
        public bool IsWeather_
        {
            get
            {
                return isWeather_;
            }

            set
            {
                isWeather_ = value;
            }
        }

        public GridGraph()
        {
            InitializeComponent();
            locationListWeather_ = new List<Price>();
        }
        public void setParent(TimeseriesGraph parent)
        {
            parent_ = parent;
        }
        public void setPrice(List<Price> priceList)
        {
            priceList_ = priceList ;
        }
        public void setWeather(List<Price> weatherList)
        {
            weatherList_ = weatherList;
        }
        public void setWeatherMonth(List<Price> weatherListMonth)
        {
            weatherListMonth_ = weatherListMonth;
        }
        public void setFuturePrice(List<Price> predictionFutureList)
        {
            predictionFutureList_ = predictionFutureList;
        }
        public void setFuturePriceWeek(List<Price> predictionFutureWeekList)
        {
            predictionFutureWeekList_ = predictionFutureWeekList;
        }
        public void setFuturePriceMonth(List<Price> predictionFutureMonthList)
        {
            predictionFutureMonthList_ = predictionFutureMonthList;
        }
        public void setEnsemble(List<Price> ensemble)
        {
            ensembleList_ = ensemble;
        }
        public void setLocationAndSheet(string location, int sheet)
        {
            locationList_ = priceList_.Where(p => p.Location_ == location && p.Year_ == sheet).ToList();
            this.Invalidate();
        }
        public void setLocationAndSheetWeather(string location, int sheet)
        {
            locationListWeather_ = weatherList_.Where(p => p.Location_ == location && p.Year_ == sheet).ToList();
            this.Invalidate();     
        }
        public void setLocationAndSheetWeatherMonth(string location, int sheet)
        {
            locationListWeatherMonth_ = weatherListMonth_.Where(p => p.Location_ == location && p.Year_ == sheet).ToList();
            this.Invalidate();
        }
        public void setLocationAndSheetPrediction(string location, int sheet)
        {
            predictionFutureLocationList_ = predictionFutureList_.Where(p => p.Location_ == location && p.Year_ == sheet).ToList();
            this.Invalidate();
        }
        public void setLocationAndSheetPredictionWeek(string location, int sheet)
        {
            predictionFutureLocationWeekList_ = predictionFutureWeekList_.Where(p => p.Location_ == location && p.Year_ == sheet).ToList();
            this.Invalidate();
        }
        public void setLocationAndSheetPredictionMonth(string location, int sheet)
        {
            predictionFutureMonthList_ = predictionFutureMonthList_.Where(p => p.Location_ == location && p.Year_ == sheet).ToList();
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            this.DrawGrid(g);
            this.DrawGraph(g);
            if (isWeather_)
            {
                this.DrawGraphWeather(g);
                this.DrawPredictedWeatherMonthkGraph(g);
            }
            if (locationList_ != null && predictionFutureList_ != null)
            {
                if (isEnsemble_)
                {
                    this.DrawEnsemble(g);
                }
            }
            if (locationList_ != null && predictionFutureList_ != null)
            {
                if (isEnsemble_)
                {
                    this.DrawPredictedWeekEnsembleGraph(g);
                }
            }
            if (locationList_ != null && predictionFutureList_ != null)
            {
                if (isPredictedPrice_)
                {
                    this.DrawPredictedGraph(g);
                }   
            }
            if (locationList_ != null && predictionFutureWeekList_ != null)
            {
                if (isPredictedPrice_)
                {
                    this.DrawPredictedWeekGraph(g);
                }
                
            }
            if (locationList_ != null && predictionFutureMonthList_ != null)
            {
                if (isPredictedPrice_)
                {
                    this.DrawPredictedMonthkGraph(g);
                }
            }
        }
        //  Draw the red graph (original price graph)
        private void DrawGraph(Graphics g)
        {
            if (locationList_ != null)
            {
                 if(locationList_.Count > 0)
                {
                    Pen pen = new Pen(Color.Red, 1);
                    Point[] point = new Point[locationList_.Count];
                    GraphicsPath gPath = new GraphicsPath();
                    gPath.StartFigure();

                    for (int i = 0; i < locationList_.Count; i++)
                    {
                        // Flip the graph, so it goes in the right direction. Or else it will be up-side down. 
                        double y = ((1.0 / (double)locationList_[i].PriceData_) / (1.0 / locationList_.Min().PriceData_)) * this.Height;
                        double x = ((double)i / 365) * this.Width;
                        point[i] = new Point((int)x, (int)y);
                    }
                    gPath.AddLines(point);
                    g.DrawPath(pen, gPath);
                }
            }
        }
        // Draw the orange graph (weather)
        private void DrawGraphWeather(Graphics g)
        {
            if (locationListWeather_ != null)
            {
                if (locationListWeather_.Count > 0)
                {
                    Pen pen = new Pen(Color.Orange, 1);
                    Point[] point = new Point[locationListWeather_.Count];
                    GraphicsPath gPath = new GraphicsPath();
                    gPath.StartFigure();
                        
                    for (int i = 0; i < locationListWeather_.Count; i++)
                    {
                        // Flip the graph, so it goes in the right direction. Or else it will be up-side down. 
                        double y = ((1.0 / (double)locationListWeather_[i].PriceData_) / (1.0 / locationList_.Min().PriceData_)) * this.Height;
                        double x = (((double)i + 192.0) / 365) * this.Width;
                        point[i] = new Point((int)x, (int)y);
                      
                    }
                    gPath.AddLines(point);
                    g.DrawPath(pen, gPath);
                }
            }
        }
        // Draw the green graph (price)
        private void DrawPredictedGraph(Graphics g)
        {
            if (predictionFutureList_ != null)
            {
                if (predictionFutureList_.Count > 0)
                {
                    if (locationList_.Count > 0)
                    {
                        Pen pen = new Pen(Color.Green, 1);
                        Point[] point = new Point[predictionFutureList_.Count];
                        GraphicsPath gPath = new GraphicsPath();
                        gPath.StartFigure();
                        for (int i = 0; i < predictionFutureList_.Count; i++)
                        {
                            // Flip the graph, so it goes in the right direction. Or else it will be up-side down. 
                            double y = ((1.0 / (double)predictionFutureList_[i].PriceData_) / (1.0 / locationList_.Min().PriceData_)) * this.Height;
                            double x = (((double)i + 192.0) / 365) * this.Width;
                            point[i] = new Point((int)x, (int)y);
                        }
                        gPath.AddLines(point);
                        g.DrawPath(pen, gPath);
                    }
                }
            }
        }
        // Draw the black graph (ensemble)
        private void DrawEnsemble(Graphics g)
        {
            if (ensembleList_ != null)
            {
                if (ensembleList_.Count > 0)
                {
                    if (locationList_.Count > 0)
                    {
                        Pen pen = new Pen(Color.Black, 1);
                        Point[] point = new Point[ensembleList_.Count];
                        GraphicsPath gPath = new GraphicsPath();
                        gPath.StartFigure();
  
                        for (int i = 0; i < ensembleList_.Count; i++)
                        {
                            // Flip the graph, so it goes in the right direction. Or else it will be up-side down. 
                            double y = ((1.0 / (double)ensembleList_[i].PriceData_) / (1.0 / locationList_.Min().PriceData_)) * this.Height;
                            double x = (((double)i + 192.0) / 365) * this.Width;
                            point[i] = new Point((int)x, (int)y);
                        }
                        gPath.AddLines(point);
                        g.DrawPath(pen, gPath);
                    }
                }
            }
        }
        // Draw the blue graph (price)
        private void DrawPredictedWeekGraph(Graphics g)
        {
            if (predictionFutureWeekList_ != null)
            {
                if (predictionFutureWeekList_.Count > 0)
                {
                    if (locationList_.Count > 0)
                    {
                        Pen pen = new Pen(Color.Blue, 1);
                        Point[] point = new Point[predictionFutureWeekList_.Count];
                        GraphicsPath gPath = new GraphicsPath();
                        gPath.StartFigure();

                        for (int i = 0; i < predictionFutureWeekList_.Count; i++)
                        {
                            // Flip the graph, so it goes in the right direction. Or else it will be up-side down. 
                            double y = ((1.0 / (double)predictionFutureWeekList_[i].PriceData_) / (1.0 / locationList_.Min().PriceData_)) * this.Height;
                            double x = (((double)i + 192.0) / 365) * this.Width;
                            point[i] = new Point((int)x, (int)y);
                        }
                        gPath.AddLines(point);
                        g.DrawPath(pen, gPath);
                    }
                }
            }
        }
        // Draw the dotted black graph (price)
        private void DrawPredictedMonthkGraph(Graphics g)
        {
            if (predictionFutureMonthList_ != null)
            {
                if (predictionFutureMonthList_.Count > 0)
                {
                    if (locationList_.Count > 0)
                    {
                        Pen pen = new Pen(Color.Black, 1);
                        pen.DashStyle = DashStyle.DashDot;
                        Point[] point = new Point[predictionFutureMonthList_.Count];
                        GraphicsPath gPath = new GraphicsPath();
                        gPath.StartFigure();

                        int today = DateTime.Now.DayOfYear;
                        for (int i = 0; i < predictionFutureMonthList_.Count; i++)
                        {
                            // Flip the graph, so it goes in the right direction. Or else it will be up-side down. 
                            double y = ((1.0 / (double)predictionFutureMonthList_[i].PriceData_) / (1.0 / locationList_.Min().PriceData_)) * this.Height;
                            double x = (((double)i + today) / 365) * this.Width;
                            point[i] = new Point((int)x, (int)y);
                        }
                        gPath.AddLines(point);
                        g.DrawPath(pen, gPath);
                    }
                }
            }
        }
        // Draw the green dotted graph (weather)
        private void DrawPredictedWeatherMonthkGraph(Graphics g)
        {
            if (weatherListMonth_ != null)
            {
                if (weatherListMonth_.Count > 0)
                {
                    if (locationList_.Count > 0)
                    {
                        Pen pen = new Pen(Color.Green, 1);
                        pen.DashStyle = DashStyle.DashDot;
                        Point[] point = new Point[weatherListMonth_.Count];
                        GraphicsPath gPath = new GraphicsPath();
                        gPath.StartFigure();

                        int today = DateTime.Now.DayOfYear;
                        for (int i = 0; i < weatherListMonth_.Count; i++)
                        {
                            // Flip the graph, so it goes in the right direction. Or else it will be up-side down. 
                            double y = ((1.0 / (double)weatherListMonth_[i].PriceData_) / (1.0 / locationList_.Min().PriceData_)) * this.Height;
                            double x = (((double)i + today) / 365) * this.Width;
                            point[i] = new Point((int)x, (int)y);
                        }
                        gPath.AddLines(point);
                        g.DrawPath(pen, gPath);
                    }
                }
            }
        }
        // Draw the gray graph (ensemble)
        private void DrawPredictedWeekEnsembleGraph(Graphics g)
        {
            if (predictionFutureWeekList_ != null)
            {
                if (predictionFutureWeekList_.Count > 0)
                {
                    if (locationList_.Count > 0)
                    {
                        Pen pen = new Pen(Color.Gray, 1);
                        Point[] point = new Point[predictionFutureWeekList_.Count];
                        GraphicsPath gPath = new GraphicsPath();
                        gPath.StartFigure();

                        for (int i = 0; i < predictionFutureWeekList_.Count; i++)
                        {
                            // Flip the graph, so it goes in the right direction. Or else it will be up-side down. 
                            double y = ((1.0 / (double)predictionFutureWeekList_[i].PriceData_) / (1.0 / locationList_.Min().PriceData_)) * this.Height;
                            double x = (((double)i + 190.0) / 365) * this.Width;
                            point[i] = new Point((int)x, (int)y);
                        }
                        gPath.AddLines(point);
                        g.DrawPath(pen, gPath);
                    }
                }
            }
        }
        // Draw the dotted black grid
        private void DrawGrid(Graphics g)
        {
            int width = this.Width;
            int height = this.Height;

            Pen pen = new Pen(Color.Black, 1);
            pen.DashStyle = DashStyle.DashDot;

            // Draw the grid.
            for (int i = 0; i < MAX_COLS; i++)
            {
                double x = ((double)i / MAX_COLS) * this.Height;
                g.DrawLine(pen, new Point(0,(int) x ), new Point(width, (int) x));

            }
            for (int j = 0; j < MAX_ROWS; j++)
            {
                double x = ((double)j / MAX_ROWS) * this.Width;
                g.DrawLine(pen, new Point((int)x, 0), new Point((int)x, height));
            }
        }
    }
}
