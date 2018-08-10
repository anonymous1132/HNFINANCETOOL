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

        }


    }
}
