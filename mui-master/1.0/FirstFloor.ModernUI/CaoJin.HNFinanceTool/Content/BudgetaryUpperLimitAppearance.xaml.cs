using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CaoJin.HNFinanceTool.Basement;
using System.Windows.Input;
using CaoJin.HNFinanceTool.Bll;
using System.Collections.ObjectModel;
using System.IO;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;


namespace CaoJin.HNFinanceTool.Content
{
    /// <summary>
    /// BudgetaryUpperLimitAppearance.xaml 的交互逻辑
    /// </summary>
    public partial class BudgetaryUpperLimitAppearance : UserControl
    {
        public BudgetaryUpperLimitAppearance()
        {
            InitializeComponent();
        }
        ObservableCollection<BudgetaryUpperLimit> obc_budgetary;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string filepath = @"App\data\";
            DirectoryInfo dir = new DirectoryInfo(filepath);
            obc_budgetary = new ObservableCollection<BudgetaryUpperLimit>();
            if (dir.Exists)
            {
                FileInfo[] fiList = dir.GetFiles();
                foreach (FileInfo f in fiList)
                {
                    if (f.Extension == ".est")
                    {
                        BudgetaryUpperLimit budgetary = new BudgetaryUpperLimit(f.Name.Split('.')[0]);
                        obc_budgetary.Add(budgetary);
                    }
                }
            }
            this.DG1.ItemsSource = obc_budgetary;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (obc_budgetary != null)
            {
                foreach (BudgetaryUpperLimit budget in obc_budgetary)
                {
                    budget.SaveToFile();
                }
            }
        }

