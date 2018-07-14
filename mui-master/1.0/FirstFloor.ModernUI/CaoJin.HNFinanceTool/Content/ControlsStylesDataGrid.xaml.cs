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

            ObservableCollection<ProjectEstimateViewModel> financedata = GetData();

            //Bind the DataGrid 
            DG1.DataContext = financedata;
        }

        private string datapath="App\\data\\";

        public string datafile = "demo.est";

        private ObservableCollection<ProjectEstimateViewModel> GetData()
        {

           DataSet ds= XmlOperate.GetDataSet(datafile);
            DataTable dt = ds.Tables[0];

            var estimate = ModelConvertHelper<ProjectEstimateViewModel>.ConvertToObc(dt);
            return estimate;
        }
    }
}
