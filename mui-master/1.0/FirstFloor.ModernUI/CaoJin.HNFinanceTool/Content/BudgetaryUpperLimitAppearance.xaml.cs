using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using CaoJin.HNFinanceTool.Bll;
using System.Collections.ObjectModel;
using System.IO;

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
    }
}