        private void DG1_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Input.KeyboardDevice kd = e.KeyboardDevice;
            if ((kd.GetKeyStates(Key.LeftCtrl) & kd.GetKeyStates(Key.V)) > 0 || (kd.GetKeyStates(Key.LeftCtrl) & kd.GetKeyStates(Key.V)) > 0)
            {
                DataGirdViewCellPaste();
            }
        }

        private void DataGirdViewCellPaste()
        {
    
                // 获取剪切板的内容，并按行分割  
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return;
                int tnum = 0;
                int nnum = 0;
                //获得当前剪贴板内容的行、列数
                for (int i = 0; i < pasteText.Length; i++)
                {
                    if (pasteText.Substring(i, 1) == "\t")
                    {
                        tnum++;
                    }
                    if (pasteText.Substring(i, 1) == "\n")
                    {
                        nnum++;
                    }
                }
                Object[,] data;
                //粘贴板上的数据来自于EXCEL时，每行末都有\n，在DATAGRIDVIEW内复制时，最后一行末没有\n
                if (pasteText.Substring(pasteText.Length - 1, 1) == "\n")
                {
                    nnum = nnum - 1;

                }

                tnum = tnum / (nnum + 1);
                data = new object[nnum + 1, tnum + 1];//定义一个二维数组

                String rowstr;
                rowstr = "";
                //MessageBox.Show(pasteText.IndexOf("B").ToString());
                //对数组赋值
                for (int i = 0; i < (nnum + 1); i++)
                {
                    for (int colIndex = 0; colIndex < (tnum + 1); colIndex++)
                    {
                        //一行中的最后一列
                        if (colIndex == tnum && pasteText.IndexOf("\r") != -1)
                        {
                            rowstr = pasteText.Substring(0, pasteText.IndexOf("\r"));
                        }
                        //最后一行的最后一列
                        if (colIndex == tnum && pasteText.IndexOf("\r") == -1)
                        {
                            rowstr = pasteText.Substring(0);
                        }
                        //其他行列
                        if (colIndex != tnum)
                        {
                            rowstr = pasteText.Substring(0, pasteText.IndexOf("\t"));
                            pasteText = pasteText.Substring(pasteText.IndexOf("\t") + 1);
                        }
                        data[i, colIndex] = rowstr;
                    }
                    //截取下一行数据
                    pasteText = pasteText.Substring(pasteText.IndexOf("\n") + 1);

                }
                //获取当前选中单元格所在的列序号
                //int curntindex = DG1.CurrentRow.Cells.IndexOf(dataGridView1.CurrentCell);
                int currentindex = DG1.CurrentCell.Column.DisplayIndex;
                 string columnname = DG1.CurrentColumn.Header.ToString();
                //获取获取当前选中单元格所在的行序号
                //int rowindex = dataGridView1.CurrentRow.Index;
                int rowindex = DG1.SelectedIndex;
            if (columnname == "项目编号")
            {
                for(int i=0;i<data.GetLength(0);i++)
                {
                    obc_budgetary[rowindex+i].ProjectCode = data[i, 0].ToString();
                    if (rowindex + i == obc_budgetary.Count-1) break;
                }
                
            }

            if (columnname == "累计综合计划下达")
            {
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    try
                    {
                        obc_budgetary[rowindex + i].AccumulativePlan = Convert.ToDouble(data[i, 0].ToString());
                    }
                    catch (Exception) { }
                 
                    if (tnum >=2)
                    {
                        try
                        {
                            obc_budgetary[rowindex + i].ErpHappenedWithoutTax = Convert.ToDouble(data[i, 1].ToString());
                        }
                        catch (Exception) { }
                        try
                        {
                            obc_budgetary[rowindex + i].DeductibleVAT = Convert.ToDouble(data[i, 2].ToString());
                        }
                        catch (Exception) { }
                    }
                    else if (tnum >= 1)
                    {
                        try
                        {
                            obc_budgetary[rowindex + i].ErpHappenedWithoutTax = Convert.ToDouble(data[i, 1].ToString());
                        }
                        catch (Exception) { }
                    }
                    if (rowindex + i == obc_budgetary.Count - 1) break;
                }
            }

            if (columnname == "截至上年ERP已发生（不含税）")
            {
                 for (int i = 0; i < data.GetLength(0); i++)
                {
                    try
                    {
                        obc_budgetary[rowindex + i].ErpHappenedWithoutTax = Convert.ToDouble(data[i, 0].ToString());
                    }
                    catch (Exception) { }
                 
     
                   if (tnum >= 1)
                    {
                        try
                        {
                            obc_budgetary[rowindex + i].DeductibleVAT = Convert.ToDouble(data[i, 1].ToString());
                        }
                        catch (Exception) { }
                    }
                    if (rowindex + i == obc_budgetary.Count - 1) break;
                }
            }

            if (columnname == "截至上年累计已抵扣增值税")
            {
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    try
                    {
                        obc_budgetary[rowindex + i].DeductibleVAT = Convert.ToDouble(data[i, 0].ToString());
                    }
                    catch (Exception) { }

                    if (rowindex + i == obc_budgetary.Count - 1) break;
                }
            }
        }

        private void button_export_Click(object sender, RoutedEventArgs e)
        {
            if (obc_budgetary.Count == 0) return;
            string filepath = "";
            SaveFile(ref filepath);
            if (string.IsNullOrEmpty(filepath)) { return; }
            ExcelOper excel = new ExcelOper(filepath);
            try
            {
                foreach (BudgetaryUpperLimit limit in obc_budgetary)
                {
                    excel.PrintOneBudgetaryUpperLimitBlock(limit);
                }
                MessageBox.Show("成功导出", "Information");
            }
            catch (Exception ex)
            {
                MessageBox.Show("过程出现错误","Error");
            }
            finally
            {
                excel.Save();
                excel.Quit();
            }
        }

        private void button_import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx|Excel 97-2003 Files (*.xls)|*.xls" };
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            string filepath = openFile.FileName;
            ExcelHelper exceloper = new ExcelHelper();
            DataTable dt = exceloper.ExcelToDT(filepath, false, "预算上限计算表");
            if (dt == null)
            {
                MessageBox.Show("EXCEL文件必须包含名为《预算上限计算表》的sheet", "ERROR");
                return;
            }
            for (int i = 2; i < dt.Rows.Count; i++)
            {
                foreach (BudgetaryUpperLimit limit in obc_budgetary)
                {
                    if (limit.ProjectName == dt.DefaultView[i][1].ToString())
                    {
                        try
                        {
                            limit.AccumulativePlan = Convert.ToDouble(dt.DefaultView[i][2].ToString());
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("无法将第" + (i + 1).ToString() + "行C列内容转换为数字，请检查文件规范性！   项目名称：" + limit.ProjectName, "ERROR");
                            break;
                        }
                        try
                        {
                            limit.ErpHappenedWithoutTax = Convert.ToDouble(dt.DefaultView[i][3].ToString());
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("无法将第" + (i + 1).ToString() + "行D列内容转换为数字，请检查文件规范性！   项目名称：" + limit.ProjectName, "ERROR");
                            break;
                        }

                        try
                        {
                            limit.DeductibleVAT = Convert.ToDouble(dt.DefaultView[i][4].ToString());
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("无法将第" + (i + 1).ToString() + "行E列内容转换为数字，请检查文件规范性！   项目名称：" + limit.ProjectName, "ERROR");
                            break;
                        }
                        break;
                    }
                }
            }

        }

        private void SaveFile(ref string filepath)
        {
            System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
            saveFile.Filter = "Excel工作表(*.xlsx)|*.xlsx|Excel 97-2003工作表(*.xls)|*.xls";
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            saveFile.FileName = "预算上限计算表-" + DateTime.Now.ToString("yyyyMMdd");
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            filepath = saveFile.FileName;
            string mouldpath = "App\\excel\\mould4.xlsx";
            if (!File.Exists(mouldpath)) { MessageBox.Show("Not Found The Mould File \"mould4.xlsx!\"", "Error"); filepath = ""; return; }
            if (File.Exists(filepath)) { try { File.Delete(filepath); } catch (Exception ex) { MessageBox.Show(ex.Message); filepath = ""; return; } }
            File.Copy(mouldpath, filepath);
        }
    }
}
