using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FANNCSharp.Double;
using System.Diagnostics;

namespace AI_Prediction_and_classification
{
    /// <summary>
    ///  This class calculates the weather prediction. Taking the data from the first 6 months of the year
    ///   to predicting the remaining part of the year. Prediction for one day ahead and 3 months ahead.
    /// </summary>
    class PredictWeather
    {
        private double[] testSet;
        private double[][] trainingInput = new double[175][];
        private double[][] trainingOutPut = new double[175][];
        private double[][] trainingInputMonth = new double[157][];
        private double[][] trainingOutPutMonth = new double[157][];
        private NeuralNet nn;
        private List<double> addPriceFuture = new List<double>();
        public PredictWeather()
        {
            //input, hidden layer, output
            nn = new NeuralNet(FANNCSharp.NetworkType.LAYER, new uint[] { 8, 5, 1 });
            nn.ActivationFunctionHidden = FANNCSharp.ActivationFunction.SIGMOID_SYMMETRIC;
            nn.ActivationFunctionOutput = FANNCSharp.ActivationFunction.SIGMOID_SYMMETRIC;
        }
        // RMSE = Root mean squared error
        private double computeRMSE(List<Price> currentPrice, List<Price> predictedPrice)
        {
            double sum = 0;

            for (int i = 0; i < predictedPrice.Count; i++)
            {
                sum += Math.Sqrt(Math.Pow(predictedPrice[i].PriceData_ - currentPrice[i + 190].PriceData_, 2));
            }
            return (1.0 / currentPrice.Count) * sum;
        }

        //predict future weather
        public List<Price> predictFutureWeather(List<Weather> currentWeather, List<Price> currentPrice)
        {
            double maxPrice = currentPrice.Max().PriceData_;
            double maxWeather = currentWeather.Max().TemperatureWeather;
            List<Price> futurePrice = new List<Price>();
            // Half year minus 1 week
            for (int i = 0; i < 183 - 8; i++)
            {

                trainingInput[i] = new double[8];
                // One week
                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    trainingInput[i][j] = currentPrice[i + j].PriceData_ / maxPrice;
                }
                // Intervention indicator
                trainingInput[i][7] = currentWeather[i + 7].TemperatureWeather/ maxWeather;
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
 
            testSet = new double[8];
            // Next half year
            for (int i = 183; i < currentPrice.Count - 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    testSet[j] = currentPrice[i + j].PriceData_ / maxPrice;
                }
                testSet[7] = currentWeather[i + 7].TemperatureWeather / maxWeather;
                double[] testSetPredictFuture = nn.Run(testSet);
                double predictedValue = testSetPredictFuture[0] * maxPrice;
           
                var predictedPrice = new Price(i.ToString(), location, predictedValue, year);
                addPriceFuture.Add(predictedValue);
                futurePrice.Add(predictedPrice);
            }
            var error = computeRMSE(currentPrice, futurePrice);
            Debug.WriteLine("Weather RMSE for : " + location + " " + year + " " + error);
            return futurePrice;
        }


        //Predict future weather by month
        public List<Price> predictionFutureWeatherMonthly(List<Weather> currentWeather, List<Price> currentPrice)
        {
            double maxPrice = currentPrice.Max().PriceData_;
            double maxWeather = currentWeather.Max().TemperatureWeather;
            List<Price> futurePriceMonth = new List<Price>();
            // Half year minus 4 weeks
            for (int i = 0; i < 183 - 26; i++)
            {

                trainingInputMonth[i] = new double[8];
                // One week
                for (int j = 0; j < 7; j++)
                {
                    // Take current price and move one week in year
                    trainingInputMonth[i][j] = currentPrice[i + j].PriceData_ / maxPrice;
                }

                trainingOutPutMonth[i] = new double[1];
                // Predicted value next 3 months
                trainingOutPutMonth[i][0] = currentPrice[i + 92].PriceData_ / maxPrice;
            }
            TrainingData tD = new TrainingData();
            tD.SetTrainData(trainingInputMonth, trainingOutPutMonth);

            // train, number of cycles, number of feedback, error rate
            nn.TrainOnData(tD, 10000, 1000, 0.00002f);
            string location = currentPrice.First().Location_;
            int year = currentPrice.First().Year_;
            testSet = new double[8];

            // Next half year.
            for (int i = currentPrice.Count - 92; i < currentPrice.Count - 7; i++)
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
                futurePriceMonth.Add(predictedPrice);
            }
            var error = computeRMSE(currentPrice, futurePriceMonth);
            Debug.WriteLine("Weather month RMSE for : " + location + " " + year + " " + error);
            return futurePriceMonth;

        }
    }
}

