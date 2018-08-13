using System;
using CaoJin.HNFinanceTool.Basement;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using CaoJin.HNFinanceTool.Bll;
using System.Collections.ObjectModel;
using System.IO;
using System.Data;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace CaoJin.HNFinanceTool.Content
{
    /// <summary>
    /// DepartmentBudgetFilledAppearence.xaml 的交互逻辑
    /// </summary>
    public partial class DepartmentBudgetFilledAppearence : UserControl
    {
        public DepartmentBudgetFilledAppearence()
        {
            InitializeComponent();
        }
        ObservableCollection<DepartmentBudgetFilled> obc_department;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string filepath = @"App\data\";
            DirectoryInfo dir = new DirectoryInfo(filepath);
            obc_department = new ObservableCollection<DepartmentBudgetFilled>();
            if (dir.Exists)
            {
                FileInfo[] fiList = dir.GetFiles();
                foreach (FileInfo f in fiList)
                {
                    if (f.Extension == ".est")
                    {
                        DepartmentBudgetFilled budgetary = new DepartmentBudgetFilled(f.Name.Split('.')[0]);
                        budgetary.NumberOnly = obc_department.Count + 1;
                        obc_department.Add(budgetary);
                    }
                }
            }
            this.DG1.ItemsSource = obc_department;

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (obc_department != null)
            {
                foreach (DepartmentBudgetFilled budget in obc_department)
                {
                    budget.SaveToFile();
                }
            }
        }

        private void button_export_Click(object sender, RoutedEventArgs e)
        {
            if (obc_department.Count == 0) return;
            string filepath = "";
            SaveFile(ref filepath);
            if (string.IsNullOrEmpty(filepath)) { return; }
            ExcelOper excel = new ExcelOper(filepath);
            try
            {
                foreach (DepartmentBudgetFilled depart in obc_department)
                {
                    excel.PrintOneDepartmentBudgetFilledBlock(depart);
                }
                MessageBox.Show("成功导出", "Information");
            }
            catch (Exception)
            { }
            finally {
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
            DataTable dt = exceloper.ExcelToDT(filepath, false, "项目部门预算填报");
            if (dt == null)
            {
                MessageBox.Show("EXCEL文件必须包含名为《项目部门预算填报》的sheet", "ERROR");
                return;
            }


            for (int i = 2; i < dt.Rows.Count; i++)
            {
                foreach (DepartmentBudgetFilled depart in obc_department)
                {
                    if (depart.ProjectName == dt.DefaultView[i][2].ToString())
                    {
                        try
                        {
                            depart.DepartmentFilledBudgetWithTax = Convert.ToDouble(dt.DefaultView[i][5].ToString());
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("无法将第"+(i+1).ToString()+"行F列内容转换为数字，请检查文件规范性！   项目名称："+depart.ProjectName, "ERROR");
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
            saveFile.FileName = "项目部门预算填报-" + DateTime.Now.ToString("yyyyMMdd");
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            filepath = saveFile.FileName;
            string mouldpath = "App\\excel\\mould3.xlsx";
            if (!File.Exists(mouldpath)) { MessageBox.Show("Not Found The Mould File \"mould3.xlsx!\"", "Error"); filepath = ""; return; }
            if (File.Exists(filepath)) { try { File.Delete(filepath); } catch (Exception ex) { MessageBox.Show(ex.Message); filepath = ""; return; } }
            File.Copy(mouldpath, filepath);
        }

    }

    
    
}
