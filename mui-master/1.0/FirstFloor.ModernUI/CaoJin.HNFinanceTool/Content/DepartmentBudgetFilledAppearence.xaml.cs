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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaoJin.HNFinanceTool.Bll;
using System.Collections.ObjectModel;
using System.IO;

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
    }
}
