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
        }

        public new string InternalControl
        {
            get { return null; }
        }

        public new string MaxDeductibleVATRatio
        {
            get { return null; }
        }

        public new string MinDeductibleVATRatio
        { get { return null; } }

        public new string EstimateNumber
        {
            get;
            set;
        }

        public new string DeductibleVATRatio
        {
            get { return null; }
        }

        public new string TotalInvestmentWithTax
        {
            get;
            set;
        }

        public new string TotalInvestmentWithoutTax
        {
            get;
            set;
        }
    }
}
