using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FANNCSharp.Double;
using System.Diagnostics;
using ClosedXML.Excel;

namespace AI_Prediction_and_classification
{
    /// <summary>
    /// This class calculates the price prediction. Taking the data from the first 6 months of the year
    ///  to predicting the remaining part of the year. Prediction for one day ahead, one week ahead and 3 months ahead.
    /// </summary>
    /// 

    class Prediction
    {
        private double[] testSet;
        private double[][] trainingInput = new double[175][];
        private double[][] trainingOutPut = new double[175][];
        private double[][] trainingInputWeek = new double[169][];
        private double[][] trainingOutPutWeek = new double[169][];
        private double[][] trainingInputMonth = new double[157][];
        private double[][] trainingOutPutMonth = new double[157][];
        private NeuralNet nn;
        private List<double> addPriceFuture = new List<double>();

        public Prediction()
        {
            //input, hidden layer, output
            nn = new NeuralNet(FANNCSharp.NetworkType.LAYER, new uint[] { 7, 5, 1 });
        }

        
        // calculating RMSE = Root mean squared error
        private double computeRMSE(List<Price> currentPrice, List<Price> predictedPrice)
        {
            double sum = 0;
            for (int i = 0; i < predictedPrice.Count; i++)
            {
                sum += Math.Sqrt(Math.Pow(predictedPrice[i].PriceData_ - currentPrice[i + 190].PriceData_, 2));
            }
            return (1.0 / currentPrice.Count) * sum;
        }


        // One day
        public List<Price> predictFuturePrice(List<Price> currentPrice)
        {
            double maxPrice = currentPrice.Max().PriceData_;
            List<Price> futurePrice = new List<Price>();
            // Half year minus 1 week
            for (int i = 0; i < 183 - 8; i++)
            {
                trainingInput[i] = new double[7];
                // One week
                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    trainingInput[i][j] = currentPrice[i + j].PriceData_ / maxPrice;
                }
                trainingOutPut[i] = new double[1];
                // Predicted value next week
                trainingOutPut[i][0] = currentPrice[i + 7].PriceData_ / maxPrice;
            }
            TrainingData tD = new TrainingData();
            tD.SetTrainData(trainingInput, trainingOutPut);

            // train, number of cycles, number of feedback, error rate
            nn.TrainOnData(tD, 10000, 1000, 0.00002f);
            string location = currentPrice.First().Location_;
            int year = currentPrice.First().Year_;
           // Debug.WriteLine("Price MSE for : " + location + " " + year + " " + nn.MSE);
            testSet = new double[7];
            // Next half year
            for (int i = 183; i < currentPrice.Count - 7; i++)
            {

                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    testSet[j] = currentPrice[i + j].PriceData_ / maxPrice;
                }
                double[] testSetPredictFuture = nn.Run(testSet);
                double predictedValue = testSetPredictFuture[0] * maxPrice;

                var predictedPrice = new Price(i.ToString(), location, predictedValue, year);
                addPriceFuture.Add(predictedValue);
                futurePrice.Add(predictedPrice);
            }
            var error = computeRMSE(currentPrice, futurePrice);
            Console.WriteLine("Price day RMSE for : " + location + " " + year + " " + error);
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sample Sheet");
            worksheet.Cell("A1").Value = futurePrice.ElementAt(0);
            workbook.SaveAs("HelloWorld.xlsx");
            Console.WriteLine(location,year);
            return futurePrice;                             
        }

        // One week
        public List<Price> predictionFuturePriceWeek(List<Price> priceWeek)
        {

            double maxPrice = priceWeek.Max().PriceData_;
            List<Price> futurePriceWeek = new List<Price>();
            // Half year minus 2 weeks
            for (int i = 0; i < 183 - 14; i++)
            {
                trainingInputWeek[i] = new double[7];
                // One week
                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    trainingInputWeek[i][j] = priceWeek[i + j].PriceData_ / maxPrice;
                }

                trainingOutPutWeek[i] = new double[1];
                // Predicted value next fortnight
                trainingOutPutWeek[i][0] = priceWeek[i + 14].PriceData_ / maxPrice;
            }
            TrainingData tD = new TrainingData();
            tD.SetTrainData(trainingInputWeek, trainingOutPutWeek);

            // train, number of cycles, number of feedback, error rate
            nn.TrainOnData(tD, 10000, 1000, 0.00002f);
            string location = priceWeek.First().Location_;
            int year = priceWeek.First().Year_;
            testSet = new double[7];

            // Next half year.
            for (int i = 183; i < priceWeek.Count - 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    testSet[j] = priceWeek[i + j].PriceData_ / maxPrice;
                }
                double[] testSetPredictFuture = nn.Run(testSet);
                double predictedValue = testSetPredictFuture[0] * maxPrice;
             
                var predictedPrice = new Price(i.ToString(), location, predictedValue, year);
                addPriceFuture.Add(predictedValue);
                futurePriceWeek.Add(predictedPrice);
            }
            var error = computeRMSE(priceWeek, futurePriceWeek);
            Console.WriteLine("Price week RMSE for : " + location + " " + year + " " + error);
            return futurePriceWeek;
        }
        
        // month
        public List<Price> predictionFuturePriceMonthly(List<Price> priceMonth)
        {
            double maxPrice = priceMonth.Max().PriceData_;
            List<Price> futurePriceMonth = new List<Price>();
            // Half year minus 4 weeks
            for (int i = 0; i < 183 - 26; i++)
            {

                trainingInputMonth[i] = new double[7];
                // One week
                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    trainingInputMonth[i][j] = priceMonth[i + j].PriceData_ / maxPrice;
                }

                trainingOutPutMonth[i] = new double[1];
                // Predicted value next fortnight
                trainingOutPutMonth[i][0] = priceMonth[i + 92].PriceData_ / maxPrice;
            }
            TrainingData tD = new TrainingData();
            tD.SetTrainData(trainingInputMonth, trainingOutPutMonth);

            // train, number of cycles, number of feedback, error rate
            nn.TrainOnData(tD, 10000, 1000, 0.00002f);
            string location = priceMonth.First().Location_;
            int year = priceMonth.First().Year_;
            testSet = new double[7];
           
            // Next half year.
            for (int i = priceMonth.Count -92; i < priceMonth.Count - 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    testSet[j] = priceMonth[i + j].PriceData_ / maxPrice;
                }
                double[] testSetPredictFuture = nn.Run(testSet);
                double predictedValue = testSetPredictFuture[0] * maxPrice;
              
                var predictedPrice = new Price(i.ToString(), location, predictedValue, year);
                addPriceFuture.Add(predictedValue);
                futurePriceMonth.Add(predictedPrice);
            }
            var error = computeRMSE(priceMonth, futurePriceMonth);
            Debug.WriteLine("Price month RMSE for : " + location + " " + year + " " + error);
            return futurePriceMonth;

        }
    }
}
