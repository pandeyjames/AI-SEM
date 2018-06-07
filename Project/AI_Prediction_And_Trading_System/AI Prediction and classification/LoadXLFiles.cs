using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace AI_Prediction_and_classification
{
    /// <summary>
    /// This class loads the EXCEL files from nordpoolspot.com (the price data) and YR.no (the weather data)
    /// </summary>
    class LoadXLFiles
    {
        List<string> priceExcel = new List<string>();
        List<string> weatherExcel = new List<string>();

        public List<Price> loadExcelFiles()
        {
            List<Price> priceList = new List<Price>();
            Application _excelApp = new Application();

            var path = AppDomain.CurrentDomain.BaseDirectory;
            priceExcel.Add(path + "../../Price/elspot-prices_2013_daily_nok.xlsx");
            priceExcel.Add(path + "../../Price/elspot-prices_2014_daily_nok.xlsx");
            priceExcel.Add(path + "../../Price/elspot-prices_2015_daily_nok.xlsx");
            priceExcel.Add(path + "../../Price/elspot-prices_2016_daily_nok.xlsx");
            priceExcel.Add(path + "../../Price/elspot-prices_2017_daily_nok.xlsx");
            priceExcel.Add(path + "../../Price/elspot-prices_2018_daily_nok.xlsx");


            for (int k = 0; k < 6; k++)
            {
                string currentExcelSheet = priceExcel[k];
                try
                {
                    Workbook workBook = _excelApp.Workbooks.Open(currentExcelSheet,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                    int numSheets = workBook.Sheets.Count;

                    for (int sheetNum = 1; sheetNum < numSheets + 1; sheetNum++)
                    {
                        Worksheet sheet = (Worksheet)workBook.Sheets[sheetNum];

                        // Take the used range of the sheet. Get an object array of all of the cells in the sheet (their values)
                        Range excelRange = sheet.UsedRange;
                        object[,] valueArray = (object[,])excelRange.get_Value(
                            XlRangeValueDataType.xlRangeValueDefault);
                        for (int i = 10; i < 16; i++)
                        {
                            for (int j = 5; j <= valueArray.Length; j++)
                            {
                                try
                                {
                                     var date = (valueArray[j, 1]).ToString();
                                     var location = (valueArray[4, i]).ToString();
                               
                                     Price price = new Price(date, location, (double)valueArray[j, i], 2013 + k);
                                     priceList.Add(price);      

                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }
                    }

                    workBook.Close(false, currentExcelSheet, null);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    
                }
            }
            return priceList;
        }
        public List<Weather> loadWeather()
        {
            List<Weather> weatherList = new List<Weather>();
            Application _excelAppW = new Application();

            var path = AppDomain.CurrentDomain.BaseDirectory;
            weatherExcel.Add(path + "../../Weather/Weather_2013.xlsx");
            weatherExcel.Add(path + "../../Weather/Weather_2014.xlsx");
            weatherExcel.Add(path + "../../Weather/Weather_2015.xlsx");
            weatherExcel.Add(path + "../../Weather/Weather_2016.xlsx");


            for (int k = 0; k < 4; k++)
            {
                string currentExcelSheet = weatherExcel[k];
                try
                {
                    Workbook workBook = _excelAppW.Workbooks.Open(currentExcelSheet,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                    int numSheets = workBook.Sheets.Count;

                    for (int sheetNum = 1; sheetNum < numSheets + 1; sheetNum++)
                    {
                        Worksheet sheet = (Worksheet)workBook.Sheets[sheetNum];

                        // Take the used range of the sheet. Get an object array of all of the cells in the sheet (their values)
                        Range excelRange = sheet.UsedRange;
                        object[,] valueArray = (object[,])excelRange.get_Value(
                            XlRangeValueDataType.xlRangeValueDefault);
                        for (int i = 10; i < 16; i++)
                        {
                            for (int j = 5; j <= valueArray.Length; j++)
                            {
                                try
                                {
                                    var date = (valueArray[j, 1]).ToString();
                                    var location = (valueArray[4, i]).ToString();

                                    Weather weather = new Weather(date, location, (double)valueArray[j, i], 2013 + k);
                                    weatherList.Add(weather);


                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }
                    }

                    workBook.Close(false, currentExcelSheet, null);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);

                }
            }
            return weatherList;
        }  
    }
}
