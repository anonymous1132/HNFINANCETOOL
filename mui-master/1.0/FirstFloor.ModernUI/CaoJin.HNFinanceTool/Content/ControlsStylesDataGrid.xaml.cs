using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        public string DataFileName = "demo.est";

        private ObservableCollection<ProjectEstimateViewModel> financedata;

        private ObservableCollection<ProjectEstimateViewModel> GetData()
        {

            string datafile = _datapath + DataFileName;
            DataSet ds= XmlOperate.GetDataSet(datafile);
            DataTable dt = ds.Tables[0];

            var estimate = ModelConvertHelper<ProjectEstimateViewModel>.ConvertToObc(dt);
            return estimate;
        }

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

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.DataFileName);
        }

        private void button_export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            financedata = GetData();

            //Bind the DataGrid 
            DG1.DataContext = financedata;
        }
    }
}
