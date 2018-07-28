using System.Reflection;
using FirstFloor.ModernUI.Presentation;
using System.Collections.ObjectModel;
using System.Data;
using System;
using System.Collections.Generic;
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
                    if (vm is ProjectTotalEstimateViewModel) { continue; }
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
                    if (vm is ProjectTotalEstimateViewModel) { continue; }
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

            string temp = TotalEstimateViewModel.TotalInvestmentWithTax;
            //System.Windows.MessageBox.Show(MinWithoutRate().ToString()+" "+MaxWithoutRate().ToString());
        }

        private double GetCompositeTaxRate()
        {
            return (this.TotalInvestmentWithTax / this.TotalInvestmentWithoutTax) - 1;
        }

        private double GetDestNumber(double CompositeTaxRate)
        {
            return this.TotalInvestmentWithTax / (1 + CompositeTaxRate / 100);
        }

        private ProjectEstimateViewModel GetProjectInSetByCategoryName(string category)
        {
            foreach (ProjectEstimateViewModel pvm in EstimateViewModels)
            {
                if (pvm is ProjectTotalEstimateViewModel) continue;
                if (pvm.ExpanseCategory.Contains(category))
                { return pvm; }
            }
            return null;
        }

        private double MinWithoutRate()
        {
            double d = 0;
            foreach (ProjectEstimateViewModel pvm in EstimateViewModels)
            {
                if (pvm is ProjectTotalEstimateViewModel) continue;
                d = d +Convert.ToDouble(pvm.TotalInvestmentWithTax) / (1 + Convert.ToDouble(pvm.MaxDeductibleVATRatio));
            }
            return d;
        }

        private double MaxWithoutRate()
        {
            double d = 0;
            foreach (ProjectEstimateViewModel pvm in EstimateViewModels)
            {
                if (pvm is ProjectTotalEstimateViewModel) continue;
                d = d + Convert.ToDouble(pvm.TotalInvestmentWithTax) / (1 + Convert.ToDouble(pvm.MinDeductibleVATRatio));
            }
            return d;
        }

        private bool CheckCanDo(double DestCompositeTaxRate)
        {
            double dest = GetDestNumber(DestCompositeTaxRate);
            if (dest > MaxWithoutRate()) return false;
            if (dest < MinWithoutRate()) return false;
            return true;
        }

        //计算至目标概算
        public void SetToDestCompositeTaxRate(double DestCompositeTaxRate)
        {
            if (!CheckCanDo(DestCompositeTaxRate)) return;
            List<string> strlist = new List<string> { "架空线路本体工程", "电缆本体工程", "建设场地征用及清理费", "项目管理经费",
                "业务招待费", "招标费", "工程监理费", "工程勘察费", "工程设计费", "设计文件评审费", "项目后评价费","技术经济标准编制管理费","工程建设监督检测费" ,
                "配电站（开关站）工程—建筑工程","配电站（开关站）工程—安装工程","配电站（开关站）工程—设备购置","通信及调度自动化—建筑工程","通信及调度自动化—安装工程",
                "通信及调度自动化—设备购置","生产准备费","基本预备费","建设期贷款利息"};
            double destNumber = GetDestNumber(DestCompositeTaxRate);
            double totalWithoutTax = this.TotalInvestmentWithoutTax;
            double delta = destNumber - totalWithoutTax;
            if (delta>0)
            {
                foreach (string str in strlist)
                {
                    ProjectEstimateViewModel pvm = GetProjectInSetByCategoryName(str);
                    if (pvm == null) continue;
                    double ldelta = Convert.ToDouble(pvm.TotalInvestmentWithTax) / (1 + Convert.ToDouble(pvm.MinDeductibleVATRatio));
                    ldelta = ldelta - Convert.ToDouble(pvm.TotalInvestmentWithoutTax);
                    if (ldelta >= delta)
                    {
                        double d_without =Convert.ToDouble( pvm.TotalInvestmentWithoutTax);
                        double d_with =Convert.ToDouble(pvm.TotalInvestmentWithTax);
                        double destrate = (d_with / (d_without + delta)) - 1;
                        destrate = destrate * 100;
                        pvm.DeductibleVATRatio = destrate.ToString();
                        break;
                    }
                    else
                    {
                        pvm.DeductibleVATRatio =(Convert.ToDouble(pvm.MinDeductibleVATRatio)*100).ToString();
                        delta = destNumber - this.TotalInvestmentWithoutTax; 
                    }
                }
            }
            else if (delta<0)
            {
                foreach (string str in strlist)
                {
                    ProjectEstimateViewModel pvm = GetProjectInSetByCategoryName(str);
                    if (pvm == null) continue;
                    double ldelta = Convert.ToDouble(pvm.TotalInvestmentWithTax) / (1 + Convert.ToDouble(pvm.MaxDeductibleVATRatio));
                    ldelta = ldelta - Convert.ToDouble(pvm.TotalInvestmentWithoutTax);
                    if (ldelta <= delta)
                    {
                        double d_without = Convert.ToDouble(pvm.TotalInvestmentWithoutTax);
                        double d_with = Convert.ToDouble(pvm.TotalInvestmentWithTax);
                        double destrate = (d_with / (d_without + delta)) - 1;
                        destrate = destrate * 100;
                        pvm.DeductibleVATRatio = destrate.ToString();
                        break;
                    }
                    else
                    {
                        pvm.DeductibleVATRatio = (Convert.ToDouble(pvm.MaxDeductibleVATRatio) * 100).ToString();
                        delta = destNumber - this.TotalInvestmentWithoutTax; 
                    }
                }

            }

            string temp = TotalEstimateViewModel.TotalInvestmentWithTax;
        }
    }
}