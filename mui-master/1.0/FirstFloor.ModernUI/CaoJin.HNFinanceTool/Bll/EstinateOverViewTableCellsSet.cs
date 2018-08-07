using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoJin.HNFinanceTool.Bll
{
   public class EstinateOverViewTableCellsSet
    {
        public EstinateOverViewTableCellsSet()
        { }
        public EstinateOverViewTableCell PDZ_Cell = new EstinateOverViewTableCell("配电站");
        public EstinateOverViewTableCell TXAuto_Cell = new EstinateOverViewTableCell("通信及调度自动化");
        public EstinateOverViewTableCell JKXL_Cell = new EstinateOverViewTableCell("架空线路");
        public EstinateOverViewTableCell DLXL_Cell = new EstinateOverViewTableCell("电缆线路");
        public EstinateOverViewTableCell NJC_Cell = new EstinateOverViewTableCell("当地编制年价差");
        public EstinateOverViewTableCell Other_Cell_Y = new EstinateOverViewTableCell("其他费用");
        public EstinateOverViewTableCell JBYB_Cell = new EstinateOverViewTableCell("基本预备费");
        public EstinateOverViewTableCell DKLX_Cell = new EstinateOverViewTableCell("贷款利息");
        public EstinateOverViewTableCell Other_JSCDQL_Cell = new EstinateOverViewTableCell("建设场地征用及清理费");
        public EstinateOverViewTableCell Other_SCZB_DL_Cell = new EstinateOverViewTableCell("生产准备费：电缆工程");
        public EstinateOverViewTableCell Other_SCZB_FDL_Cell = new EstinateOverViewTableCell("生产准备费：非电缆工程");
        public EstinateOverViewTableCell Other_SCZB_Cell = new EstinateOverViewTableCell("生产准备费");
        public EstinateOverViewTableCell GCDT_Cell = new EstinateOverViewTableCell("工程动态投资");

        public EstinateOverViewTableCell JZGC_Cell = new EstinateOverViewTableCell("建筑工程费");
        public EstinateOverViewTableCell SBGZ_Cell = new EstinateOverViewTableCell("设备购置费");
        public EstinateOverViewTableCell AZGC_Cell = new EstinateOverViewTableCell("安装工程费");
        public EstinateOverViewTableCell Other_Cell_X = new EstinateOverViewTableCell("其他费用");
        public EstinateOverViewTableCell HJ_Cell = new EstinateOverViewTableCell("合计");
    }
}
