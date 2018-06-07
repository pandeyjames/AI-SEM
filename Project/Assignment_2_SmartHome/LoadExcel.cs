using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SmartHome {
    /// <summary>
    /// this class load the excel files with use of Microsoft Office Interop Excel
    /// 
    /// </summary>
    public class LoadExcel {

        Application excelApp_;
        List<WeatherData> listWeather = new List<WeatherData>();

        public List<WeatherData> loadExcel() {
            excelApp_ = new Application();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = path + "../../datafiles/Bergen_Flesland_2016_jan1_april9.xlsx";
            Debug.WriteLine(fileName);
            //try {
            Workbooks wbs = excelApp_.Workbooks;
            Workbook workBook = wbs.Open(fileName,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing);

            int numSheets = workBook.Sheets.Count;
            for (int sheetNum = 1; sheetNum < numSheets + 1; sheetNum++) {

                Worksheet sheet = (Worksheet)workBook.Sheets[sheetNum];
                Range excelRange = sheet.UsedRange;
                object[,] valueArray = (object[,])excelRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);

                Range findLastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                int lastRow = findLastRow.Row;

                int day = 1;
                for (int row = 19; row <= lastRow; row++) {
                    for (int col = 4; col <= 4; col++) {

                        var date = (valueArray[row, 2]).ToString();
                        int k = date.IndexOf(":");
                        int f = date.Count();
                        string findHour = date.Substring(k - 2, 2);
                        var hour = int.Parse(findHour);
                        WeatherData weather = new WeatherData(hour, day, /* (int)valueArray[row,col],*/ (double)valueArray[row, col]);
                        listWeather.Add(weather);

                        if (hour == 23) {
                            day++;
                        }
                    }
                }
            }
            workBook.Close(false, fileName, null);
            Marshal.ReleaseComObject(wbs);
            Marshal.ReleaseComObject(workBook);
            //}
            //catch (Exception ex) {
            //    Debug.WriteLine(ex.Message);
            // }
            return listWeather;
        }
    }
}


