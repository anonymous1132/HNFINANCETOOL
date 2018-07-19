using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaoJin.HNFinanceTool.Bll;
using CaoJin.HNFinanceTool.Dal;
using CaoJin.HNFinanceTool.Basement;
using System.Data;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace CaoJin.HNFinanceTool.Content
{

    /// <summary>
    /// Interaction logic for ControlsStylesDataGrid.xaml
    /// </summary>
    public partial class ControlsStylesDataGrid : UserControl
    {
        public ControlsStylesDataGrid()
        {
            InitializeComponent();
        }


        private string _datapath="App\\data\\";

        public string DataFileName = "mould";

        private ObservableCollection<ProjectEstimateViewModel> financedata;

        //从文件获取数据
        private ObservableCollection<ProjectEstimateViewModel> GetData()
        {

            string datafile = _datapath + DataFileName;
            DataSet ds= XmlOperate.GetDataSet(datafile);
            DataTable dt = ds.Tables[0];

            var estimate = ModelConvertHelper<ProjectEstimateViewModel>.ConvertToObc(dt);
            return estimate;
        }

        //将obc转换为dt
        private DataTable TranslateVM2DT()
        {
            DataTable dt = new DataTable();
            dt = ModelConvertHelper<ProjectEstimateViewModel>.ConvertToDt(financedata);
            return dt;
        }
        //全置button
        private void button_allset_Click(object sender, RoutedEventArgs e)
        {
            switch (this.combobox_title.SelectedIndex)
            {
                case 0:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.ProjectName = this.textbox_setcontent.Text;
                    }
                    break;
                case 1:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.ProjectCode = this.textbox_setcontent.Text;
                    }
                    break;
                case 2:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.WBSCode = this.textbox_setcontent.Text;
                    }
                    break;
                case 3:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.InternalControl = this.textbox_setcontent.Text;
                    }
                    break;
                case 4:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.DeductibleVATRatio = this.textbox_setcontent.Text;
                    }
                    break;
                case 5:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.MaxInternalControl = this.textbox_setcontent.Text;
                    }
                    break;
                case 6:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.MaxDeductibleVATRatio = this.textbox_setcontent.Text;
                    }
                    break;
                case 7:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.MinDeductibleVATRatio= this.textbox_setcontent.Text;
                    }
                    break;
                default:

                    break;

            }
        }
        //保存至文件
        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            if (DataFileName == "mould")
            {
                SaveFileDialog saveFile = new SaveFileDialog() { Filter = "财务工具文件 (*.est)|*.est" };
                saveFile.Title = "导出文件路径";
                saveFile.FileName = DateTime.Now.GetDateTimeFormats('D')[0].ToString() + financedata[0].ProjectName;
                saveFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\App\\data";
                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
                XmlHelper.SaveTableToFile(TranslateVM2DT(), saveFile.FileName);
                this.DataFileName = System.IO.Path.GetFileName(saveFile.FileName);
            }
            else
            {
                string datafile = _datapath + DataFileName;
                XmlHelper.SaveTableToFile(TranslateVM2DT(), datafile);
            }
        }
        //导出至excel
        private void button_export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx" };
            saveFile.Title = "导出文件路径";
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            saveFile.FileName = DateTime.Now.GetDateTimeFormats('D')[0].ToString() + financedata[0].ProjectName;
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            try
            {
                if (System.IO.File.Exists(saveFile.FileName)) { System.IO.File.Delete(saveFile.FileName); }
                string mouldpath = "App\\excel\\mould1.xlsx";
                if (!System.IO.File.Exists(mouldpath)) { MessageBox.Show("错误：App\\excel\\mould1.xlsx丢失！"); return; }
                System.IO.File.Copy(mouldpath, saveFile.FileName);
                ExcelHelper exceloper = new ExcelHelper();
                exceloper.DT2Excel3(TranslateVM2DT(), saveFile.FileName);
                MessageBox.Show("操作完成！");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            financedata = GetData();

            //Bind the DataGrid 
            DG1.DataContext = financedata;
        }

        private void button_import_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImportFile()) return;


        }

        private ProjectClass proc;
        private bool CheckImportFile()
        {
            OpenFileDialog openFile = new OpenFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx|Excel 97-2003 Files (*.xls)|*.xls" };
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return false;
            string filepath = openFile.FileName;
            ExcelHelper exceloper = new ExcelHelper();
            DataSet ds = exceloper.ExcelToDS(filepath);
            string tablenames = "";
            //所有表名称拼接
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                tablenames = tablenames + ds.Tables[i].TableName;
            }
            if (!(tablenames.Contains("封面") && tablenames.Contains("总概算")))
            {
                MessageBox.Show("Excel文件缺少《封面》或《总概算》表");
                return false;
            }
            //获取《封面》内容，根据封面内容获取项目名称、项目编号
            proc = new ProjectClass();
            int rownum = exceloper.cellindex(ds.Tables["封面$"], "工 程 名 称:")[1];
            MessageBox.Show(rownum.ToString());
            return true;
        }

       
    }
}
