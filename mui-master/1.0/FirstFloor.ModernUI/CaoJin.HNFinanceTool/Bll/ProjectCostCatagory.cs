using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoJin.HNFinanceTool.Bll
{
   public class ProjectCostCatagory
    {
        public ProjectCostCatagory()
        { }
        public ProjectCostCatagory(string name)
        {
            catagoryName = name;
        }

        public ProjectCostCatagory(string name,double cvalue)
        {
            this.catagoryName = name;
            this.costValue = cvalue;
        }
       public string catagoryName;
       public double costValue=0;
    }
}
