using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace CaoJin.HNFinanceTool.Basement
{
   public class ExcelHelper
    {
        //打开多个文件，每个文件读取一个sheet，返回dataset
        public DataSet ExcelToDS(string[] filepath, bool hasTitle = false, string sheetname = "Sheet1")
        {
            string strCon = "";
            string strCom = "";
            using (DataSet ds = new DataSet())
            {
                for (int i = 0; i < filepath.Length; i++)
                {
                    strCon = getstrCon(filepath[i], hasTitle);
                    strCom = string.Format(" SELECT * FROM [{0}$]", sheetname);
                    try
                    {
                        using (OleDbConnection myConn = new OleDbConnection(strCon))
                        using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, strCon))
                        {
                            myConn.Open();
                            myCommand.Fill(ds, i.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        ds.Tables.Add(i.ToString());
                    }
                }

                if (ds == null || ds.Tables.Count <= 0) return null;
                return ds;
            }
        }

        //打开一个文件，读取一个sheet页，返回datatable
        public DataTable ExcelToDT(string filepath, bool hasTitle = false, string sheetname = "Sheet1")
        {
            string strCon = "";
            string strCom = "";
            using (DataSet ds = new DataSet())
            {
                strCon = getstrCon(filepath, hasTitle);
                strCom = string.Format(" SELECT * FROM [{0}$]", sheetname);
                try
                {
                    using (OleDbConnection myConn = new OleDbConnection(strCon))
                    using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, strCon))
                    {
                        myConn.Open();
                        myCommand.Fill(ds, sheetname);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                if (ds == null || ds.Tables.Count <= 0) return null;
                return ds.Tables[sheetname];
            }
        }

        //构造连接excel字符串
        private string getstrCon(string filepath, bool hasTitle)
        {
            string filetype = Path.GetExtension(filepath);
            return string.Format("Provider=Microsoft.{4}.OLEDB.{0}.0;" +
                                   "Extended Properties=\"Excel {1}.0;HDR={2};IMEX=1;\";" +
                                   "data source={3};",
                                   (filetype == ".xls" ? 4 : 12), (filetype == ".xls" ? 8 : 12), (hasTitle ? "Yes" : "NO"), filepath, (filetype == ".xls" ? "Jet" : "ACE"));
        }

        //打开一个文件，读取文件内所有sheet页，返回dataset
        public DataSet ExcelToDS(string filepath, bool hasTitle = false)
        {
            string strCon = "";
            string strCom = "";
            using (DataSet ds = new DataSet())
            {
                strCon = getstrCon(filepath, hasTitle);
                try
                {
                    using (OleDbConnection myConn = new OleDbConnection(strCon))
                    {
                        myConn.Open();
                        DataTable sheetNames = myConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                        foreach (DataRow dr in sheetNames.Rows)
                        {
                            strCom = string.Format(" SELECT * FROM [{0}]", dr[2].ToString());
                            using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, strCon))
                            {
                                myCommand.Fill(ds, dr[2].ToString());
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return null;
                }
                if (ds == null || ds.Tables.Count <= 0) return null;
                return ds;
            }
        }

        //指定列，查找datatable中的字符串首次在第几行； colnum从0开始
        public int? RowIndex(DataTable dt, string str,int colnum)
        {
            DataView dv = dt.DefaultView;
            if (colnum < dt.Columns.Count)
            {
                foreach (DataRowView drv in dv)
                {
                    if (drv[colnum].ToString() == str)
                    { return dt.Rows.IndexOf(drv.Row); }

                }
            }
            return null;
        }
        //指定列，查找datatable中的字符串首次在第几行； colnum从0开始,模糊查询
        public int? RowIndexContain(DataTable dt, string str, int colnum)
        {
            DataView dv = dt.DefaultView;
            if (colnum < dt.Columns.Count)
            {
                foreach (DataRowView drv in dv)
                {
                    if (drv[colnum].ToString().Contains(str))
                    { return dt.Rows.IndexOf(drv.Row); }

                }
            }
            return null;
        }


        //指定行，查找datatable中字符串首次在第几列
        public int? ColumnIndex(DataTable dt,string str,int row)
        {
            DataView dv = dt.DefaultView;
            if (row < dt.Rows.Count)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dv[row][i].ToString()==str)
                    { return i; }
                }
            }
            return null;
        }

        //指定行，查找datatable中字符串首次在第几列，模糊查询
        public int? ColumnIndexContain(DataTable dt, string str, int row)
        {
            DataView dv = dt.DefaultView;
            if (row < dt.Rows.Count)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dv[row][i].ToString().Replace(" ", "").Contains(str.Replace(" ", "")))
                    { return i; }
                }
            }
            return null;
        }

        //colnum、row都是从0开始算起
        public int[] Cellindex(DataTable dt, string str)
        {
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    if (dt.DefaultView[r][c].ToString()==str)
                    {
                        return new int[]{ r,c};
                    }
                }
            }
            return null;
        }
        //colnum、row都是从0开始算起，单元格包含关键词
        public int[] CellindexContain(DataTable dt, string str)
        {
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    if (dt.DefaultView[r][c].ToString().Contains(str))
                    {
                        return new int[] { r, c };
                    }
                }
            }
            return null;
        }




        //根据同行关键字获取字段数据
        public string GetXValueByKeyWord(DataTable dt, string str)
        {
            int[] cellnum = Cellindex(dt,str);
            for (int i = cellnum[1] + 1; i < dt.Columns.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.DefaultView[cellnum[0]][i].ToString()))
                {
                    return dt.DefaultView[cellnum[0]][i].ToString();
                }
            }
            return "";
        }
        //根据同行关键字获取字段数据，单元格包含关键词
        public string GetXValueByContainKeyWord(DataTable dt,string str)
        {
            int[] cellnum = CellindexContain(dt, str);
            for (int i = cellnum[1] + 1; i < dt.Columns.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.DefaultView[cellnum[0]][i].ToString()))
                {
                    return dt.DefaultView[cellnum[0]][i].ToString();
                }
            }
            return "";
        }
        //根据同行关键字获取字段数据，单元格包含关键词设定最多列数差
        public string GetXValueByContainKeyWord(DataTable dt, string str,int maxskip)
        {
            int[] cellnum = CellindexContain(dt, str);
            if (cellnum[1] + maxskip >=dt.Columns.Count) return GetXValueByContainKeyWord(dt,str);
            for (int i = cellnum[1] + 1; i <= cellnum[1]+maxskip; i++)
            {
                if (!string.IsNullOrEmpty(dt.DefaultView[cellnum[0]][i].ToString()))
                {
                    return dt.DefaultView[cellnum[0]][i].ToString();
                }
            }
            return "";
        }
        //根据同列关键字获取字段数据
        public string GetYValueByKeyWord(DataTable dt, string str)
        {
            int[] cellnum = Cellindex(dt, str);
            for (int i = cellnum[1] + 1; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.DefaultView[i][cellnum[1]].ToString()))
                {
                    return dt.DefaultView[i][cellnum[1]].ToString();
                }
            }
            return "";
        }
        //根据同列关键字获取字段数据，单元格包含关键词
        public string GetYValueByContainKeyWord(DataTable dt,string str)
        {
            int[] cellnum = CellindexContain(dt, str);
            for (int i = cellnum[1] + 1; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.DefaultView[i][cellnum[1]].ToString()))
                {
                    return dt.DefaultView[i][cellnum[1]].ToString();
                }
            }
            return "";
        }
        //根据同列关键字获取字段数据，单元格包含关键词,设定最多行数差
        public string GetYValueByContainKeyWord(DataTable dt, string str,int maxskip)
        {
            int[] cellnum = CellindexContain(dt, str);
            if (cellnum[1] + maxskip > dt.Rows.Count) return GetYValueByContainKeyWord(dt,str);
            for (int i = cellnum[1] + 1; i < cellnum[1]+maxskip; i++)
            {
                if (!string.IsNullOrEmpty(dt.DefaultView[i][cellnum[1]].ToString()))
                {
                    return dt.DefaultView[i][cellnum[1]].ToString();
                }
            }
            return "";

        }

        //dt导出至excel第一个sheet，且将columnname设置为首行
        public void ExExcel(DataTable dt, string path)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null) return;
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Excel.Workbooks workbooks = xlApp.Workbooks;
            Excel.Workbook workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];
            Excel.Range range;
            long totalCount = dt.Rows.Count;
            long rowRead = 0;
            float percent = 0;

            for (int r = 0; r < totalCount; r++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();

                }
                rowRead++;
                percent = ((float)(100 * rowRead)) / totalCount;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                range = (Excel.Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
                range.EntireColumn.AutoFit();
            }
            // xlApp.Visible = true;
            workbook.Saved = true;
            workbook.SaveCopyAs(path);
            workbooks.Close();
            Kill(xlApp);
        }

        //不含标题
        public void ExExcel2(DataTable dt, string path, string sheetname = "")
        {
            if (dt == null || dt.Rows.Count == 0) return;
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null) return;
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Excel.Workbooks workbooks = xlApp.Workbooks;
            Excel.Workbook workbook = workbooks.Add(path);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];

            sheetname = sheetname.Replace("?", "");
            sheetname = sheetname.Replace("[", "");
            sheetname = sheetname.Replace("]", "");
            sheetname = sheetname.Replace("/", "");
            sheetname = sheetname.Replace("\\", "");
            sheetname = sheetname.Replace("*", "");
            sheetname = sheetname.Replace("？", "");
            if (sheetname.Length > 31)
            { sheetname = sheetname.Substring(0, 31); }
            if (sheetname != "")
            {

                worksheet.Name = sheetname;
            }

            long totalCount = dt.Rows.Count;
            //long rowRead = 0;

            for (int r = 0; r < totalCount; r++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[r][i].ToString() != "")
                    {
                        worksheet.Cells[r + 1, i + 1] = dt.Rows[r][i].ToString();
                    }
                }
                //rowRead++;
            }
            workbook.Saved = true;
            workbook.SaveCopyAs(path);
            workbooks.Close();
            Kill(xlApp);
        }

        //本项目专用
        public void DT2Excel3(DataTable dt, string path)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null) return;
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Excel.Workbooks workbooks = xlApp.Workbooks;
            Excel.Workbook workbook = workbooks.Add(path);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];

            long totalCount = dt.Rows.Count;
            //long rowRead = 0;
            long columcount = dt.Columns.Count;

            for (int r = 0; r < totalCount; r++)
            {
                for (int i = 1; i < columcount; i++)
                {
                    if (dt.Rows[r][i].ToString() != "")
                    {
                        worksheet.Cells[r + 3, i ] = dt.Rows[r][i].ToString();
                    }
                }
            }
            //设置表格边框
            //Microsoft.Office.Interop.Excel.Range range = worksheet.Range[worksheet.Cells[3, 1], worksheet.Cells[totalCount, 25]];
            //range.Cells.Borders.LineStyle = 1;

            //保存并关闭
            workbook.Saved = true;
            workbook.SaveCopyAs(path);
            workbooks.Close();
            Kill(xlApp);
        }

        #region   杀死束Excel进程
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口   

            int k = 0;
            GetWindowThreadProcessId(t, out k);   //得到本进程唯一标志k  
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用  
            p.Kill();     //关闭进程k  

        }
        #endregion
    }
}
