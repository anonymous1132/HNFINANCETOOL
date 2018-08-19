using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CaoJin.HNFinanceTool.Basement;
using System.Data;
using System.Collections.ObjectModel;
using CaoJin.HNFinanceTool.Bll;
using System.IO;
using System.Globalization;
namespace CaoJin.HNFinanceTool.Content
{
    /// <summary>
    /// ImportMouldAppearence.xaml 的交互逻辑
    /// </summary>
    public partial class ImportMouldAppearence : UserControl
    {
        public ImportMouldAppearence()
        {
            InitializeComponent();
        }

        private ObservableCollection<ProjectImportMouldViewModel> obc_import;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string filepath = @"App\data\";
            DirectoryInfo dir = new DirectoryInfo(filepath);
            obc_import = new ObservableCollection<ProjectImportMouldViewModel>();
            if (dir.Exists)
            {
                FileInfo[] fiList = dir.GetFiles();
                foreach (FileInfo f in fiList)
                {
                    if (f.Extension == ".est")
                    {
                        DataTable dt = XmlHelper.GetTable(filepath + f.Name, XmlHelper.XmlType.File, "Estinates");
                        ProjectEstimateSetViewModel setViewModel = new ProjectEstimateSetViewModel(dt);
                        foreach (ProjectEstimateViewModel pvm in setViewModel.EstimateViewModels)
                        {
                            ProjectImportMouldViewModel import= new ProjectImportMouldViewModel();
                            import.GetData(pvm);
                            obc_import.Add(import);
                        }
                        DepartmentBudgetFilled department = new DepartmentBudgetFilled(setViewModel.TotalEstimateViewModel.ProjectName);
                        obc_import[obc_import.Count-23].DepartmentFilledBudgetWithTax = department.DepartmentFilledBudgetWithTax;
                        obc_import[obc_import.Count-23].YearBudgetWithoutTax = 0;
                        for (int i = obc_import.Count-1; i >obc_import.Count-23; i--)
                        {
                            obc_import[i].DepartmentFilledBudgetWithTax = (setViewModel.EstimateViewModels[i%23].totalInvestmentWithTax / setViewModel.TotalInvestmentWithTax) * department.DepartmentFilledBudgetWithTax;
                            obc_import[i].YearBudgetWithoutTax = obc_import[i].DepartmentFilledBudgetWithTax / (1 + obc_import[i].deductibleVATRatio / 100);
                            obc_import[obc_import.Count-23].YearBudgetWithoutTax += obc_import[i].YearBudgetWithoutTax;
                        }
                      
                    }
                }
            }
            this.DG1.ItemsSource = obc_import;
        }

        private void button_export_Click(object sender, RoutedEventArgs e)
        {
            if (obc_import.Count == 0) return;
            string filepath = "";
            SaveFile(ref filepath);
            if (string.IsNullOrEmpty(filepath)) { return; }
            ExcelOper excel = new ExcelOper(filepath);
            foreach (ProjectImportMouldViewModel import in obc_import)
            {
                excel.PrintOneImportBlock(import);

            }
            excel.Save();
            excel.Quit();
            MessageBox.Show("成功导出", "Information");

        }

        private void SaveFile(ref string filepath)
        {
            System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
            saveFile.Filter = "Excel工作表(*.xlsx)|*.xlsx|Excel 97-2003工作表(*.xls)|*.xls";
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            saveFile.FileName = "年度预算导入模版-" + DateTime.Now.ToString("yyyyMMdd");
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            filepath = saveFile.FileName;
            string mouldpath = "App\\excel\\mould2.xlsx";
            if (!File.Exists(mouldpath)) { MessageBox.Show("Not Found The Mould File \"mould2.xlsx!\"", "Error"); return; }
            if (File.Exists(filepath)) { try { File.Delete(filepath); } catch (Exception ex) { MessageBox.Show(ex.Message); return; } }
            File.Copy(mouldpath, filepath);
        }
    }
}
