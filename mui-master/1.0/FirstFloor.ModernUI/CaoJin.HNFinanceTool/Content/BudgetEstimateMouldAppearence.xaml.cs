using CaoJin.HNFinanceTool.Basement;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using CaoJin.HNFinanceTool.Bll;
using System.Data;
using System.Collections.ObjectModel;
using System.Globalization;
using System;

namespace CaoJin.HNFinanceTool.Content
{
    /// <summary>
    /// BudgetEstimateMouldAppearence.xaml 的交互逻辑
    /// </summary>
    public partial class BudgetEstimateMouldAppearence : UserControl
    {
        public BudgetEstimateMouldAppearence()
        {
            InitializeComponent();
        }
        private ObservableCollection<ProjectEstimateViewModel> obc_project;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string filepath = @"App\data\";
            DirectoryInfo dir = new DirectoryInfo(filepath);
            obc_project = new ObservableCollection<ProjectEstimateViewModel>();
            if (dir.Exists)
            {
                FileInfo[] fiList = dir.GetFiles();
                foreach (FileInfo f in fiList)
                {
                    if (f.Extension == ".est")
                    {
                        DataTable dt = XmlHelper.GetTable(filepath+f.Name,XmlHelper.XmlType.File, "Estinates");
                        ProjectEstimateSetViewModel  setViewModel = new ProjectEstimateSetViewModel(dt);
                        foreach (ProjectEstimateViewModel pvm in setViewModel.EstimateViewModels)
                        {
                            obc_project.Add(pvm);
                        }
                    }
                }
            }
         //   this.DG1.Items.Clear();
            this.DG1.ItemsSource = obc_project;
        }

        private void button_export_Click(object sender, RoutedEventArgs e)
        {
            if (obc_project.Count==0) return;
            string filepath = "";
            SaveFile(ref filepath);
            if (string.IsNullOrEmpty(filepath)) { return; }
            ExcelOper excel = new ExcelOper(filepath);
            foreach (ProjectEstimateViewModel project in obc_project)
            {
                excel.PrintOneProjectEstimateBlcok(project);

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
            saveFile.FileName = "电网基建概算数导入模版-" + DateTime.Now.ToString("yyyyMMdd");
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            filepath = saveFile.FileName;
            string mouldpath = "App\\excel\\mould1.xlsx";
            if (!File.Exists(mouldpath)) { MessageBox.Show("Not Found The Mould File \"mould1.xlsx!\"", "Error"); return; }
            if (File.Exists(filepath)) { try { File.Delete(filepath); } catch (Exception ex) { MessageBox.Show(ex.Message); return; } }
            File.Copy(mouldpath, filepath);
        }
    }
}
