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

namespace CaoJin.HNFinanceTool.Content
{

    public class ExpanseItem
    {
        public string ProjectName { get; set; } //项目名称
        public string ProjectCode { get; set; } //项目编码
        public string IndividualProjectName { get; set; } //单项工程名称
        public string IndividualProjectCode { get; set; } //单项工程编码
        public string ExpanseCategory { get; set; } //费用类别
        public string WBSCode { get; set; }//wbs识别码
        public double EstimateNumber { get; set; }//概算数
        public double InternalControl { get; set; }//内控系数
        public string DeductibleVATRatio { get; set; }//可抵扣增值税比例
        public double TotalInvestmentWithTax { get; set; }//总投资预算（含税）
        public double TotalInvestmentWithoutTax { get; set; }//总投资预算（不含税）
        public double MaxInternalControl { get; set; }//内控系数上限
        public string MaxDeductibleVATRatio { get; set; }//可抵扣增值税比例上限
        public string MinDeductibleVATRatio { get; set; }//可抵扣增值税比例下限

    }


    /// <summary>
    /// Interaction logic for ControlsStylesDataGrid.xaml
    /// </summary>
    public partial class ControlsStylesDataGrid : UserControl
    {
        public ControlsStylesDataGrid()
        {
            InitializeComponent();

            ObservableCollection<ExpanseItem> custdata = GetData();

            //Bind the DataGrid to the customer data
            DG1.DataContext = custdata;
        }

        private ObservableCollection<ExpanseItem> GetData()
        {
            var expanseitems = new ObservableCollection<ExpanseItem>();
 

            return expanseitems;
        }
    }
}
