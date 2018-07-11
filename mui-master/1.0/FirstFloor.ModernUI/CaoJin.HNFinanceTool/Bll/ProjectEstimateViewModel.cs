using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using FirstFloor.ModernUI.Presentation;



namespace CaoJin.HNFinanceTool.Bll

{

    public class ProjectEstimateViewModel : NotifyPropertyChanged

    {

        private string _projectName; //项目名称

        public string ProjectName

        {

            get { return _projectName; }

            set { _projectName = value; OnPropertyChanged("ProjectName"); }

        }



        private string _projectCode;//项目编码

        public string ProjectCode

        {

            get { return _projectCode; }

            set { _projectCode = value; OnPropertyChanged("ProjectCode"); }

        }



        private string _individualProjectName;//单项工程名称

        public string IndividualProjectName

        {

            get { return _individualProjectName; }

            set { _individualProjectName = value; OnPropertyChanged("IndividualProjectName"); }

        }



        private string _individualProjectCode;//单项工程编码

        public string IndividualProjectCode

        {

            get { return _individualProjectCode; }

            set { _individualProjectCode = value; OnPropertyChanged("IndividualProjectCode"); }

        }



        private string _expanseCategory;//费用类别

        public string ExpanseCategory

        {

            get { return _expanseCategory; }

            set { _expanseCategory = value; OnPropertyChanged("ExpanseCategory"); }

        }



        private string _wbsCode;//wbs元素

        public string WBSCode

        {

            get { return _wbsCode; }

            set { _wbsCode = value; OnPropertyChanged("WBSCode"); }

        }



        private double _estimateNumber;//概算数

        public double EstimateNumber

        {

            get { return _estimateNumber; }

            set { _estimateNumber = value; OnPropertyChanged("EstimateNumber"); }

        }



        private double _internalControl;//内控系数

        public double InternalControl

        {

            get { return _internalControl; }

            set { _internalControl = value; OnPropertyChanged("InternalControl"); }

        }

        private string _deductibleVATRatio;//可抵扣增值税比例
        public string DeductibleVATRatio
        {
            get { return _deductibleVATRatio; }
            set { _deductibleVATRatio = value;OnPropertyChanged("DeductibleVATRatio"); }
        }

        private double _totalInvestmentWithTax;//总投资预算（含税）
        public double TotalInvestmentWithTax
        {
            get { return _totalInvestmentWithTax; }
            set { _totalInvestmentWithTax = value;OnPropertyChanged("TotalInvestmentWithTax"); }
        }
        private double _totalInvestmentWithoutTax;//总投资预算（不含税）
        public double TotalInvestmentWithoutTax
        {
            get { return _totalInvestmentWithoutTax; }
            set { _totalInvestmentWithoutTax = value;OnPropertyChanged("TotalInvestmentWithoutTax"); }
        }

        private double _maxInternalControl;//内控系数上限
        public double MaxInternalControl
        {
            get { return _maxInternalControl; }
            set { _maxInternalControl = value;OnPropertyChanged("MaxInternalControl"); }
        }

        private string _maxDeductibleVATRatio;//可抵扣增值税比例上限
        public string MaxDeductibleVATRatio
        {
            get { return _maxDeductibleVATRatio; }
            set { _maxDeductibleVATRatio = value;OnPropertyChanged("MaxDeductibleVATRatio"); }
        }

        private string _minDeductibleVATRatio;//可抵扣增值税比例下限
        public string MinDeductibleVATRatio
        {
            get { return _minDeductibleVATRatio; }
            set { _minDeductibleVATRatio = value;OnPropertyChanged("MinDeductibleVATRatio"); }
        }
    }

}