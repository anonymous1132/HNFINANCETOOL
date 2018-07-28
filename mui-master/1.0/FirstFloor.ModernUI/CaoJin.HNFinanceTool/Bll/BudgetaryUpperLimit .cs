using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;

namespace CaoJin.HNFinanceTool.Bll
{
    public class BudgetaryUpperLimit : NotifyPropertyChanged
    {
        public BudgetaryUpperLimit(string projectName)
        { this._projectName = projectName; }
        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; OnPropertyChanged("ProjectName"); }
        }

        private string _projectCode;
        public string ProjectCode
        {
            get { return _projectCode; }
            set { _projectCode = value; OnPropertyChanged("ProjectCode"); }
        }

        private double _estimateNumber = 0;
        public double EstimateNumber
        {
            get
            {
                return _estimateNumber;
            }
            set
            {
                _estimateNumber = value; OnPropertyChanged("EstimateNumber");
            }
        }

        private string _internalControl;
        public string InternalControl
        {
            get { return _internalControl; }
            set { _internalControl = value; OnPropertyChanged("InternalControl"); }
        }

        private double _totalInvestmentWithTax = 0;
        public double TotalInvestmentWithTax
        {
            get { return _totalInvestmentWithTax; }
            set { _totalInvestmentWithTax = value; OnPropertyChanged("TotalInvestmentWithTax");MaxBudgetWithTax = WhichMin(_totalInvestmentWithTax, _accumulativePlan) - _erpHappenedWithoutTax - _deductibleVAT; }
        }

        private double _totalInvestmentWithoutTax = 0;
        public double TotalInvestmentWithoutTax
        {
            get { return _totalInvestmentWithoutTax; }
            set { _totalInvestmentWithoutTax = value;OnPropertyChanged("TotalInvestmentWithoutTax");MaxBudgetWithoutTax = _totalInvestmentWithoutTax - _erpHappenedWithoutTax; }
        }

        //累计综合计划下达
        private double _accumulativePlan = 0;
        public double AccumulativePlan
        {
            get { return _accumulativePlan; }
            set { _accumulativePlan = value;OnPropertyChanged("AccumulativePlan"); MaxBudgetWithTax = WhichMin(_totalInvestmentWithTax, _accumulativePlan) - _erpHappenedWithoutTax - _deductibleVAT; }
        }

        //截至上年ERP已发生（不含税
        private double _erpHappenedWithoutTax = 0;
        public double ErpHappenedWithoutTax
        {
            get { return _erpHappenedWithoutTax; }
            set { _erpHappenedWithoutTax = value;OnPropertyChanged("ErpHappenedWithoutTax"); MaxBudgetWithTax = WhichMin(_totalInvestmentWithTax, _accumulativePlan) - _erpHappenedWithoutTax - _deductibleVAT; MaxBudgetWithoutTax = _totalInvestmentWithoutTax - _erpHappenedWithoutTax; }
        }

        //截至上年累计已抵扣增值税
        private double _deductibleVAT = 0;
        public double DeductibleVAT
        {
            get { return _deductibleVAT; }
            set { _deductibleVAT = value;OnPropertyChanged("DeductibleVAT"); MaxBudgetWithTax = WhichMin(_totalInvestmentWithTax, _accumulativePlan) - _erpHappenedWithoutTax - _deductibleVAT; }
        }
        //本年预算可发生最大数（含税）
        private double _maxBudgetWithTax = 0;
        public double MaxBudgetWithTax
        {
            get { return _maxBudgetWithTax; }
            set { _maxBudgetWithTax = value;OnPropertyChanged("MaxBudgetWithTax"); }
        }

        //本年预算可发生最大数（不含税）
        private double _maxBudgetWithoutTax = 0;
        public double MaxBudgetWithoutTax
        {
            get { return _maxBudgetWithoutTax; }
            set { _maxBudgetWithoutTax = value;OnPropertyChanged("MaxBudgetWithoutTax"); }
        }

        private double WhichMin(double x, double y)
        {
            return x < y ? x : y;
        }

        public void GetData()
        {
            string path = "App\\data\\"+this.ProjectName;
           // ProjectEstimateSetViewModel projectEstimateSet = new ProjectEstimateSetViewModel();
        }
    }
}
