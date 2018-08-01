using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace CaoJin.HNFinanceTool.Basement
{
   public class ExcelAppHelper
    {
        public ExcelAppHelper()
        { }
        public DataSet GetContent(string path)
        {
            String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
             "Data Source="+path+";" +
            "Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(sConnectionString);
            objConn.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [sheet1]", objConn);
            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
            objAdapter1.SelectCommand = objCmdSelect;
            DataSet objDataset1 = new DataSet();
            //将Excel中数据填充到数据集
            objAdapter1.Fill(objDataset1, "XLData");
            objConn.Close();
            return objDataset1;
        }


        public Excel.Application app()
        {
            //设置程序运行语言
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            //创建Application
            Excel.Application xlApp = new Excel.Application();
            //设置是否显示警告窗体
            xlApp.DisplayAlerts = false;
            //设置是否显示Excel
            xlApp.Visible = false;
            //禁止刷新屏幕
            xlApp.ScreenUpdating = false;
            //屏蔽关闭前告警
            xlApp.AlertBeforeOverwriting = false;
            //根据路径path打开
            return xlApp;
        }

        public Excel.Workbook GetWorkbook(string filePath,Excel.Application app)
        {
            //根据路径path打开
            Excel.Workbook xlsWorkBook = app.Workbooks.Open(filePath, System.Type.Missing, System.Type.Missing, System.Type.Missing,
            System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing,
            System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);

            return xlsWorkBook;
        }

        //按sheet名获取
        public Excel.Worksheet GetWorksheet(Excel.Workbook workbook,string sheetname)
        {
            return  (Worksheet)workbook.Worksheets[sheetname];
        }
        //按sheet排序获取
        public Excel.Worksheet GetWorksheet(Excel.Workbook workbook,int sheetno)
        {
            return (Worksheet)workbook.Worksheets[sheetno];
        }

        public int GetRowRang(Excel.Worksheet sheet)
        {
            return sheet.UsedRange.Rows.Count;
        }

        public int GetColumRange(Excel.Worksheet worksheet)
        {
            return worksheet.UsedRange.Columns.Count;
        }

        //删除行
        public void DeleteRow(Excel.Worksheet sheet,int rowno)
        {
            Range deleteRng = (Range)sheet.Rows[rowno, System.Type.Missing];
            deleteRng.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
        }
        //删除列
        public void DeleteColumn(Excel.Worksheet sheet,int colno)
        {
            ((Excel.Range)sheet.Cells[1,colno]).EntireColumn.Delete(0);
        }

        //设置背景色，红色colorindex=3
        public void SetRangeBackground(Range range,int colorIndex=3)
        {
            range.Interior.ColorIndex = colorIndex;
        }

        //获取range值
        public string GetCellValue(Range range)
        {
          return  Convert.ToString(range.Value2);
            
        }

        //设置行高
        public void SetRowHeight(Range range,double height)
        {
            range.RowHeight = height;
        }

        //设置字号
        public void SetFontSize(Range range,int size)
        {
            range.Font.Size = size;
        }
        //设置字体
        public void SetFontStyle(Range range,string name)
        {
            range.Font.Name = name;
        }

        //是否设置粗体
        public void SetBoldValue(Range range,bool isbold=true)
        {
            range.Font.Bold = isbold;
        }

        //设置水平垂直居中
        public void SetFontHVCenter(Range range)
        {
            range.HorizontalAlignment = XlVAlign.xlVAlignCenter;
        }

        //设置水平靠左
        public void SetFontHVLeft(Range range)
        {
            range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
        }

        //设置区域边框
        public void SetRangeBodersStyle(Range range, int linestyle)
        {
            range.Borders.LineStyle = linestyle;
        }

        //设置边框的线条
        public  void SetRangeBodersThickness(Range range,XlBorderWeight weight)
        {
            range.Borders.Weight = weight;
        }

        //设置区域单元格为数字格式,小数点后面保留1位
        public void SetRangeValueStyleNumber(Range range,string format="0.0")
        {
            range.NumberFormat = format;
        }

        //设置区域单元格为文本格式
        public void SetRangeValueStyleText(Range range)
        {
            range.NumberFormat = "@";
        }

        //设置区域复制
        public void Copy(Range sRange,Range dRang)
        {
            sRange.Select();
            sRange.Copy(Type.Missing);
            dRang.Select();
            dRang.Parse(Missing.Value, Missing.Value);
        }

        public void Save(Excel.Workbook workbook,string filePath)
        {
           workbook.SaveAs(filePath,Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        }

        //关闭excel
        public void QuitExcel(Excel.Application app, Excel.Workbook workbook)
        {
            workbook.Close();
            app.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            app= null;
        }
        
    }
}
