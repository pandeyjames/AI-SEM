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
    /// This class creates the rectangles on the TimeseriesGraph window
    /// </summary>
    public partial class ColoredBox : Panel
    {
        public ColoredBox()
        {
            InitializeComponent();
          
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            DrawBoxRed(g);
            DrawBoxGreen(g);
            DrawBoxBlue(g);
            DrawOrangeBox(g);
            DrawBlackBox(g);
            DrawGrayBox(g);
            DrawBlackPenBox(g);
            DrawGreenPenBox(g);
        }
        //  Create the red rectangle
        private void DrawBoxRed(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Red);
            g.FillRectangle( brush, 5, 5, 15, 15);
           
        }
        // Create the green rectangle
        private void DrawBoxGreen(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Green);
            g.FillRectangle(brush, 5, 25, 15, 15);

        }
        // Create the blue rectangle
        private void DrawBoxBlue(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Blue);
            g.FillRectangle(brush, 5, 45, 15, 15);

        }
        // Create the green rectangle
        private void DrawOrangeBox(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Orange);
            g.FillRectangle(brush, 5, 65, 15, 15);
        }
        // Create the black rectangle
        private void DrawBlackBox(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Black);
            g.FillRectangle(brush, 5, 85, 15, 15);
        }
        // Create the gray rectangle
        private void DrawGrayBox(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Gray);
            g.FillRectangle(brush, 5, 105, 15, 15);
        }
        // Create the black dotted rectangle
        private void DrawBlackPenBox(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1);
            pen.DashStyle = DashStyle.DashDot;
            g.DrawRectangle(pen, 5, 125, 15, 15);
        }
        // Create the green dotted rectangle
        private void DrawGreenPenBox(Graphics g)
        {
            Pen pen = new Pen(Color.Green, 1);
            pen.DashStyle = DashStyle.DashDot;
            g.DrawRectangle(pen, 5, 145, 15, 15);
        }

    }
}
