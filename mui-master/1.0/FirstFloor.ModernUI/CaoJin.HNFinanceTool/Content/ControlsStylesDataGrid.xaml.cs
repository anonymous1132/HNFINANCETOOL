﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
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

        private ProjectEstimateSetViewModel projectEstimateSetViewModel ;

        private TailDifferenceViewModel tdvm;

        private bool? isWanYuan;

        //从文件获取数据
        private void GetData(ref ProjectEstimateSetViewModel projectEstimateSetViewModel,ref TailDifferenceViewModel tdvm)
        {

            string datafile = _datapath + DataFileName;
            DataSet ds = XmlOperate.GetDataSet(datafile);
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            projectEstimateSetViewModel=new ProjectEstimateSetViewModel(dt);
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
            ProjectEstimateViewModel temp = new ProjectEstimateViewModel();
            PropertyInfo[] propertys = temp.GetType().GetProperties();
            ProjectTotalEstimateViewModel temp2 = new ProjectTotalEstimateViewModel();
            PropertyInfo[] property2 = temp2.GetType().GetProperties();
            dt.Columns.Add("id");
            dt.Columns.Add("ID");
            dt.Columns.Add("ProjectName");
            dt.Columns.Add("ProjectCode");
            dt.Columns.Add("IndividualProjectName");
            dt.Columns.Add("IndividualProjectCode");
            dt.Columns.Add("ExpanseCategory");
            dt.Columns.Add("WBSCode");
            dt.Columns.Add("EstimateNumber");
            dt.Columns.Add("InternalControl");
            dt.Columns.Add("DeductibleVATRatio");
            dt.Columns.Add("TotalInvestmentWithTax");
            dt.Columns.Add("TotalInvestmentWithoutTax");
            dt.Columns.Add("MaxInternalControl");
            dt.Columns.Add("MaxDeductibleVATRatio");
            dt.Columns.Add("MinDeductibleVATRatio");
            foreach (ProjectEstimateViewModel t in projectEstimateSetViewModel.EstimateViewModels)
            {
                if (t is ProjectTotalEstimateViewModel) continue;
                PropertyInfo[] property = t.GetType().GetProperties();
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pi in propertys)
                {
                    if (!pi.CanRead) continue;

                    dr[pi.Name] = pi.GetValue(t, null);

                }
                dt.Rows.Add(dr);
            }
              DataRow  dr2 = dt.NewRow();

            foreach (PropertyInfo pi in property2)
            {
                if (!pi.CanRead) continue;

                dr2[pi.Name] = pi.GetValue(projectEstimateSetViewModel.TotalEstimateViewModel, null);

            }
            dr2["EstimateNumber"] = projectEstimateSetViewModel.TotalEstimateViewModel.EstimateNumber;
            dr2["TotalInvestmentWithTax"] = projectEstimateSetViewModel.TotalEstimateViewModel.TotalInvestmentWithTax;
            dr2["TotalInvestmentWithoutTax"] = projectEstimateSetViewModel.TotalEstimateViewModel.TotalInvestmentWithoutTax;
            dt.Rows.InsertAt(dr2,0);
            return dt;
        }

        private DataTable TranslateBudgetaryBlank2DT()
        {
            DataTable dt = new DataTable("BudgetaryUpperLimit");
            dt.Columns.Add("AccumulativePlan");
            dt.Columns.Add("ErpHappenedWithoutTax");
            dt.Columns.Add("DeductibleVAT");
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "";
            dt.Rows.Add(dr);
            return dt;
        }

        private DataTable TranslateDepartmentFilledBlank2DT()
        {
            DataTable dt = new DataTable("DepartmentBudgetFilled");
            dt.Columns.Add("DepartmentFilledBudgetWithTax");
            DataRow dr = dt.NewRow();
            dr[0] = "";
            dt.Rows.Add(dr);
            return dt;
        }
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
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
                    foreach (ProjectEstimateViewModel pjvm in projectEstimateSetViewModel.EstimateViewModels)
                    {
                        pjvm.ProjectName = this.textbox_setcontent.Text;
                    }
                    break;
                case 1:
                    foreach (ProjectEstimateViewModel pjvm in projectEstimateSetViewModel.EstimateViewModels)
                    {
                        pjvm.ProjectCode = this.textbox_setcontent.Text;
                    }
                    break;
                case 2:
                    foreach (ProjectEstimateViewModel pjvm in projectEstimateSetViewModel.EstimateViewModels)
                    {
                        pjvm.WBSCode = this.textbox_setcontent.Text;
                    }
                    break;
                case 3:
                    foreach (ProjectEstimateViewModel pjvm in projectEstimateSetViewModel.EstimateViewModels)
                    {
                        pjvm.InternalControl = this.textbox_setcontent.Text;
                    }
                    break;
                case 4:
                    foreach (ProjectEstimateViewModel pjvm in projectEstimateSetViewModel.EstimateViewModels)
                    {
                        pjvm.DeductibleVATRatio = this.textbox_setcontent.Text;
                    }
                    break;
                case 5:
                    foreach (ProjectEstimateViewModel pjvm in projectEstimateSetViewModel.EstimateViewModels)
                    {
                        pjvm.MaxInternalControl = this.textbox_setcontent.Text;
                    }
                    break;
                case 6:
                    foreach (ProjectEstimateViewModel pjvm in projectEstimateSetViewModel.EstimateViewModels)
                    {
                        pjvm.MaxDeductibleVATRatio = this.textbox_setcontent.Text;
                    }
                    break;
                case 7:
                    foreach (ProjectEstimateViewModel pjvm in projectEstimateSetViewModel.EstimateViewModels)
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
                string filename = string.IsNullOrEmpty(projectEstimateSetViewModel.EstimateViewModels[0].ProjectName.Trim()) ? "mould" : projectEstimateSetViewModel.EstimateViewModels[0].ProjectName.Trim() + ".est";
                string filepath = System.IO.Directory.GetCurrentDirectory() + "\\App\\data\\" + filename;
                DataSet ds = new DataSet("Finance");
                ds.Tables.Add(TranslateVM2DT());
                ds.Tables.Add(TranslateTDVM2DT());
                ds.Tables.Add(TranslateBudgetaryBlank2DT());
                ds.Tables.Add(TranslateDepartmentFilledBlank2DT());
                ds.WriteXml(filepath);
                this.DataFileName = System.IO.Path.GetFileName(filepath);
                this.Item_projectname.IsReadOnly = true;
                this.combo_item_projectname.IsEnabled = false;
                if (this.combobox_title.SelectedIndex == 0)
                {
                    this.combobox_title.SelectedIndex = 1;
                }
                this.button_import.IsEnabled = false;
            }
            else
            {
                string datafile = _datapath + DataFileName;
                DataSet ds = new DataSet("Finance");
                ds.Tables.Add(TranslateVM2DT());
                ds.Tables.Add(TranslateTDVM2DT().Copy());
                ds.Tables.Add(XmlHelper.GetTable(datafile, XmlHelper.XmlType.File, "BudgetaryUpperLimit").Copy());
                ds.Tables.Add(XmlHelper.GetTable(datafile,XmlHelper.XmlType.File, "DepartmentBudgetFilled").Copy());
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
            saveFile.FileName = DateTime.Now.GetDateTimeFormats('D')[0].ToString() + projectEstimateSetViewModel.EstimateViewModels[0].ProjectName;
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
                this.button_import.IsEnabled = true;
                this.Item_projectname.IsReadOnly = false;
                this.combo_item_projectname.IsEnabled = true;
            }
            else { this.Item_projectname.IsReadOnly = true;this.combo_item_projectname.IsEnabled = false;this.button_import.IsEnabled = false; }
            tdvm = new TailDifferenceViewModel();
            GetData(ref projectEstimateSetViewModel,ref tdvm);
            //Bind the DataGrid 
            this.DataContext = tdvm;
            DG1.DataContext = projectEstimateSetViewModel.EstimateViewModels;
            
        }
        //从excel导入
        private void button_import_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImportFile()) return;
            ManageTailDifference(ref tdvm);
            projectEstimateSetViewModel.GetDataToFinanceData(proc,catagorySet);
            projectEstimateSetViewModel.SetToDestCompositeTaxRate(tdvm.Double_CompositeTaxRate);
        }

        private ProjectClass proc;
        private EstinateOverViewTableCellsSet cellsSet;
        private ProjectCostCatagorySet catagorySet;
  
        private double njc;//年价差

        //debug----封面、其他费用1取消判断必要条件
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
            if (!(tablenames.Contains("总概算") && tablenames.Contains("其他费用")))
            {
                MessageBox.Show("Excel文件至少得包含《总概算》、《其他费用》或《其他费用1、2、3……》表");
                return false;
            }
            //获取《封面》内容，根据封面内容获取项目名称、项目编号
            proc = new ProjectClass();
            try
            {
                proc.ProjectName = exceloper.GetXValueByContainKeyWord(ds.Tables["封面$"], "工 程 名 称");
                proc.ProjectCode = exceloper.GetXValueByContainKeyWord(ds.Tables["封面$"], "工程编号", 6);
            }
            catch (Exception) { }
            //获取《总概算》内容
            GetEstinateOverViewTableValues(ds.Tables["总概算$"],ref cellsSet);
            GetCatagorySetValues(ds.Tables["总概算$"],cellsSet,ref catagorySet);
            
            GetCatagorySetValues_Other(ds,ref catagorySet);
            return true;
        }

        //给cellsSet赋值:总概算
        private void GetEstinateOverViewTableValues(DataTable dt, ref EstinateOverViewTableCellsSet cellsSet)
        {
            ExcelHelper excel = new ExcelHelper();

            //-------------------------debug:从总概算页获取工程名称--------------------------------------------
            int[] title = excel.CellindexContain(dt,"概算书");
            proc.ProjectName = string.IsNullOrEmpty(proc.ProjectName) & (!(title is null)) ? dt.DefaultView[title[0]][title[1]].ToString().Replace("概算书","").Trim():proc.ProjectName;
            proc.ProjectCode = string.IsNullOrEmpty(proc.ProjectCode) ? "" : proc.ProjectCode;
            //-------------------------------------------------------------------------------------------------
           
            //---------------------------debug:万元还是元？----------------------------------------------------------------------
            int[]unit= excel.CellindexContain(dt, "金额单位");
            if (unit is null) isWanYuan = null;
            else
            {
                isWanYuan = dt.DefaultView[unit[0]][unit[1]].ToString().Contains("万元");
            }
            //-------------------------------------------------------------------------------------------------
            //rows
            int[] topcell = excel.CellindexContain(dt, "工程或费用名称");
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
            int? r_sczb = excel.RowIndex(dt, "生产准备费", topcell[1]);
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
            cellsSet.Other_SCZB_Cell.cell = new ExcelCellCoordinate(r_sczb, topcell[1]);
            cellsSet.JZGC_Cell.cell = new ExcelCellCoordinate(topcell[0],c_jzgc);
            cellsSet.SBGZ_Cell.cell = new ExcelCellCoordinate(topcell[0],c_sbgz);
            cellsSet.AZGC_Cell.cell = new ExcelCellCoordinate(topcell[0],c_azgc);
            cellsSet.Other_Cell_X.cell = new ExcelCellCoordinate(topcell[0],c_other);
            cellsSet.HJ_Cell.cell = new ExcelCellCoordinate(topcell[0],c_sum);
            cellsSet.GCDT_Cell.cell = new ExcelCellCoordinate(r_all,topcell[1]);
            
           
        }

        //catagorySet赋值：总概算
        //-----------------debug:万元单位判断--------------------------------------------------
        private void GetCatagorySetValues(DataTable dt, EstinateOverViewTableCellsSet cellsSet,ref ProjectCostCatagorySet catagorySet)
        {
            DataView dv = dt.DefaultView;
            int c_jz =Convert.ToInt32(cellsSet.JZGC_Cell.cell.Column);//建筑列
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
                catagorySet.pcc_pd_az.costValue = (string.IsNullOrEmpty(pd_az) ? 0 : Convert.ToDouble(pd_az));
                catagorySet.pcc_pd_jz.costValue = (string.IsNullOrEmpty(pd_jz) ? 0 : Convert.ToDouble(pd_jz));
                catagorySet.pcc_pd_sb.costValue = (string.IsNullOrEmpty(pd_sb) ? 0 : Convert.ToDouble(pd_sb));
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
                catagorySet.pcc_pd_az.costValue = (string.IsNullOrEmpty(tx_az) ? 0 : Convert.ToDouble(tx_az));
                catagorySet.pcc_pd_jz.costValue = (string.IsNullOrEmpty(tx_jz) ? 0 : Convert.ToDouble(tx_jz));
                catagorySet.pcc_pd_sb.costValue = (string.IsNullOrEmpty(tx_sb) ? 0 : Convert.ToDouble(tx_sb));
            }
            //架空线路1层
            if (!(cellsSet.JKXL_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.JKXL_Cell.cell.Row);
                string jk_hj = "";
                jk_hj = dv[r][cell_hj].ToString();
                catagorySet.pcc_jk.costValue= (string.IsNullOrEmpty(jk_hj) ? 0 : Convert.ToDouble(jk_hj));
            }
            //电缆线路1层
            if (!(cellsSet.DLXL_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.DLXL_Cell.cell.Row);
                string dl_hj = "";
                dl_hj = dv[r][cell_hj].ToString();
                catagorySet.pcc_dl.costValue = (string.IsNullOrEmpty(dl_hj) ? 0 : Convert.ToDouble(dl_hj));
            }

            //年价差。
            if (!(cellsSet.NJC_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.NJC_Cell.cell.Row);
                string njc_hj = "";
                njc_hj = dv[r][cell_hj].ToString();
                njc = (string.IsNullOrEmpty(njc_hj) ? 0 : Convert.ToDouble(njc_hj));
            }

            //建设场地征用及清理费
            if (!(cellsSet.Other_JSCDQL_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.Other_JSCDQL_Cell.cell.Row);
                string jscd = "";
                jscd = dv[r][cell_hj].ToString();
                catagorySet.pcc_other_cd.costValue = (string.IsNullOrEmpty(jscd) ? 0 : Convert.ToDouble(jscd));
            }

            //生产准备费=生产准备费(电缆工程)+生产准备费(非电缆工程)+基本预备费+生产准备费
            string sczb_dl = "";
            string sczb_fdl = "";
            string sczb_jbyb = "";
            string sczb = "";
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
            if (!(cellsSet.Other_SCZB_Cell.cell.Row is null))
            {
                int r= Convert.ToInt32(cellsSet.Other_SCZB_Cell.cell.Row);
                sczb = dv[r][cell_hj].ToString();
            }
            catagorySet.pcc_other_sczb.costValue = ((string.IsNullOrEmpty(sczb_dl) ? 0 : Convert.ToDouble(sczb_dl)) + (string.IsNullOrEmpty(sczb_fdl) ? 0 : Convert.ToDouble(sczb_fdl)) + (string.IsNullOrEmpty(sczb_jbyb) ? 0 : Convert.ToDouble(sczb_jbyb))+ (string.IsNullOrEmpty(sczb) ? 0 : Convert.ToDouble(sczb)));

            //贷款利息
            if (!(cellsSet.DKLX_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.DKLX_Cell.cell.Row);
                string dklx = "";
                dklx = dv[r][cell_hj].ToString();
                catagorySet.pcc_other_dklx.costValue= (string.IsNullOrEmpty(dklx) ? 0 : Convert.ToDouble(dklx));
            }

            //动态投资
            if (!(cellsSet.GCDT_Cell.cell.Row is null))
            {
                int r = Convert.ToInt32(cellsSet.GCDT_Cell.cell.Row);
                string all = "";
                all = dv[r][cell_hj].ToString();
                catagorySet.pcc_all.costValue = (string.IsNullOrEmpty(all) ? 0 : Convert.ToDouble(all));
            }
            if (isWanYuan==true || ((isWanYuan ==null)&catagorySet.pcc_all.costValue<10000))
            {
                catagorySet.pcc_all.costValue *= 10000;
                catagorySet.pcc_dl.costValue *= 10000;
                catagorySet.pcc_jk.costValue *= 10000;
                catagorySet.pcc_pd_az.costValue *= 10000;
                catagorySet.pcc_pd_jz.costValue *= 10000;
                catagorySet.pcc_pd_sb.costValue *= 10000;
                catagorySet.pcc_tx_az.costValue *= 10000;
                catagorySet.pcc_tx_jz.costValue *= 10000;
                catagorySet.pcc_tx_sb.costValue *= 10000;
                njc *= 10000;
                catagorySet.pcc_other_cd.costValue *= 10000;
                catagorySet.pcc_other_sczb.costValue *= 10000;
                catagorySet.pcc_other_dklx.costValue *= 10000;
            }

        }
        //------------------------------------------------------------------------------------------------------------------------------------------


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
            //debug----
            int? r_xmjs = excel.RowIndex(dt, "项目管理经费", topcell[1]);
            //----
            int? r_xmjs_zb= excel.RowIndexContain(dt, "招标费", topcell[1]);
            int? r_jl_dl = excel.RowIndexContain(dt, "监理费(电缆工程)", topcell[1]);
            int? r_jl_fdl = excel.RowIndexContain(dt, "监理费(非电缆工程)", topcell[1]);
            //debug--------
            int? r_jl = excel.RowIndexContain(dt, "工程监理费", topcell[1]);
            //----------------
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
            string xmjs_dl, xmjs_fdl, xmjs_zb, jl_dl, jl_fdl, qq_dl, qq_fdl, jbsj_fpd, sgtys, jgtys, cswj, sgtwjsc, xmhpj_dl, xmhpj_fdl, gcjs_dl, gcjs_fdl, jsjc_dl, jsjc_fdl, jl, xmjs;
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
                jl = dv[Convert.ToInt32(r_jl)][col].ToString();
                xmjs = dv[Convert.ToInt32(r_xmjs)][col].ToString();
                //string转换int
                catagorySet.pcc_other_xmgl.costValue = (string.IsNullOrEmpty(xmjs_dl) ? 0 : Convert.ToDouble(xmjs_dl)) + (string.IsNullOrEmpty(xmjs_fdl) ? 0 : Convert.ToDouble(xmjs_fdl)) + (string.IsNullOrEmpty(xmjs) ? 0 : Convert.ToDouble(xmjs));
                catagorySet.pcc_other_zb.costValue = string.IsNullOrEmpty(xmjs_zb) ? 0 : Convert.ToDouble(xmjs_zb);
                catagorySet.pcc_other_gcjl.costValue = (string.IsNullOrEmpty(jl_dl) ? 0 : Convert.ToDouble(jl_dl)) + (string.IsNullOrEmpty(jl_fdl)?0:Convert.ToDouble(jl_fdl)) + (string.IsNullOrEmpty(jl) ? 0 : Convert.ToDouble(jl));
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

        //catagorySet赋值：其他费用
        private void GetCatagorySetValues_Other(DataSet ds, ref ProjectCostCatagorySet catagorySet)
        {
            ExcelHelper excel = new ExcelHelper();
            string xmjs_dl = "", xmjs_fdl = "", xmjs_zb = "", jl_dl = "", jl_fdl = "", qq_dl = "", qq_fdl = "", jbsj_fpd = "", sgtys = "", jgtys = "", cswj = "", sgtwjsc = "", xmhpj_dl = "", xmhpj_fdl = "", gcjs_dl = "", gcjs_fdl = "", jsjc_dl = "", jsjc_fdl = "", jl = "",xmjs="",gckc="",gcsjf="" ;
            string sjwjps = "",bzbz = "",jsjc="";
            foreach (DataTable dt in ds.Tables)
            {
                if (!dt.TableName.Contains("其他费用")) continue;
                DataView dv = dt.DefaultView;
                int[] topcell = excel.CellindexContain(dt, "项目名称");
                int? c_sum = excel.ColumnIndexContain(dt, "合价", topcell[0]);
                int col = Convert.ToInt32(c_sum);
                int? r_xmjs_dl = excel.RowIndexContain(dt, "项目管理经费(电缆工程)", topcell[1]);
                int? r_xmjs_fdl = excel.RowIndexContain(dt, "项目管理经费(非电缆工程)", topcell[1]);
                int? r_xmjs_zb = excel.RowIndexContain(dt, "招标费", topcell[1]);
                int? r_jl_dl = excel.RowIndexContain(dt, "监理费(电缆工程)", topcell[1]);
                int? r_jl_fdl = excel.RowIndexContain(dt, "监理费(非电缆工程)", topcell[1]);
                //勘察
                int? r_qq_dl = excel.RowIndexContain(dt, "项目前期工作费(电缆工程)", topcell[1]);
                int? r_qq_fdl = excel.RowIndexContain(dt, "项目前期工作费(非电缆工程)", topcell[1]);
                int? r_gckc = excel.RowIndex(dt, "工程勘察费", topcell[1]);
                //设计
                int? r_jbsj_fpd = excel.RowIndexContain(dt, "基本设计费(非配电站工程)", topcell[1]);
                int? r_sgtys = excel.RowIndexContain(dt, "施工图预算编制费", topcell[1]);
                int? r_jgtys = excel.RowIndexContain(dt, "竣工图文件编制费", topcell[1]);
                int? r_gcsjf = excel.RowIndex(dt, "工程设计费", topcell[1]);
                //评审
                int? r_cswj = excel.RowIndexContain(dt, "初步设计文件评审费", topcell[1]);
                int? r_sgtwjsc = excel.RowIndexContain(dt, "施工图文件审查费", topcell[1]);
                int?r_sjwjps= excel.RowIndexContain(dt, "设计文件评审费", topcell[1]);

                int? r_xmhpj_dl = excel.RowIndexContain(dt, "项目后评价费(电缆工程)", topcell[1]);
                int? r_xmhpj_fdl = excel.RowIndexContain(dt, "项目后评价费(非电缆工程)", topcell[1]);

                //技术经济标准编制管理费
                int? r_gcjs_dl = excel.RowIndexContain(dt, "工程结算审查费(电缆工程)", topcell[1]);
                int? r_gcjs_fdl = excel.RowIndexContain(dt, "工程结算审查费(非电缆工程)", topcell[1]);
                int? r_bzbz = excel.RowIndexContain(dt, "技术经济标准编制管理费", topcell[1]);

                int? r_jsjc_dl = excel.RowIndexContain(dt, "工程建设检测费(电缆工程)", topcell[1]);
                int? r_jsjc_fdl = excel.RowIndexContain(dt, "工程建设检测费(非电缆工程)", topcell[1]);
                int? r_jsjc = excel.RowIndexContain(dt, "工程建设监督检测费", topcell[1]);
                //debug----
                int? r_xmjs = excel.RowIndex(dt, "项目管理经费", topcell[1]);
                //----
                //debug--------
                int? r_jl = excel.RowIndexContain(dt, "工程监理费", topcell[1]);
                //----------------
                xmjs_dl = (r_xmjs_dl is null)?xmjs_dl: dv[Convert.ToInt32(r_xmjs_dl)][col].ToString();
                xmjs_fdl =(r_xmjs_fdl is null)?xmjs_fdl: dv[Convert.ToInt32(r_xmjs_fdl)][col].ToString();
                xmjs_zb =(r_xmjs_zb is null)?xmjs_zb: dv[Convert.ToInt32(r_xmjs_zb)][col].ToString();
                xmjs= (r_xmjs is null) ? xmjs : dv[Convert.ToInt32(r_xmjs)][col].ToString();
                jl_dl =(r_jl_dl is null)?jl_dl: dv[Convert.ToInt32(r_jl_dl)][col].ToString();
                jl_fdl =(r_jl_fdl is null)?jl_fdl: dv[Convert.ToInt32(r_jl_fdl)][col].ToString();
                jl= (r_jl is null) ? jl : dv[Convert.ToInt32(r_jl)][col].ToString();
                qq_dl =(r_qq_dl is null)?qq_dl: dv[Convert.ToInt32(r_qq_dl)][col].ToString();
                qq_fdl =(r_qq_fdl is null)?qq_fdl: dv[Convert.ToInt32(r_qq_fdl)][col].ToString();
                jbsj_fpd =(r_jbsj_fpd is null)?jbsj_fpd: dv[Convert.ToInt32(r_jbsj_fpd)][col].ToString();
                sgtys =(r_sgtys is null)?sgtys: dv[Convert.ToInt32(r_sgtys)][col].ToString();
                jgtys =(r_jgtys is null)?jgtys: dv[Convert.ToInt32(r_jgtys)][col].ToString();
                cswj =(r_cswj is null)?cswj: dv[Convert.ToInt32(r_cswj)][col].ToString();
                sgtwjsc =(r_sgtwjsc is null)?sgtwjsc: dv[Convert.ToInt32(r_sgtwjsc)][col].ToString();
                xmhpj_dl =(r_xmhpj_dl is null)?xmhpj_dl: dv[Convert.ToInt32(r_xmhpj_dl)][col].ToString();
                xmhpj_fdl =(r_xmhpj_fdl is null)?xmhpj_fdl: dv[Convert.ToInt32(r_xmhpj_fdl)][col].ToString();
                gcjs_dl =(r_gcjs_dl is null)?gcjs_dl: dv[Convert.ToInt32(r_gcjs_dl)][col].ToString();
                gcjs_fdl =(r_gcjs_fdl is null)?gcjs_fdl: dv[Convert.ToInt32(r_gcjs_fdl)][col].ToString();
                jsjc_dl =(r_jsjc_dl is null)?jsjc_dl: dv[Convert.ToInt32(r_jsjc_dl)][col].ToString();
                jsjc_fdl =(r_jsjc_fdl is null)?jsjc_fdl: dv[Convert.ToInt32(r_jsjc_fdl)][col].ToString();
                gckc= (r_gckc is null) ? gckc : dv[Convert.ToInt32(r_gckc)][col].ToString();
                gcsjf= (r_gcsjf is null) ? gcsjf : dv[Convert.ToInt32(r_gcsjf)][col].ToString();
                sjwjps= (r_sjwjps is null) ? sjwjps : dv[Convert.ToInt32(r_sjwjps)][col].ToString();
                bzbz = (r_bzbz is null) ? bzbz : dv[Convert.ToInt32(r_bzbz)][col].ToString();
                jsjc = (r_jsjc is null) ? jsjc : dv[Convert.ToInt32(r_jsjc)][col].ToString();
            }
            //赋值开始
            
            try
            {
                //string转换int
                catagorySet.pcc_other_xmgl.costValue = (string.IsNullOrEmpty(xmjs_dl) ? 0 : Convert.ToDouble(xmjs_dl)) + (string.IsNullOrEmpty(xmjs_fdl) ? 0 : Convert.ToDouble(xmjs_fdl))+ (string.IsNullOrEmpty(xmjs) ? 0 : Convert.ToDouble(xmjs));
                catagorySet.pcc_other_zb.costValue = string.IsNullOrEmpty(xmjs_zb) ? 0 : Convert.ToDouble(xmjs_zb);
                catagorySet.pcc_other_gcjl.costValue = (string.IsNullOrEmpty(jl_dl) ? 0 : Convert.ToDouble(jl_dl)) + (string.IsNullOrEmpty(jl_fdl) ? 0 : Convert.ToDouble(jl_fdl))+ (string.IsNullOrEmpty(jl) ? 0 : Convert.ToDouble(jl));
                //勘察
                catagorySet.pcc_other_kc.costValue = (string.IsNullOrEmpty(qq_dl) ? 0 : Convert.ToDouble(qq_dl)) + (string.IsNullOrEmpty(qq_fdl) ? 0 : Convert.ToDouble(qq_fdl))+ (string.IsNullOrEmpty(gckc) ? 0 : Convert.ToDouble(gckc));
                //工程设计费=基本设计费+施工图预算编制费+竣工图文件编制费
                catagorySet.pcc_other_sj.costValue = (string.IsNullOrEmpty(jbsj_fpd) ? 0 : Convert.ToDouble(jbsj_fpd)) + (string.IsNullOrEmpty(sgtys) ? 0 : Convert.ToDouble(sgtys)) + (string.IsNullOrEmpty(jgtys) ? 0 : Convert.ToDouble(jgtys))+ (string.IsNullOrEmpty(gcsjf) ? 0 : Convert.ToDouble(gcsjf));
                //2个评审
                catagorySet.pcc_other_ps.costValue = (string.IsNullOrEmpty(cswj) ? 0 : Convert.ToDouble(cswj)) + (string.IsNullOrEmpty(sgtwjsc) ? 0 : Convert.ToDouble(sgtwjsc)) + (string.IsNullOrEmpty(sjwjps) ? 0 : Convert.ToDouble(sjwjps));
                //2个后评价
                catagorySet.pcc_other_hpj.costValue = (string.IsNullOrEmpty(xmhpj_dl) ? 0 : Convert.ToDouble(xmhpj_dl)) + (string.IsNullOrEmpty(xmhpj_fdl) ? 0 : Convert.ToDouble(xmhpj_fdl));
                //技术经济标准编制管理费=2个结算审查
                catagorySet.pcc_other_bzbz.costValue = (string.IsNullOrEmpty(gcjs_dl) ? 0 : Convert.ToDouble(gcjs_dl)) + (string.IsNullOrEmpty(gcjs_fdl) ? 0 : Convert.ToDouble(gcjs_fdl))+ (string.IsNullOrEmpty(bzbz) ? 0 : Convert.ToDouble(bzbz));
                //工程建设监督检测费
                catagorySet.pcc_other_jdjc.costValue = (string.IsNullOrEmpty(jsjc_dl) ? 0 : Convert.ToDouble(jsjc_dl)) + (string.IsNullOrEmpty(jsjc_fdl) ? 0 : Convert.ToDouble(jsjc_fdl)) + (string.IsNullOrEmpty(jsjc) ? 0 : Convert.ToDouble(jsjc));

            }
            catch (Exception e)
            { MessageBox.Show(e.Message); }
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

        //private ProjectCostCatagory WitchNotNull()
        //{
        //    ProjectCostCatagory catagory = new ProjectCostCatagory();
        //    if (catagorySet.pcc_jk.costValue != 0)
        //    {
        //        catagory = catagorySet.pcc_jk;
        //    }
        //    else if (catagorySet.pcc_dl.costValue != 0)
        //    {
        //        catagory = catagorySet.pcc_dl;
        //    }
        //    else if (catagorySet.pcc_pd_az.costValue != 0)
        //    {
        //        catagory = catagorySet.pcc_pd_az;
        //    }
        //    else if (catagorySet.pcc_pd_sb.costValue != 0)
        //    {
        //        catagory = catagorySet.pcc_pd_sb;
        //    }
        //    else if (catagorySet.pcc_pd_jz.costValue != 0)
        //    {
        //        catagory = catagorySet.pcc_pd_jz;
        //    }
        //    else if (catagorySet.pcc_tx_az.costValue != 0)
        //    {
        //        catagory = catagorySet.pcc_tx_az;
        //    }
        //    else if (catagorySet.pcc_tx_sb.costValue != 0)
        //    {
        //        catagory = catagorySet.pcc_tx_sb;
        //    }
        //    else
        //    {
        //        catagory = catagorySet.pcc_tx_jz;
        //    }
        //    return catagory;
        //}
    }



}
