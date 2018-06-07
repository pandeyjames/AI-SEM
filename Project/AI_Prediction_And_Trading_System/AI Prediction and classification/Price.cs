using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AI_Prediction_and_classification
{
    /// <summary>
    /// This is the public class Price. Taking the variables date, location, pricedata and year
    /// </summary>
    public class Price : IComparable<Price>
    {

        public string Date_
        {
            get; private set;
        }
        public string Location_
        {
            get; private set;
        }
        public double PriceData_
        {
            get; private set;
        }
        public int Year_
        {
            get; private set;
        }
        public Price(string date_, string location_, double priceData_, int year_)
        {
            Date_ = date_;
            Location_ = location_;
            PriceData_ = priceData_;
            Year_ = year_;

        }

        // Method for sorting highest and lowest price. 
        public int CompareTo(Price cmp)
        {
            if (cmp.PriceData_.CompareTo(PriceData_) >= 1)
            {
                return -1;
            }
            else if (cmp.PriceData_.CompareTo(PriceData_) < 1)
            {
                return 1;
            }
            else
                return 0;
        }
    }
}
