using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoJin.HNFinanceTool.Bll
{
   public class ProjectClass
    {
        public ProjectClass()
        { }
        //工程名称
        public string ProjectName;
        //工程编号
        public string ProjectCode;

    }

    public enum FinanceCatagory
    {
        建筑工程费,
        设备购置费,
        安装工程费,
        其他费用,
        合计
    }

    public enum ProjectCatagory
    {
        配电站工程,
        通信及调度自动化,
        架空线路工程,
        电缆线路工程,

    }
}
