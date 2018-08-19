using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;
using CaoJin.HNFinanceTool.Basement;
using System.Data;

namespace CaoJin.HNFinanceTool.Bll
{
   public class ProjectImportMouldViewModel:NotifyPropertyChanged
    {
        public ProjectImportMouldViewModel()
        {
            
        }

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value;OnPropertyChanged("ProjectName"); }
        }

        private string _projectCode;
        public string ProjectCode
        {
            get { return _projectCode; }
            set { _projectCode = value;OnPropertyChanged("ProjectCode"); }
        }

        private string _individualProjectName;
        public string IndividualProjectName
        {
            get { return _individualProjectName; }
            set { _individualProjectName = value;OnPropertyChanged("IndividualProjectName"); }
        }

        private string _individualProjectCode;
        public string IndividualProjectCode
        {
            get { return _individualProjectCode; }
            set { _individualProjectCode = value;OnPropertyChanged("IndividualProjectCode"); }
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

        private string _constructionStage="新建";
        public string ConstructionStage
        {
            get { return _constructionStage; }
            set { _constructionStage = value;OnPropertyChanged("ConstructionStage"); }
        }

        private string _prestandardVersion= "13/15版本";
        public string PrestandardVersion
        {
            get { return _prestandardVersion; }
            set { _prestandardVersion = value;OnPropertyChanged("PrestandardVersion"); }
        }

        private string _vlevel = "10kV（含20kV）及以下项目";
        public string VLevel
        {
            get { return _vlevel; }
            set { _vlevel = value;OnPropertyChanged("VLevel"); }
        }

        private string _singleProjectClass;
        public string SingleProjectClass
        {
            get { return _singleProjectClass; }
            set { _singleProjectClass = value;OnPropertyChanged("SingleProjectClass"); }
        }

        private string _totalInvestmentWithTax;
        public string TotalInvestmentWithTax
        {
            get { return _totalInvestmentWithTax; }
            set { _totalInvestmentWithTax = value;OnPropertyChanged("TotalInvestmentWithTax"); }
        }

        private string _totalInvestmentWithoutTax;
        public string TotalInvestmentWithoutTax
        {
            get { return _totalInvestmentWithoutTax; }
            set { _totalInvestmentWithoutTax = value;OnPropertyChanged("TotalInvestmentWithoutTax"); }
        }

        //工程成本累计已发生
        private string _cumulativeCost;
        public string CumulativeCost
        {
            get { return _cumulativeCost; }
            set { _cumulativeCost = value;OnPropertyChanged("CumulativeCost"); }
        }

        //本年成本已发生
        private string _yearCost;
        public string YearCost
        {
            get { return _yearCost; }
            set { _yearCost = value;OnPropertyChanged("YearCost"); }
        }

        //累计抵扣增值税
        private string _cumulativeDeductibleVAT;
        public string CumulativedeDeductibleVAT
        {
            get { return _cumulativeDeductibleVAT; }
            set { _cumulativeDeductibleVAT = value;OnPropertyChanged("CumulativeDeductibleVAT"); }
        }

        //本年抵扣增值税
        private string _yearDeductibleVAT;
        public string YearDeductibleVAT
        {
            get { return _yearDeductibleVAT; }
            set { _yearDeductibleVAT = value;OnPropertyChanged("YearDeductibleVAT"); }
        }

        private string _deductibleVATRatio;//可抵扣增值税比例
        public string DeductibleVATRatio
        {
            get { return _deductibleVATRatio; }
            set { _deductibleVATRatio = value; }
        }
        public double deductibleVATRatio
        {
            get
            {
                string temp = _deductibleVATRatio.Replace("%","");
                double dou = 0;
                try
                {
                    dou = Convert.ToDouble(temp);
                }
                catch (Exception)
                { }
                return dou;
            }

        }

        //年度投资预算含税
        private double _departmentFilledBudgetWithTax;
        public double DepartmentFilledBudgetWithTax
        {
            get { return _departmentFilledBudgetWithTax; }
            set { _departmentFilledBudgetWithTax = value; OnPropertyChanged("DepartmentFilledBudgetWithTax"); }
        }


        //年度投资预算不含税
        private double _yearBudgetWithoutTax;
        public double YearBudgetWithoutTax
        {
            get { return _yearBudgetWithoutTax; }
            set { _yearBudgetWithoutTax = value;OnPropertyChanged("YearBudgetWithoutTax"); }
        }

        public void GetData(ProjectEstimateViewModel project)
        {

            if (project is ProjectTotalEstimateViewModel)
            {
                this.ProjectCode = ((ProjectTotalEstimateViewModel)project).ProjectCode;
                this.ProjectName = ((ProjectTotalEstimateViewModel)project).ProjectName;
                this.IndividualProjectCode = ((ProjectTotalEstimateViewModel)project).IndividualProjectCode;
                this.IndividualProjectName = ((ProjectTotalEstimateViewModel)project).IndividualProjectName;
                this.WBSCode = ((ProjectTotalEstimateViewModel)project).WBSCode;
                this.ExpanseCategory = ((ProjectTotalEstimateViewModel)project).ExpanseCategory;
                this.TotalInvestmentWithTax = ((ProjectTotalEstimateViewModel)project).TotalInvestmentWithTax;
                this.TotalInvestmentWithoutTax = ((ProjectTotalEstimateViewModel)project).TotalInvestmentWithoutTax;
               // DepartmentBudgetFilled department = new DepartmentBudgetFilled(ProjectName);
               // this.DeductibleVATRatio = ((ProjectTotalEstimateViewModel)project).DeductibleVATRatio;
               // this.DepartmentFilledBudgetWithTax = department.DepartmentFilledBudgetWithTax;
               // this.YearBudgetWithoutTax = department.YearBudgetWithoutTax;
            }
            else
            {
                this.ProjectCode = project.ProjectCode;
                this.ProjectName = project.ProjectName;
                this.IndividualProjectCode = project.IndividualProjectCode;
                this.IndividualProjectName = project.IndividualProjectName;
                this.ExpanseCategory = project.ExpanseCategory;
                this.WBSCode = project.WBSCode;
                this.TotalInvestmentWithTax = project.TotalInvestmentWithTax;
                this.TotalInvestmentWithoutTax = project.TotalInvestmentWithoutTax;
                this.DeductibleVATRatio = project.DeductibleVATRatio;

            }

        }

    }
}
