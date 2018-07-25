using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaoJin.HNFinanceTool.Bll;
using CaoJin.HNFinanceTool.Dal;
using CaoJin.HNFinanceTool.Basement;
using System.Data;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace CaoJin.HNFinanceTool.Content
{

    /// <summary>
    /// Interaction logic for ControlsStylesDataGrid.xaml
    /// </summary>
    public partial class ControlsStylesDataGrid : UserControl
    {
        public ControlsStylesDataGrid()
        {
            InitializeComponent();
        }

        public bool isadd = true;

        private string _datapath = "App\\data\\";

        public string DataFileName = "mould";

        private ObservableCollection<ProjectEstimateViewModel> financedata;

        private TailDifferenceViewModel tdvm;

        //从文件获取数据
        private void GetData(ref ObservableCollection<ProjectEstimateViewModel>financedata,ref TailDifferenceViewModel tdvm)
        {

            string datafile = _datapath + DataFileName;
            DataSet ds = XmlOperate.GetDataSet(datafile);
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            financedata = ModelConvertHelper<ProjectEstimateViewModel>.ConvertToObc(dt);
            tdvm = new TailDifferenceViewModel();
            tdvm.TailDifference = dt2.DefaultView[0]["TailDifference"].ToString();
            tdvm.ItemWithTailDifference = dt2.DefaultView[0]["ItemWithTailDifference"].ToString();
            tdvm.CompositeTaxRate = dt2.DefaultView[0]["CompositeTaxRate"].ToString();
            tdvm.AnnualPriceDifference = dt2.DefaultView[0]["AnnualPriceDifference"].ToString();
        }

        //将obc转换为dt
        private DataTable TranslateVM2DT()
        {
            DataTable dt = new DataTable("Estinates");
            dt = ModelConvertHelper<ProjectEstimateViewModel>.ConvertToDt(financedata);
            return dt;
        }

        //将taildifferencevm转换为dt
        private DataTable TranslateTDVM2DT()
        {
            DataTable dt = new DataTable("Configure");
            dt.Columns.Add("TailDifference");
            dt.Columns.Add("ItemWithTailDifference");
            dt.Columns.Add("CompositeTaxRate");
            dt.Columns.Add("AnnualPriceDifference");
            DataRow dr = dt.NewRow();
            dr["TailDifference"] = tdvm.TailDifference;
            dr["ItemWithTailDifference"] = tdvm.ItemWithTailDifference;
            dr["CompositeTaxRate"] = tdvm.CompositeTaxRate;
            dr["AnnualPriceDifference"] = tdvm.AnnualPriceDifference;
            dt.Rows.Add(dr);
            return dt;
        }
        //全置button
        private void button_allset_Click(object sender, RoutedEventArgs e)
        {
            switch (this.combobox_title.SelectedIndex)
            {
                case 0:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.ProjectName = this.textbox_setcontent.Text;
                    }
                    break;
                case 1:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.ProjectCode = this.textbox_setcontent.Text;
                    }
                    break;
                case 2:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.WBSCode = this.textbox_setcontent.Text;
                    }
                    break;
                case 3:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.InternalControl = this.textbox_setcontent.Text;
                    }
                    break;
                case 4:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.DeductibleVATRatio = this.textbox_setcontent.Text;
                    }
                    break;
                case 5:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.MaxInternalControl = this.textbox_setcontent.Text;
                    }
                    break;
                case 6:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.MaxDeductibleVATRatio = this.textbox_setcontent.Text;
                    }
                    break;
                case 7:
                    foreach (ProjectEstimateViewModel pjvm in financedata)
                    {
                        pjvm.MinDeductibleVATRatio = this.textbox_setcontent.Text;
                    }
                    break;
                default:

                    break;

            }
        }
        //保存至文件
        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            if (isadd)
            {
                string filename = string.IsNullOrEmpty(financedata[0].ProjectName.Trim()) ? "mould" : financedata[0].ProjectName.Trim() + ".est";
                string filepath = System.IO.Directory.GetCurrentDirectory() + "\\App\\data\\" + filename;
                DataSet ds = new DataSet("Finance");
                ds.Tables.Add(TranslateVM2DT());
                ds.Tables.Add(TranslateTDVM2DT());
                ds.WriteXml(filepath);
                this.DataFileName = System.IO.Path.GetFileName(filepath);
            }
            else
            {
                string datafile = _datapath + DataFileName;
                DataSet ds = new DataSet("Finance");
                ds.Tables.Add(TranslateVM2DT().Copy());
                ds.Tables.Add(TranslateTDVM2DT().Copy());
                ds.WriteXml(datafile);
            }
        }
        //导出至excel
        private void button_export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx" };
            saveFile.Title = "导出文件路径";
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            saveFile.FileName = DateTime.Now.GetDateTimeFormats('D')[0].ToString() + financedata[0].ProjectName;
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            try
            {
                if (System.IO.File.Exists(saveFile.FileName)) { System.IO.File.Delete(saveFile.FileName); }
                string mouldpath = "App\\excel\\mould1.xlsx";
                if (!System.IO.File.Exists(mouldpath)) { MessageBox.Show("错误：App\\excel\\mould1.xlsx丢失！"); return; }
                System.IO.File.Copy(mouldpath, saveFile.FileName);
                ExcelHelper exceloper = new ExcelHelper();
                exceloper.DT2Excel3(TranslateVM2DT(), saveFile.FileName);
                MessageBox.Show("操作完成！");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (isadd)
            {
                DataFileName = "mould";
            }
            GetData(ref financedata,ref tdvm);
            //Bind the DataGrid 
            this.DataContext = tdvm;
            DG1.DataContext = financedata;
            
        }

        private void button_import_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImportFile()) return;
            ManageTailDifference(ref tdvm);
            GetDataToFinanceData(proc,catagorySet,ref financedata);
            
        }

        private ProjectClass proc;
        private EstinateOverViewTableCellsSet cellsSet;
        private ProjectCostCatagorySet catagorySet;
  
        private double njc;//年价差
        private bool CheckImportFile()
        {
            OpenFileDialog openFile = new OpenFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx|Excel 97-2003 Files (*.xls)|*.xls" };
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return false;
            string filepath = openFile.FileName;
            ExcelHelper exceloper = new ExcelHelper();
            DataSet ds = exceloper.ExcelToDS(filepath);
            string tablenames = "";
            //所有表名称拼接
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                tablenames = tablenames + ds.Tables[i].TableName;
            }
            if (!(tablenames.Contains("封面") && tablenames.Contains("总概算") && tablenames.Contains("其他费用1")))
            {
                MessageBox.Show("Excel文件至少得包含《封面》、《总概算》、《其他费用1》表");
                return false;
            }
            //获取《封面》内容，根据封面内容获取项目名称、项目编号
            proc = new ProjectClass();
            proc.ProjectName = exceloper.GetXValueByContainKeyWord(ds.Tables["封面$"], "工 程 名 称");
            proc.ProjectCode = exceloper.GetXValueByContainKeyWord(ds.Tables["封面$"], "工程编号", 6);
            //获取《总概算》内容
            GetEstinateOverViewTableValues(ds.Tables["总概算$"],ref cellsSet);
            GetCatagorySetValues(ds.Tables["总概算$"],cellsSet,ref catagorySet);
            GetCatagorySetValues_Other(ds.Tables["其他费用1$"],ref catagorySet);
            return true;
        }

        //给cellsSet赋值:总概算
        private void GetEstinateOverViewTableValues(DataTable dt, ref EstinateOverViewTableCellsSet cellsSet)
        {
            ExcelHelper excel = new ExcelHelper();
            int[] topcell = excel.CellindexContain(dt, "工程或费用名称");
            //rows
            int? r_pd = excel.RowIndexContain(dt,"配电站",topcell[1]);
            int? r_tx = excel.RowIndexContain(dt,"通信及调度自动化",topcell[1]);
            int? r_jkxl = excel.RowIndexContain(dt,"架空线路",topcell[1]);
            int? r_dlxl = excel.RowIndexContain(dt,"电缆线路",topcell[1]);
            int? r_njc = excel.RowIndexContain(dt,"当地编制年价差",topcell[1]);
            int? r_other = excel.RowIndexContain(dt, "其他费用", topcell[1]);
            int? r_jbyb = excel.RowIndexContain(dt,"基本预备费",topcell[1]);
            int? r_dklx = excel.RowIndexContain(dt,"贷款利息",topcell[1]);
            //other费用项
            int? r_jscd = excel.RowIndexContain(dt, "建设场地征用及清理费", topcell[1]);
            int? r_sczb_dl = excel.RowIndexContain(dt, "生产准备费(电缆工程)", topcell[1]);
            int? r_sczb_fdl = excel.RowIndexContain(dt, "生产准备费(非电缆工程)", topcell[1]);
            int? r_all = excel.RowIndexContain(dt,"工程动态投资",topcell[1]);
            //columns
            int? c_jzgc = excel.ColumnIndexContain(dt,"建筑工程费",topcell[0]);
            int? c_sbgz = excel.ColumnIndexContain(dt,"设备购置费",topcell[0]);
            int? c_azgc = excel.ColumnIndexContain(dt,"安装工程费",topcell[0]);
            int? c_other = excel.ColumnIndexContain(dt,"其他费用",topcell[0]);
            int? c_sum = excel.ColumnIndexContain(dt,"合计",topcell[0]);

            cellsSet = new EstinateOverViewTableCellsSet();
            cellsSet.PDZ_Cell.cell = new ExcelCellCoordinate(r_pd,topcell[1]);
            cellsSet.TXAuto_Cell.cell = new ExcelCellCoordinate(r_tx,topcell[1]);
            cellsSet.JKXL_Cell.cell = new ExcelCellCoordinate(r_jkxl,topcell[1]);
            cellsSet.DLXL_Cell.cell = new ExcelCellCoordinate(r_dlxl,topcell[1]);
            cellsSet.NJC_Cell.cell = new ExcelCellCoordinate(r_njc,topcell[1]);
            cellsSet.Other_Cell_Y.cell = new ExcelCellCoordinate(r_other,topcell[1]);
            cellsSet.JBYB_Cell.cell = new ExcelCellCoordinate(r_jbyb,topcell[1]);
            cellsSet.DKLX_Cell.cell = new ExcelCellCoordinate(r_dklx,topcell[1]);
            cellsSet.Other_JSCDQL_Cell.cell = new ExcelCellCoordinate(r_jscd,topcell[1]);
            cellsSet.Other_SCZB_DL_Cell.cell = new ExcelCellCoordinate(r_sczb_dl,topcell[1]);
            cellsSet.Other_SCZB_FDL_Cell.cell = new ExcelCellCoordinate(r_sczb_fdl,topcell[1]);
            cellsSet.JZGC_Cell.cell = new ExcelCellCoordinate(topcell[0],c_jzgc);
            cellsSet.SBGZ_Cell.cell = new ExcelCellCoordinate(topcell[0],c_sbgz);
            cellsSet.AZGC_Cell.cell = new ExcelCellCoordinate(topcell[0],c_azgc);
            cellsSet.Other_Cell_X.cell = new ExcelCellCoordinate(topcell[0],c_other);
            cellsSet.HJ_Cell.cell = new ExcelCellCoordinate(topcell[0],c_sum);
            cellsSet.GCDT_Cell.cell = new ExcelCellCoordinate(r_all,topcell[1]);
           
        }

        //catagorySet赋值：总概算
        private void GetCatagorySetValues(DataTable dt, EstinateOverViewTableCellsSet cellsSet,ref ProjectCostCatagorySet catagorySet)
        {
            DataView dv = dt.DefaultView;
            int c_jz =Convert.ToInt32(cellsSet.AZGC_Cell.cell.Column);//建筑列
            int c_sb =Convert.ToInt32(cellsSet.SBGZ_Cell.cell.Column);//设备列
            int c_az =Convert.ToInt32(cellsSet.AZGC_Cell.cell.Column);//安装列
            int c_other =Convert.ToInt32(cellsSet.Other_Cell_X.cell.Column);//其他列
            int cell_hj =Convert.ToInt32(cellsSet.HJ_Cell.cell.Column);//合计列
            catagorySet = new ProjectCostCatagorySet();

            //配电站3层
            if (cellsSet.PDZ_Cell.cell.Row is null)
            {

            }
            else
            {
                int r =Convert.ToInt32(cellsSet.PDZ_Cell.cell.Row);
                string pd_jz = "";
                string pd_az = "";
                string pd_sb = "";
                pd_az = dv[r][c_az].ToString();
                pd_jz = dv[r][c_jz].ToString();
                pd_sb = dv[r][c_sb].ToString();
                catagorySet.pcc_pd_az.costValue = (string.IsNullOrEmpty(pd_az) ? 0 : Convert.ToDouble(pd_az))*10000;
                catagorySet.pcc_pd_jz.costValue = (string.IsNullOrEmpty(pd_jz) ? 0 : Convert.ToDouble(pd_jz))*10000;
                catagorySet.pcc_pd_sb.costValue = (string.IsNullOrEmpty(pd_sb) ? 0 : Convert.ToDouble(pd_sb))*10000;
            }
            //通信自动化3层
            if (!(cellsSet.TXAuto_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.TXAuto_Cell.cell.Row);
                string tx_jz = "";
                string tx_az = "";
                string tx_sb = "";
                tx_az = dv[r][c_az].ToString();
                tx_jz = dv[r][c_jz].ToString();
                tx_sb = dv[r][c_sb].ToString();
                catagorySet.pcc_pd_az.costValue = (string.IsNullOrEmpty(tx_az) ? 0 : Convert.ToDouble(tx_az))*10000;
                catagorySet.pcc_pd_jz.costValue = (string.IsNullOrEmpty(tx_jz) ? 0 : Convert.ToDouble(tx_jz))*10000;
                catagorySet.pcc_pd_sb.costValue = (string.IsNullOrEmpty(tx_sb) ? 0 : Convert.ToDouble(tx_sb))*10000;
            }
            //架空线路1层
            if (!(cellsSet.JKXL_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.JKXL_Cell.cell.Row);
                string jk_hj = "";
                jk_hj = dv[r][cell_hj].ToString();
                catagorySet.pcc_jk.costValue= (string.IsNullOrEmpty(jk_hj) ? 0 : Convert.ToDouble(jk_hj))*10000;
            }
            //电缆线路1层
            if (!(cellsSet.DLXL_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.DLXL_Cell.cell.Row);
                string dl_hj = "";
                dl_hj = dv[r][cell_hj].ToString();
                catagorySet.pcc_dl.costValue = (string.IsNullOrEmpty(dl_hj) ? 0 : Convert.ToDouble(dl_hj))*10000;
            }

            //年价差。
            if (!(cellsSet.NJC_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.NJC_Cell.cell.Row);
                string njc_hj = "";
                njc_hj = dv[r][cell_hj].ToString();
                njc = (string.IsNullOrEmpty(njc_hj) ? 0 : Convert.ToDouble(njc_hj)) * 10000;
            }

            //建设场地征用及清理费
            if (!(cellsSet.Other_JSCDQL_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.Other_JSCDQL_Cell.cell.Row);
                string jscd = "";
                jscd = dv[r][cell_hj].ToString();
                catagorySet.pcc_other_cd.costValue = (string.IsNullOrEmpty(jscd) ? 0 : Convert.ToDouble(jscd))*10000;
            }

            //生产准备费=生产准备费(电缆工程)+生产准备费(非电缆工程)+基本预备费
            string sczb_dl = "";
            string sczb_fdl = "";
            string sczb_jbyb = "";
            if (!(cellsSet.Other_SCZB_DL_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.Other_SCZB_DL_Cell.cell.Row);
                sczb_dl = dv[r][cell_hj].ToString();
            }
            if (!(cellsSet.Other_SCZB_FDL_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.Other_SCZB_FDL_Cell.cell.Row);
                sczb_fdl = dv[r][cell_hj].ToString();
            }
            if (!(cellsSet.JBYB_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.JBYB_Cell.cell.Row);
                sczb_jbyb = dv[r][cell_hj].ToString();
            }
            catagorySet.pcc_other_sczb.costValue = ((string.IsNullOrEmpty(sczb_dl) ? 0 : Convert.ToDouble(sczb_dl)) + (string.IsNullOrEmpty(sczb_fdl) ? 0 : Convert.ToDouble(sczb_fdl)) + (string.IsNullOrEmpty(sczb_jbyb) ? 0 : Convert.ToDouble(sczb_jbyb)))*10000;

            //贷款利息
            if (!(cellsSet.DKLX_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.DKLX_Cell.cell.Row);
                string dklx = "";
                dklx = dv[r][cell_hj].ToString();
                catagorySet.pcc_other_dklx.costValue= (string.IsNullOrEmpty(dklx) ? 0 : Convert.ToDouble(dklx))*10000;
            }

            //动态投资
            if (!(cellsSet.GCDT_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.GCDT_Cell.cell.Row);
                string all = "";
                all = dv[r][cell_hj].ToString();
                catagorySet.pcc_all.costValue = (string.IsNullOrEmpty(all) ? 0 : Convert.ToDouble(all))*10000;
            }
        }

        //catagorySet赋值：其他费用
        private void GetCatagorySetValues_Other(DataTable dt, ref ProjectCostCatagorySet catagorySet)
        {
            ExcelHelper excel = new ExcelHelper();
            DataView dv = dt.DefaultView;
            int[] topcell = excel.CellindexContain(dt, "项目名称");
            int? c_sum = excel.ColumnIndexContain(dt, "合价", topcell[0]);
            int col = Convert.ToInt32(c_sum);
            int? r_xmjs_dl = excel.RowIndexContain(dt, "项目管理经费(电缆工程)",topcell[1]);
            int? r_xmjs_fdl = excel.RowIndexContain(dt, "项目管理经费(非电缆工程)", topcell[1]);
            int? r_xmjs_zb= excel.RowIndexContain(dt, "招标费", topcell[1]);
            int? r_jl_dl = excel.RowIndexContain(dt, "监理费(电缆工程)", topcell[1]);
            int? r_jl_fdl = excel.RowIndexContain(dt, "监理费(非电缆工程)", topcell[1]);
            int? r_qq_dl = excel.RowIndexContain(dt, "项目前期工作费(电缆工程)",topcell[1]);
            int? r_qq_fdl = excel.RowIndexContain(dt, "项目前期工作费(非电缆工程)",topcell[1]);
            int? r_jbsj_fpd = excel.RowIndexContain(dt, "基本设计费(非配电站工程)",topcell[1]);
            int? r_sgtys = excel.RowIndexContain(dt, "施工图预算编制费",topcell[1]);
            int? r_jgtys = excel.RowIndexContain(dt, "竣工图文件编制费",topcell[1]);
            int? r_cswj = excel.RowIndexContain(dt, "初步设计文件评审费",topcell[1]);
            int? r_sgtwjsc = excel.RowIndexContain(dt, "施工图文件审查费",topcell[1]);
            int? r_xmhpj_dl = excel.RowIndexContain(dt, "项目后评价费(电缆工程)", topcell[1]);
            int? r_xmhpj_fdl = excel.RowIndexContain(dt, "项目后评价费(非电缆工程)",topcell[1]);
            int? r_gcjs_dl = excel.RowIndexContain(dt, "工程结算审查费(电缆工程)",topcell[1]);
            int? r_gcjs_fdl = excel.RowIndexContain(dt, "工程结算审查费(非电缆工程)",topcell[1]);
            int? r_jsjc_dl = excel.RowIndexContain(dt, "工程建设检测费(电缆工程)", topcell[1]);
            int? r_jsjc_fdl = excel.RowIndexContain(dt, "工程建设检测费(非电缆工程)",topcell[1]);

            //赋值开始
            string xmjs_dl , xmjs_fdl,xmjs_zb,jl_dl,jl_fdl,qq_dl,qq_fdl,jbsj_fpd,sgtys,jgtys,cswj,sgtwjsc,xmhpj_dl,xmhpj_fdl,gcjs_dl,gcjs_fdl,jsjc_dl,jsjc_fdl;
            try
            {
                xmjs_dl = dv[Convert.ToInt32(r_xmjs_dl)][col].ToString();
                xmjs_fdl = dv[Convert.ToInt32(r_xmjs_fdl)][col].ToString();
                xmjs_zb= dv[Convert.ToInt32(r_xmjs_zb)][col].ToString();
                jl_dl = dv[Convert.ToInt32(r_jl_dl)][col].ToString();
                jl_fdl = dv[Convert.ToInt32(r_jl_fdl)][col].ToString();
                qq_dl = dv[Convert.ToInt32(r_qq_dl)][col].ToString();
                qq_fdl = dv[Convert.ToInt32(r_qq_fdl)][col].ToString();
                jbsj_fpd = dv[Convert.ToInt32(r_jbsj_fpd)][col].ToString();
                sgtys = dv[Convert.ToInt32(r_sgtys)][col].ToString();
                jgtys = dv[Convert.ToInt32(r_jgtys)][col].ToString();
                cswj = dv[Convert.ToInt32(r_cswj)][col].ToString();
                sgtwjsc = dv[Convert.ToInt32(r_sgtwjsc)][col].ToString();
                xmhpj_dl = dv[Convert.ToInt32(r_xmhpj_dl)][col].ToString();
                xmhpj_fdl = dv[Convert.ToInt32(r_xmhpj_fdl)][col].ToString();
                gcjs_dl = dv[Convert.ToInt32(r_gcjs_dl)][col].ToString();
                gcjs_fdl = dv[Convert.ToInt32(r_gcjs_fdl)][col].ToString();
                jsjc_dl = dv[Convert.ToInt32(r_jsjc_dl)][col].ToString();
                jsjc_fdl = dv[Convert.ToInt32(r_jsjc_fdl)][col].ToString();

                //string转换int
                catagorySet.pcc_other_xmgl.costValue = (string.IsNullOrEmpty(xmjs_dl) ? 0 : Convert.ToDouble(xmjs_dl)) + (string.IsNullOrEmpty(xmjs_fdl) ? 0 : Convert.ToDouble(xmjs_fdl));
                catagorySet.pcc_other_zb.costValue = string.IsNullOrEmpty(xmjs_zb) ? 0 : Convert.ToDouble(xmjs_zb);
                catagorySet.pcc_other_gcjl.costValue = (string.IsNullOrEmpty(jl_dl) ? 0 : Convert.ToDouble(jl_dl)) + (string.IsNullOrEmpty(jl_fdl)?0:Convert.ToDouble(jl_fdl));
                catagorySet.pcc_other_kc.costValue = (string.IsNullOrEmpty(qq_dl) ? 0 : Convert.ToDouble(qq_dl)) + (string.IsNullOrEmpty(qq_fdl)?0:Convert.ToDouble(qq_fdl));
                //工程设计费=基本设计费+施工图预算编制费+竣工图文件编制费
                catagorySet.pcc_other_sj.costValue = (string.IsNullOrEmpty(jbsj_fpd) ? 0 : Convert.ToDouble(jbsj_fpd)) + (string.IsNullOrEmpty(sgtys)?0:Convert.ToDouble(sgtys)) + (string.IsNullOrEmpty(jgtys)?0:Convert.ToDouble(jgtys));
                //2个评审
                catagorySet.pcc_other_ps.costValue = (string.IsNullOrEmpty(cswj) ? 0 : Convert.ToDouble(cswj)) + (string.IsNullOrEmpty(sgtwjsc) ? 0 : Convert.ToDouble(sgtwjsc));
                //2个后评价
                catagorySet.pcc_other_hpj.costValue = (string.IsNullOrEmpty(xmhpj_dl) ? 0 : Convert.ToDouble(xmhpj_dl)) + (string.IsNullOrEmpty(xmhpj_fdl)?0:Convert.ToDouble(xmhpj_fdl));
                //技术经济标准编制管理费=2个结算审查
                catagorySet.pcc_other_bzbz.costValue = (string.IsNullOrEmpty(gcjs_dl) ? 0 : Convert.ToDouble(gcjs_dl)) + (string.IsNullOrEmpty(gcjs_fdl)?0:Convert.ToDouble(gcjs_fdl));
                //工程建设监督检测费
                catagorySet.pcc_other_jdjc.costValue = (string.IsNullOrEmpty(jsjc_dl) ? 0 : Convert.ToDouble(jsjc_dl)) + (string.IsNullOrEmpty(jsjc_fdl) ? 0 : Convert.ToDouble(jsjc_fdl));

            }
            catch (Exception e)
            { MessageBox.Show(e.Message); }
        }

        //为financdata赋值
        private void GetDataToFinanceData(ProjectClass proc, ProjectCostCatagorySet catagorySet, ref ObservableCollection<ProjectEstimateViewModel> finacedata)
        {
            foreach (ProjectEstimateViewModel pevm in finacedata)
            {
                pevm.ProjectName = proc.ProjectName;
                pevm.ProjectCode = proc.ProjectCode;
                switch (pevm.ExpanseCategory)
                {
                    case "10KV（含20KV）及以下基建项目":
                        pevm.EstimateNumber = catagorySet.pcc_all.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—配电站（开关站）工程—建筑工程":
                        pevm.EstimateNumber = catagorySet.pcc_pd_jz.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—配电站（开关站）工程—安装工程":
                        pevm.EstimateNumber = catagorySet.pcc_pd_az.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—配电站（开关站）工程—设备购置":
                        pevm.EstimateNumber = catagorySet.pcc_pd_sb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—通信及调度自动化—建筑工程":
                        pevm.EstimateNumber = catagorySet.pcc_tx_jz.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—通信及调度自动化—安装工程":
                        pevm.EstimateNumber = catagorySet.pcc_tx_az.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—通信及调度自动化—设备购置":
                        pevm.EstimateNumber = catagorySet.pcc_tx_sb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—架空线路工程—架空线路本体工程":
                        pevm.EstimateNumber = catagorySet.pcc_jk.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—电缆线路工程—电缆本体工程":
                        pevm.EstimateNumber = catagorySet.pcc_dl.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—建设场地征用及清理费":
                        pevm.EstimateNumber = catagorySet.pcc_other_cd.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设管理费—项目管理经费":
                        pevm.EstimateNumber = catagorySet.pcc_other_xmgl.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设管理费—项目管理经费—其中：业务招待费":
                        pevm.EstimateNumber = catagorySet.pcc_other_zd.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设管理费—招标费":
                        pevm.EstimateNumber = catagorySet.pcc_other_zb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设管理费—工程监理费":
                        pevm.EstimateNumber = catagorySet.pcc_other_gcjl.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—工程勘察费":
                        pevm.EstimateNumber = catagorySet.pcc_other_kc.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—工程设计费":
                        pevm.EstimateNumber = catagorySet.pcc_other_sj.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—设计文件评审费":
                        pevm.EstimateNumber = catagorySet.pcc_other_ps.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—项目后评价费":
                        pevm.EstimateNumber = catagorySet.pcc_other_hpj.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—项目建设技术服务费—技术经济标准编制管理费":
                        pevm.EstimateNumber = catagorySet.pcc_other_bzbz.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—工程建设监督检测费":
                        pevm.EstimateNumber = catagorySet.pcc_other_jdjc.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—生产准备费":
                        pevm.EstimateNumber = catagorySet.pcc_other_sczb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—其他费用—基本预备费":
                        pevm.EstimateNumber = catagorySet.pcc_other_jbyb.costValue.ToString();
                        break;
                    case "10KV（含20KV）及以下基建项目—建设期贷款利息":
                        pevm.EstimateNumber = catagorySet.pcc_other_dklx.costValue.ToString();
                        break;
                    default:
                        MessageBox.Show("错误：未能识别的费用类别——"+pevm.ExpanseCategory);
                        break;

                }
            }



        }

        //尾差处理
        private void ManageTailDifference(ref TailDifferenceViewModel tailDifferenceViewModel)
        {
            tailDifferenceViewModel.AnnualPriceDifference = njc.ToString();
            if (catagorySet.pcc_jk.costValue != 0)
            {
                catagorySet.pcc_jk.costValue += njc;
                tailDifferenceViewModel.ItemWithTailDifference = catagorySet.pcc_jk.catagoryName;
                tailDifferenceViewModel.TailDifference = catagorySet.pcc_weicha.costValue.ToString();
                catagorySet.pcc_jk.costValue += catagorySet.pcc_weicha.costValue;
            }
            else if (catagorySet.pcc_dl.costValue != 0)
            {
                catagorySet.pcc_dl.costValue += njc;
                tailDifferenceViewModel.ItemWithTailDifference = catagorySet.pcc_dl.catagoryName;
                tailDifferenceViewModel.TailDifference = catagorySet.pcc_weicha.costValue.ToString();
                catagorySet.pcc_dl.costValue += catagorySet.pcc_weicha.costValue;
            }
            else if (catagorySet.pcc_pd_az.costValue != 0)
            {
                catagorySet.pcc_pd_az.costValue += njc;
                tailDifferenceViewModel.ItemWithTailDifference = catagorySet.pcc_pd_az.catagoryName;
                tailDifferenceViewModel.TailDifference = catagorySet.pcc_weicha.costValue.ToString();
                catagorySet.pcc_pd_az.costValue += catagorySet.pcc_weicha.costValue;
            }
            else if (catagorySet.pcc_pd_sb.costValue != 0)
            {
                catagorySet.pcc_pd_sb.costValue += njc;
                tailDifferenceViewModel.ItemWithTailDifference = catagorySet.pcc_pd_sb.catagoryName;
                tailDifferenceViewModel.TailDifference = catagorySet.pcc_weicha.costValue.ToString();
                catagorySet.pcc_pd_sb.costValue += catagorySet.pcc_weicha.costValue;
            }
            else if (catagorySet.pcc_pd_jz.costValue != 0)
            {
                catagorySet.pcc_pd_jz.costValue += njc;
                tailDifferenceViewModel.ItemWithTailDifference = catagorySet.pcc_pd_jz.catagoryName;
                tailDifferenceViewModel.TailDifference = catagorySet.pcc_weicha.costValue.ToString();
                catagorySet.pcc_pd_jz.costValue += catagorySet.pcc_weicha.costValue;
            }
            else if (catagorySet.pcc_tx_az.costValue != 0)
            {
                catagorySet.pcc_tx_az.costValue += njc;
                tailDifferenceViewModel.ItemWithTailDifference = catagorySet.pcc_tx_az.catagoryName;
                tailDifferenceViewModel.TailDifference = catagorySet.pcc_weicha.costValue.ToString();
                catagorySet.pcc_tx_az.costValue += catagorySet.pcc_weicha.costValue;
            }
            else if (catagorySet.pcc_tx_sb.costValue != 0)
            {
                catagorySet.pcc_tx_sb.costValue += njc;
                tailDifferenceViewModel.ItemWithTailDifference = catagorySet.pcc_tx_sb.catagoryName;
                tailDifferenceViewModel.TailDifference = catagorySet.pcc_weicha.costValue.ToString();
                catagorySet.pcc_tx_sb.costValue += catagorySet.pcc_weicha.costValue;
            }
            else
            {
                catagorySet.pcc_tx_jz.costValue += njc;
                tailDifferenceViewModel.ItemWithTailDifference = catagorySet.pcc_tx_jz.catagoryName;
                tailDifferenceViewModel.TailDifference = catagorySet.pcc_weicha.costValue.ToString();
                catagorySet.pcc_tx_jz.costValue += catagorySet.pcc_weicha.costValue;
            }

            
           
            
        }

        private ProjectCostCatagory WitchNotNull()
        {
            ProjectCostCatagory catagory = new ProjectCostCatagory();
            if (catagorySet.pcc_jk.costValue != 0)
            {
                catagory = catagorySet.pcc_jk;
            }
            else if (catagorySet.pcc_dl.costValue != 0)
            {
                catagory = catagorySet.pcc_dl;
            }
            else if (catagorySet.pcc_pd_az.costValue != 0)
            {
                catagory = catagorySet.pcc_pd_az;
            }
            else if (catagorySet.pcc_pd_sb.costValue != 0)
            {
                catagory = catagorySet.pcc_pd_sb;
            }
            else if (catagorySet.pcc_pd_jz.costValue != 0)
            {
                catagory = catagorySet.pcc_pd_jz;
            }
            else if (catagorySet.pcc_tx_az.costValue != 0)
            {
                catagory = catagorySet.pcc_tx_az;
            }
            else if (catagorySet.pcc_tx_sb.costValue != 0)
            {
                catagory = catagorySet.pcc_tx_sb;
            }
            else
            {
                catagory = catagorySet.pcc_tx_jz;
            }
            return catagory;
        }
    }



}
