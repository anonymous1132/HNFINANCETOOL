﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;

namespace CaoJin.HNFinanceTool.Bll
{
   public class TailDifferenceViewModel:NotifyPropertyChanged
    {
        public TailDifferenceViewModel()
        { }

        private string _tailDifference;
        public string TailDifference
        {
            get { return _tailDifference; }
            set { _tailDifference = value;OnPropertyChanged(TailDifference); }
        }

        private string _itemWithTailDifference;
        public string ItemWithTailDifference
        {
            get { return _itemWithTailDifference; }
            set { _itemWithTailDifference = value;OnPropertyChanged("ItemWithTailDifference"); }
        }

        private string _compositeTaxRate;
        public string CompositeTaxRate
        {
            get { return _compositeTaxRate; }
            set
            {
                if (((string)value).Substring(value.Length - 1) != "%")
                {
                    return;
                }
                try
                {
                    double test = Convert.ToDouble(((string)value).Substring(0,value.Length - 1));
                    _double_compositeTaxRate = test;
                    _compositeTaxRate = value;
                    OnPropertyChanged(CompositeTaxRate);
                }
                catch (Exception)
                { return; }
            }
        }

        private double _double_compositeTaxRate;
        public double Double_CompositeTaxRate
        {
            get { return _double_compositeTaxRate; }
        }
    }
}
