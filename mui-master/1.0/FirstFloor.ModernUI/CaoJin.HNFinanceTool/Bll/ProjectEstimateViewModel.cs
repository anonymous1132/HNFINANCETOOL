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
        private string _id;
        public string  id
        {
            get { return _id; }
            set { _id = value;OnPropertyChanged("id"); }
        }
        public int ID
        {
            get { return Convert.ToInt32(_id); }
        }

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


        private double _maxInternalControl = 1;//内控系数上限
        public string MaxInternalControl
        {
            get { return _maxInternalControl.ToString(); }
            set
            {
               // if (ID == 0) { _maxInternalControl = null; return; }
                try
                {
                    double test = Convert.ToDouble(((string)value).Trim());
                    if (test >= _internalControl)
                    {
                        _maxInternalControl = test;
                        OnPropertyChanged("MaxInternalControl");
                    }
                }
                catch (Exception) { return; }
            }
        }

        private double _maxDeductibleVATRatio = 0.17;//可抵扣增值税比例上限
        public string MaxDeductibleVATRatio
        {
            get { return _maxDeductibleVATRatio.ToString(); }
            set
            {

                try
                {
                    double test = Convert.ToDouble(((string)value).Trim());
                    if (test >= _deductibleVATRatio / 100)
                    {
                        _maxDeductibleVATRatio = test;
                        OnPropertyChanged("MaxDeductibleVATRatio");
                    }
                }
                catch (Exception ) {  return; }
            }
        }

        private double _minDeductibleVATRatio=0;//可抵扣增值税比例下限
        public string MinDeductibleVATRatio
        {
            get { return _minDeductibleVATRatio.ToString(); }
            set
            {

                try
                {
                    double test = Convert.ToDouble(((string)value).Trim());
                    if (test <= _deductibleVATRatio/100)
                    {
                        _minDeductibleVATRatio = test;
                        OnPropertyChanged("MinDeductibleVATRatio");
                    }
                }
                catch (Exception ) { return; }
       
            }
        }


        //概算数
        public string EstimateNumber

        {

            get { return _estimateNumber.ToString("N"); }

            set
            {
                try

                {
                    double test = Convert.ToDouble((((string)value)).Trim());
                    _estimateNumber = test;
                    _totalInvestmentWithTax = test * _internalControl;
                    _totalInvestmentWithoutTax = _totalInvestmentWithTax / (1 + _deductibleVATRatio / 100);
                    OnPropertyChanged("EstimateNumber");
                    OnPropertyChanged("TotalInvestmentWithTax");
                    OnPropertyChanged("TotalInvestmentWithoutTax");
                }
                catch (Exception )
                {  return; }
            }

        }
        
        private double _estimateNumber = 0;



        private double _internalControl=0;//内控系数

        public string InternalControl

        {

            get { return _internalControl.ToString(); }

            set
            {
               // if (ID == 0) { _internalControl = null; return; }
                try
                {
                    double test = Convert.ToDouble(((string)value).Trim());
                    if(test<_maxInternalControl)
                    {
                        _internalControl = test;
                        _totalInvestmentWithTax = test * _estimateNumber;
                        _totalInvestmentWithoutTax = _totalInvestmentWithTax / (1 + _deductibleVATRatio / 100);
                        OnPropertyChanged("InternalControl");
                        OnPropertyChanged("TotalInvestmentWithTax");
                        OnPropertyChanged("TotalInvestmentWithoutTax");
                    }
                    
                }
                catch (Exception )
                {  return; }
            }

        }

        private double _deductibleVATRatio=0;//可抵扣增值税比例
        public string DeductibleVATRatio
        {
            get {
               // if (ID == 0) return null;
                return _deductibleVATRatio.ToString()+"%";
            }
            set
            {
               // if (ID == 0) { _deductibleVATRatio = null; return; }
                try
                {
                    double test = Convert.ToDouble(((string)value).Replace("%", "").Trim());
                    if (test/100 <= _maxDeductibleVATRatio && test/100 >= _minDeductibleVATRatio)
                    {
                        _deductibleVATRatio = test;
                        _totalInvestmentWithoutTax = _totalInvestmentWithTax / (1 + test / 100);
                        OnPropertyChanged("DeductibleVATRatio");
                        OnPropertyChanged("TotalInvestmentWithoutTax");
                    }
                }
                catch (Exception )
                { return; }
            }
        }

        private double _totalInvestmentWithTax=0;//总投资预算（含税）
        public string TotalInvestmentWithTax
        {
            get { return _totalInvestmentWithTax.ToString("N"); }
        }
        public double totalInvestmentWithTax
        {
            get { return _totalInvestmentWithTax; }
        }
        private double _totalInvestmentWithoutTax=0;//总投资预算（不含税）
        public string TotalInvestmentWithoutTax
        {
            get { return _totalInvestmentWithoutTax.ToString("N"); }

        }
        public double totalInvestmentWithoutTax
        {
            get { return _totalInvestmentWithoutTax; }
        }


    }

}