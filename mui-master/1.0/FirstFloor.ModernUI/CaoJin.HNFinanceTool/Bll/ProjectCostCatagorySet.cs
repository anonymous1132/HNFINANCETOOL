using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoJin.HNFinanceTool.Bll
{
   public class ProjectCostCatagorySet
    {
        public ProjectCostCatagorySet()
        { }

        //计算出的总价
        public ProjectCostCatagory pcc_SUM
        { get { return new ProjectCostCatagory("总价",SUM()); } }

        //表格获取的合计价格
        public ProjectCostCatagory pcc_all = new ProjectCostCatagory("合计");

        //配电-建筑
        private ProjectCostCatagory _pcc_pd_jz = new ProjectCostCatagory("配电-建筑");
        public ProjectCostCatagory pcc_pd_jz
        {
            get { return _pcc_pd_jz; }
            set { _pcc_pd_jz = value; }
        }
        //配电-安装
        private ProjectCostCatagory _pcc_pd_az = new ProjectCostCatagory("配电-安装");
        public ProjectCostCatagory pcc_pd_az
        {
            get { return _pcc_pd_az; }
            set { _pcc_pd_az = value;}
        }
        //配电-设备购置
        private ProjectCostCatagory _pcc_pd_sb = new ProjectCostCatagory("配电-设备购置");
        public ProjectCostCatagory pcc_pd_sb
        {
            get { return _pcc_pd_sb; }
            set
            {
                _pcc_pd_sb = value;
            }
        }
        //通信-建筑
        private ProjectCostCatagory _pcc_tx_jz = new ProjectCostCatagory("通信-建筑");
        public ProjectCostCatagory pcc_tx_jz
        {
            get { return _pcc_tx_jz; }
            set { _pcc_tx_jz = value; }
        }
        //通信-安装
        private ProjectCostCatagory _pcc_tx_az = new ProjectCostCatagory("通信-安装");
        public ProjectCostCatagory pcc_tx_az
        {
            get { return _pcc_tx_az; }
            set { _pcc_tx_az = value; }
        }
        //通信-设备购置
        private ProjectCostCatagory _pcc_tx_sb = new ProjectCostCatagory("通信-设备购置");
        public ProjectCostCatagory pcc_tx_sb
        {
            get { return _pcc_tx_sb; }
            set { _pcc_tx_sb = value; }
        }
        //架空
        private ProjectCostCatagory _pcc_jk = new ProjectCostCatagory("架空");
        public ProjectCostCatagory pcc_jk
        {
            get { return _pcc_jk; }
            set { _pcc_jk = value; }
        }
        //电缆
        private ProjectCostCatagory _pcc_dl = new ProjectCostCatagory("电缆");
        public ProjectCostCatagory pcc_dl
        {
            get { return _pcc_dl; }
            set { _pcc_dl = value; }
        }
        //其他-场地
        private ProjectCostCatagory _pcc_other_cd = new ProjectCostCatagory("其他-场地");
        public ProjectCostCatagory pcc_other_cd
        {
            get { return _pcc_other_cd; }
            set { _pcc_other_cd = value; }
        }
        //其他-项目管理
        private ProjectCostCatagory _pcc_other_xmgl = new ProjectCostCatagory("其他-项目管理");
        public ProjectCostCatagory pcc_other_xmgl
        {
            get { return _pcc_other_xmgl; }
            set { _pcc_other_xmgl = value; }
        }
        //其他-招待费
        private ProjectCostCatagory _pcc_other_zd = new ProjectCostCatagory("其他-招待费");
        public ProjectCostCatagory pcc_other_zd
        {
            get { return _pcc_other_zd; }
            set { _pcc_other_zd = value; }
        }
        //其他-招标
        private ProjectCostCatagory _pcc_other_zb = new ProjectCostCatagory("其他-招标");
        public ProjectCostCatagory pcc_other_zb
        {
            get { return _pcc_other_zb; }
            set { _pcc_other_zb = value; }
        }
        //其他_工程监理
        private ProjectCostCatagory _pcc_other_gcjl = new ProjectCostCatagory("其他-工程监理");
        public ProjectCostCatagory pcc_other_gcjl
        {
            get { return _pcc_other_gcjl; }
            set { _pcc_other_gcjl = value; }
        }
        //其他-工程勘察
        private ProjectCostCatagory _pcc_other_kc = new ProjectCostCatagory("其他-工程勘察");
        public ProjectCostCatagory pcc_other_kc
        {
            get { return _pcc_other_kc; }
            set { _pcc_other_kc = value; }
        }
        //其他-设计
        private ProjectCostCatagory _pcc_other_sj = new ProjectCostCatagory("其他-设计");
        public ProjectCostCatagory pcc_other_sj
        {
            get { return _pcc_other_sj; }
            set { _pcc_other_sj = value; }
        }
        //其他-设计文件评审
        private ProjectCostCatagory _pcc_other_ps = new ProjectCostCatagory("其他-设计文件评审");
        public ProjectCostCatagory pcc_other_ps
        {
            get { return _pcc_other_ps; }
            set { _pcc_other_ps = value;}
        }
        //其他-项目后评价费
        private ProjectCostCatagory _pcc_other_hpj = new ProjectCostCatagory("其他-项目后评价");
        public ProjectCostCatagory pcc_other_hpj
        {
            get { return _pcc_other_hpj; }
            set { _pcc_other_hpj = value;}
        }
        //其他-技术经济标准编制管理费
        private ProjectCostCatagory _pcc_other_bzbz = new ProjectCostCatagory("其他-技术经济标准编制管理费");
        public ProjectCostCatagory pcc_other_bzbz
        {
            get { return _pcc_other_bzbz; }
            set { _pcc_other_bzbz = value; }
        }
        //其他-工程建设监督检测费
        private ProjectCostCatagory _pcc_other_jdjc = new ProjectCostCatagory("其他-工程建设监督检测费");
        public ProjectCostCatagory pcc_other_jdjc
        {
            get { return _pcc_other_jdjc; }
            set { _pcc_other_jdjc = value; }
        }
        //其他费用—生产准备费
        private ProjectCostCatagory _pcc_other_sczb = new ProjectCostCatagory("其他费用—生产准备费");
        public ProjectCostCatagory pcc_other_sczb
        {
            get { return _pcc_other_sczb; }
            set { _pcc_other_sczb = value; }
        }
        //其他费用—基本预备费
        private ProjectCostCatagory _pcc_other_jbyb = new ProjectCostCatagory("其他费用—基本预备费");
        public ProjectCostCatagory pcc_other_jbyb
        {
            get { return _pcc_other_jbyb; }
            set { _pcc_other_jbyb = value; }
        }
        //建设期贷款利息
        private ProjectCostCatagory _pcc_other_dklx = new ProjectCostCatagory("建设期贷款利息");
        public ProjectCostCatagory pcc_other_dklx
        {
            get { return _pcc_other_dklx; }
            set { _pcc_other_dklx = value; }
        }

        public ProjectCostCatagory pcc_weicha
        {
            get { return new ProjectCostCatagory("尾差",Deriver()); }
        }


        private double SUM()
        {
            return _pcc_pd_az.costValue + _pcc_pd_jz.costValue + _pcc_pd_sb.costValue + _pcc_tx_az.costValue + _pcc_tx_jz.costValue + _pcc_tx_sb.costValue
                + _pcc_jk.costValue + _pcc_dl.costValue + _pcc_other_bzbz.costValue + _pcc_other_cd.costValue + _pcc_other_dklx.costValue + _pcc_other_gcjl.costValue + _pcc_other_hpj.costValue
                + _pcc_other_jbyb.costValue + _pcc_other_jdjc.costValue + _pcc_other_kc.costValue + _pcc_other_ps.costValue + _pcc_other_sczb.costValue + _pcc_other_sj.costValue + _pcc_other_xmgl.costValue
                + _pcc_other_zb.costValue + _pcc_other_zd.costValue;
        }

        private double Deriver()
        {
            return pcc_all.costValue - pcc_SUM.costValue;
        }
    }
}
