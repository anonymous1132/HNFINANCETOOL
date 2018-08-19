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

        private double _estimateNumber=0;
        public new string EstimateNumber
        {
            get { return _estimateNumber.ToString("N"); }

            set
            {
                try

                {
                    double test = Convert.ToDouble((((string)value)).Trim());
                    _estimateNumber = test;
                    OnPropertyChanged("EstimateNumber");
                }
                catch (Exception)
                { return; }
            }
        }

        public new string DeductibleVATRatio
        {
            get { return null; }
            set { return; }
        }

        private double _totalInvestmentWithTax;
        public new string TotalInvestmentWithTax
        {
            get { return _totalInvestmentWithTax.ToString("N"); }
            set
            {
                double test = Convert.ToDouble((((string)value)).Trim());
                _totalInvestmentWithTax=test;
                OnPropertyChanged("TotalInvestmentWithTax");
            }
        }

        private double _totalInvestmentWithoutTax;
        public new string TotalInvestmentWithoutTax
        {
            get { return _totalInvestmentWithoutTax.ToString("N"); }
            set
            {
                double test = Convert.ToDouble((((string)value)).Trim());
                _totalInvestmentWithoutTax = test;
                OnPropertyChanged("TotalInvestmentWithoutTax");
            }
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
