using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CaoJin.HNFinanceTool.Dal;

namespace CaoJin.HNFinanceTool.Bll
{
   public class MouldDataSet
    {
        public static ProjectEstimateSetViewModel GetMouldModel()
        {
            string datafile = "App\\data\\mould";
            DataSet ds = XmlOperate.GetDataSet(datafile);
            DataTable dt = ds.Tables[0];
            return new ProjectEstimateSetViewModel(dt);
        }
    }
}
