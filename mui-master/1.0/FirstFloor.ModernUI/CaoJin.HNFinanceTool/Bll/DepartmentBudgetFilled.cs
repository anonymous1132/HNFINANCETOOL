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
   public class DepartmentBudgetFilled:NotifyPropertyChanged
    {
        public DepartmentBudgetFilled(string projectName)
        {
            this.ProjectName = projectName;
            GetData();
        }

        private int _numberOnly;
        public int NumberOnly
        {
            get { return _numberOnly; }
            set { _numberOnly = value;OnPropertyChanged("NumberOnly"); }
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

        private double _maxBudgetWithTax;
        public double MaxBudgetWithTax
        {
            get { return _maxBudgetWithTax; }
            set { _maxBudgetWithTax = value;OnPropertyChanged("MaxBudgetWithTax"); }
        }

        private double _maxBudgetWithoutTax;
        public double MaxBudgetWithoutTax
        {
            get { return _maxBudgetWithoutTax; }
            set { _maxBudgetWithoutTax = value;OnPropertyChanged("MaxBudgetWithoutTax"); }
        }

        private double _departmentFilledBudgetWithTax;

        public double CompositeTaxRate { get; private set; }

        public double DepartmentFilledBudgetWithTax
        {
            get { return _departmentFilledBudgetWithTax; }
            set { _departmentFilledBudgetWithTax = value;OnPropertyChanged("DepartmentFilledBudgetWithTax");this.YearBudgetWithoutTax = _departmentFilledBudgetWithTax / (1 + CompositeTaxRate / 100); OnPropertyChanged("IsYearBudgetWithTaxLegal"); OnPropertyChanged("IsUsedBelowLimit"); }
        }

        private double _yearBudgetWithoutTax;
        public double YearBudgetWithoutTax
        {
            get { return _yearBudgetWithoutTax; }
            set { _yearBudgetWithoutTax = value;OnPropertyChanged("YearBudgetWithoutTax"); OnPropertyChanged("IsYearBudgetWithoutTaxLegal"); }
        }

        public bool IsYearBudgetWithTaxLegal
        {
            get { return MaxBudgetWithTax > _departmentFilledBudgetWithTax; }
        }

        public bool IsYearBudgetWithoutTaxLegal
        {
            get { return MaxBudgetWithoutTax > _yearBudgetWithoutTax; }
        }

        //debug 20180815
        public bool IsUsedBelowLimit
        {
            get { return limit.ErpHappenedWithoutTax + limit.DeductibleVAT +DepartmentFilledBudgetWithTax <=limit.EstimateNumber; }
        }

        private BudgetaryUpperLimit limit;

        private void GetData()
        {
            string path = "App\\data\\" + this.ProjectName + ".est";
            this.limit = new BudgetaryUpperLimit(this.ProjectName);
            this.ProjectCode = limit.ProjectCode;
            this.MaxBudgetWithoutTax = limit.MaxBudgetWithoutTax;
            this.MaxBudgetWithTax = limit.MaxBudgetWithTax;
            DataTable dt = XmlHelper.GetTable(path,XmlHelper.XmlType.File, "Configure");
            this.CompositeTaxRate = GetDouble(dt.DefaultView[0]["CompositeTaxRate"].ToString().Replace("%", ""));
            dt= XmlHelper.GetTable(path, XmlHelper.XmlType.File, "DepartmentBudgetFilled");
            this.DepartmentFilledBudgetWithTax = GetDouble(dt.DefaultView[0]["DepartmentFilledBudgetWithTax"]);
        }

        public void SaveToFile()
        {
            string path = "App\\data\\" + this.ProjectName + ".est";
            XmlHelper.Update(path, "/Finance/DepartmentBudgetFilled/DepartmentFilledBudgetWithTax", "", this.DepartmentFilledBudgetWithTax.ToString());
        }

        private double GetDouble(object x)
        {
            double t = 0;
            try
            {
                t = Convert.ToDouble(x.ToString().Trim());
            }
            catch (Exception) { }

            return t;
        }
    }
}
