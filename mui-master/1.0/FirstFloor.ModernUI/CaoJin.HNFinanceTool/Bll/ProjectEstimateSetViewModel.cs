using System.Reflection;
using FirstFloor.ModernUI.Presentation;
using System.Collections.ObjectModel;
using System.Data;
using System;
namespace CaoJin.HNFinanceTool.Bll
{
    public class ProjectEstimateSetViewModel : NotifyPropertyChanged
    {
        public ProjectEstimateSetViewModel(DataTable dataTable)
        {
            this.dataTable = dataTable;
            SetEstimateViewModels();
        }
        private DataTable dataTable;
        private ProjectTotalEstimateViewModel _totalEstimateViewModel = new ProjectTotalEstimateViewModel();
        public ProjectTotalEstimateViewModel TotalEstimateViewModel
        {
            get
            {
                _totalEstimateViewModel.TotalInvestmentWithTax = this.TotalInvestmentWithTax.ToString();
                _totalEstimateViewModel.TotalInvestmentWithoutTax = this.TotalInvestmentWithoutTax.ToString();

                return _totalEstimateViewModel;
            }
            set { _totalEstimateViewModel = value; OnPropertyChanged("TotalEstimateViewModel"); }
        }
        public ObservableCollection<ProjectEstimateViewModel> EstimateViewModels;
        private void SetEstimateViewModels()
        {
            EstimateViewModels = new ObservableCollection<ProjectEstimateViewModel>();
            string tempName = "";
            for (int i = 1; i < dataTable.Rows.Count; i++)
            {

                ProjectEstimateViewModel estimateViewModel = new ProjectEstimateViewModel();
                PropertyInfo[] propertys = estimateViewModel.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列   
                    if (dataTable.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter     
                        if (!pi.CanWrite) continue;
                        object value = dataTable.DefaultView[i][tempName];
                        if (value != System.DBNull.Value)
                        {
                            pi.SetValue(estimateViewModel, value, null);
                        }
                    }
                }
                EstimateViewModels.Add(estimateViewModel);
            }
            TotalEstimateViewModel.ProjectName = dataTable.DefaultView[0]["ProjectName"].ToString();
            TotalEstimateViewModel.ProjectCode = dataTable.DefaultView[0]["ProjectCode"].ToString();
            TotalEstimateViewModel.EstimateNumber = dataTable.DefaultView[0]["EstimateNumber"].ToString();
            EstimateViewModels.Insert(0, TotalEstimateViewModel);
        }


        // private double _totalInvestmentWithTax;
        public double TotalInvestmentWithTax
        {
            get
            {
                double temp = 0;
                foreach (ProjectEstimateViewModel vm in EstimateViewModels)
                {
                    if (vm is ProjectTotalEstimateViewModel) { break; }
                    temp += Convert.ToDouble(vm.TotalInvestmentWithTax);
                }
                return temp;
            }
        }

        public double TotalInvestmentWithoutTax
        {
            get
            {
                double temp = 0;
                foreach (ProjectEstimateViewModel vm in EstimateViewModels)
                {
                    if (vm is ProjectTotalEstimateViewModel) { break; }
                    temp += Convert.ToDouble(vm.TotalInvestmentWithoutTax);
                }
                return temp;
            }
        }

        //为financdata赋值
        public void GetDataToFinanceData(ProjectClass proc, ProjectCostCatagorySet catagorySet)
        {
            foreach (ProjectEstimateViewModel pevm in EstimateViewModels)
            {
                pevm.ProjectName = proc.ProjectName;
                pevm.ProjectCode = proc.ProjectCode;
                if (pevm is ProjectTotalEstimateViewModel) { ((ProjectTotalEstimateViewModel)pevm).EstimateNumber = catagorySet.pcc_all.costValue.ToString();continue; }
                switch (pevm.ExpanseCategory)
                {
                    case "10KV（含20KV）及以下基建项目":
                        pevm.EstimateNumber = catagorySet.pcc_all.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—配电站（开关站）工程—建筑工程":
                        pevm.EstimateNumber = catagorySet.pcc_pd_jz.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—配电站（开关站）工程—安装工程":
                        pevm.EstimateNumber = catagorySet.pcc_pd_az.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—配电站（开关站）工程—设备购置":
                        pevm.EstimateNumber = catagorySet.pcc_pd_sb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—通信及调度自动化—建筑工程":
                        pevm.EstimateNumber = catagorySet.pcc_tx_jz.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—通信及调度自动化—安装工程":
                        pevm.EstimateNumber = catagorySet.pcc_tx_az.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—通信及调度自动化—设备购置":
                        pevm.EstimateNumber = catagorySet.pcc_tx_sb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—架空线路工程—架空线路本体工程":
                        pevm.EstimateNumber = catagorySet.pcc_jk.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—电缆线路工程—电缆本体工程":
                        pevm.EstimateNumber = catagorySet.pcc_dl.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—建设场地征用及清理费":
                        pevm.EstimateNumber = catagorySet.pcc_other_cd.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设管理费—项目管理经费":
                        pevm.EstimateNumber = catagorySet.pcc_other_xmgl.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设管理费—项目管理经费—其中：业务招待费":
                        pevm.EstimateNumber = catagorySet.pcc_other_zd.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设管理费—招标费":
                        pevm.EstimateNumber = catagorySet.pcc_other_zb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设管理费—工程监理费":
                        pevm.EstimateNumber = catagorySet.pcc_other_gcjl.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—工程勘察费":
                        pevm.EstimateNumber = catagorySet.pcc_other_kc.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—工程设计费":
                        pevm.EstimateNumber = catagorySet.pcc_other_sj.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—设计文件评审费":
                        pevm.EstimateNumber = catagorySet.pcc_other_ps.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—项目后评价费":
                        pevm.EstimateNumber = catagorySet.pcc_other_hpj.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—技术经济标准编制管理费":
                        pevm.EstimateNumber = catagorySet.pcc_other_bzbz.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—工程建设监督检测费":
                        pevm.EstimateNumber = catagorySet.pcc_other_jdjc.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—生产准备费":
                        pevm.EstimateNumber = catagorySet.pcc_other_sczb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—基本预备费":
                        pevm.EstimateNumber = catagorySet.pcc_other_jbyb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—建设期贷款利息":
                        pevm.EstimateNumber = catagorySet.pcc_other_dklx.costValue.ToString();
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("错误：未能识别的费用类别——" + pevm.ExpanseCategory+pevm.WBSCode);
                        break;

                }
            }

        }
    }
}