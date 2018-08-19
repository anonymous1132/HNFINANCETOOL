using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoJin.HNFinanceTool.Bll
{
   public class ProjectTotalEstimateViewModel:ProjectEstimateViewModel
    {
        public ProjectTotalEstimateViewModel()
        { }

        public new string MaxInternalControl
        {
            get { return null; }
            set { return; }
        }

        public new string InternalControl
        {
            get { return null; }
            set { return; }
        }

        public new string MaxDeductibleVATRatio
        {
            get { return null; }
            set { return; }
        }

        public new string MinDeductibleVATRatio
        { get { return null; }
            set { return; }
        }

        private string _estimateNumber;
        public new string EstimateNumber
        {
            get { return _estimateNumber; }
            set { _estimateNumber=value;OnPropertyChanged("EstimateNumber"); }
        }

        public new string DeductibleVATRatio
        {
            get { return null; }
            set { return; }
        }

        private string _totalInvestmentWithTax;
        public new string TotalInvestmentWithTax
        {
            get { return _totalInvestmentWithTax; }
            set { _totalInvestmentWithTax = value; OnPropertyChanged("TotalInvestmentWithTax"); }
        }

        private string _totalInvestmentWithoutTax;
        public new string TotalInvestmentWithoutTax
        {
            get { return _totalInvestmentWithoutTax; }
            set { _totalInvestmentWithoutTax = value;OnPropertyChanged("TotalInvestmentWithoutTax"); }
        }

        public new string ExpanseCategory
        {
            get { return "10KV（含20KV）及以下基建项目"; }
            set { return; }
        }

        public new string WBSCode
        {
            get { return "A0000000"; }
            set { return; }
        }

        public new string IndividualProjectCode
        {
            get { return ""; }
            set { return; }
        }

        public new string IndividualProjectName
        {
            get { return ""; }
            set { return; }
        }

       
    }
}
