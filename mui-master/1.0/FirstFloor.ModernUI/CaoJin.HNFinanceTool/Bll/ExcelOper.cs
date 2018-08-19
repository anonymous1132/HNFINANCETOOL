using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaoJin.HNFinanceTool.Basement;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Runtime.InteropServices;

namespace CaoJin.HNFinanceTool.Bll
{
    public class ExcelOper
    {
        public ExcelOper(string filepath)
        {
            _filePath = filepath;
        }

        private Application _app;
        public Application app
        {
            get
            {
                if (_app == null)
                {
                    _app = excelHelper.app();
                    return _app;
                }
                else { return _app; }
            }
        }

        private ExcelAppHelper excelHelper = new ExcelAppHelper();
        private string _filePath;

        private Workbook _workbook;
        private Workbook workbook
        {
            get
            {
                if (_workbook == null) { _workbook = excelHelper.GetWorkbook(_filePath, app); return _workbook; }
                else { return _workbook; }
            }
        }

        private Worksheet _worksheet;
        private Worksheet worksheet
        {
            get
            {
                if (_worksheet == null) { _worksheet = excelHelper.GetWorksheet(workbook, 1); }
                return _worksheet;
            }
        }

        public DataSet dataset
        {
            get { return excelHelper.GetContent(_filePath); }
        }
        public void Save()
        { excelHelper.Save(workbook, _filePath); }

        public void Quit()
        {
            // excelHelper.QuitExcel(app, workbook);
            CloseExcel(app,workbook);
        }

        /// <summary>
        /// 关闭Excel进程
        /// </summary>
        private class KeyMyExcelProcess
        {
            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
            public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
            {
                try
                {
                    IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 
                    int k = 0;
                    GetWindowThreadProcessId(t, out k);   //得到本进程唯一标志k
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
                    p.Kill();     //关闭进程k
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }


        //关闭打开的Excel方法
        private void CloseExcel(Microsoft.Office.Interop.Excel.Application ExcelApplication, Microsoft.Office.Interop.Excel.Workbook ExcelWorkbook)
        {
            ExcelWorkbook.Close(false, Type.Missing, Type.Missing);
            ExcelWorkbook = null;
            ExcelApplication.Quit();
            GC.Collect();
            KeyMyExcelProcess.Kill(ExcelApplication);
        }




        int curerntRow = 3;

        //电网基建概算数导入模版
        public void PrintOneProjectEstimateBlcok(ProjectEstimateViewModel project)
        {
            Range rng = worksheet.Range["A" + curerntRow.ToString(), "N" + curerntRow.ToString()];
            excelHelper.SetRangeBodersStyle(rng, 1);
            excelHelper.SetRangeBodersThickness(rng, XlBorderWeight.xlThin);
            excelHelper.SetRowHeight(rng, 20);
            rng = worksheet.Range["G" + curerntRow.ToString(), "H" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng, "#,##0.00");
            rng = worksheet.Range["J" + curerntRow.ToString(), "N" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng, "#,##0.00");
            rng = worksheet.Range["I" + curerntRow.ToString(), "I" + curerntRow.ToString()];
            rng.NumberFormatLocal = "0.00%";
            if (project is ProjectTotalEstimateViewModel)
            {
                rng = worksheet.Range["A" + curerntRow.ToString(), "N" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 35);
                worksheet.Cells[curerntRow, "A"] = project.ProjectName;
                worksheet.Cells[curerntRow, "B"] = project.ProjectCode;
                worksheet.Cells[curerntRow, "C"] = ((ProjectTotalEstimateViewModel)project).IndividualProjectName;
                worksheet.Cells[curerntRow, "D"] = ((ProjectTotalEstimateViewModel)project).IndividualProjectCode;
                worksheet.Cells[curerntRow, "E"] = ((ProjectTotalEstimateViewModel)project).ExpanseCategory;
                worksheet.Cells[curerntRow, "F"] = ((ProjectTotalEstimateViewModel)project).WBSCode;
                worksheet.Cells[curerntRow, "G"] = ((ProjectTotalEstimateViewModel)project).EstimateNumber;
                worksheet.Cells[curerntRow, "H"] = ((ProjectTotalEstimateViewModel)project).InternalControl;
                worksheet.Cells[curerntRow, "I"] = ((ProjectTotalEstimateViewModel)project).DeductibleVATRatio;
                worksheet.Cells[curerntRow, "J"] = ((ProjectTotalEstimateViewModel)project).TotalInvestmentWithTax;
                worksheet.Cells[curerntRow, "K"] = ((ProjectTotalEstimateViewModel)project).TotalInvestmentWithoutTax;
                worksheet.Cells[curerntRow, "L"] = ((ProjectTotalEstimateViewModel)project).MaxInternalControl;
                worksheet.Cells[curerntRow, "M"] = ((ProjectTotalEstimateViewModel)project).MaxDeductibleVATRatio;
                worksheet.Cells[curerntRow, "N"] = ((ProjectTotalEstimateViewModel)project).MinDeductibleVATRatio;
            }
            else
            {
                rng = worksheet.Range["A" + curerntRow.ToString(), "F" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 15);
                rng = worksheet.Range["L" + curerntRow.ToString(), "N" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 15);
                worksheet.Cells[curerntRow, "A"] = project.ProjectName;
                worksheet.Cells[curerntRow, "B"] = project.ProjectCode;
                worksheet.Cells[curerntRow, "C"] = project.IndividualProjectName;
                worksheet.Cells[curerntRow, "D"] = project.IndividualProjectCode;
                worksheet.Cells[curerntRow, "E"] = project.ExpanseCategory;
                worksheet.Cells[curerntRow, "F"] = project.WBSCode;
                worksheet.Cells[curerntRow, "G"] = project.EstimateNumber;
                worksheet.Cells[curerntRow, "H"] = project.InternalControl;
                worksheet.Cells[curerntRow, "I"] = project.DeductibleVATRatio;
                worksheet.Cells[curerntRow, "J"] = project.TotalInvestmentWithTax;
                worksheet.Cells[curerntRow, "K"] = project.TotalInvestmentWithoutTax;
                worksheet.Cells[curerntRow, "L"] = project.MaxInternalControl;
                worksheet.Cells[curerntRow, "M"] = project.MaxDeductibleVATRatio;
                worksheet.Cells[curerntRow, "N"] = project.MinDeductibleVATRatio;
            }

                
           
           
            
            curerntRow++;
        }

        public void PrintOneImportBlock(ProjectImportMouldViewModel importvm)
        {
            worksheet.Cells[curerntRow, "A"] = importvm.ProjectName;
            worksheet.Cells[curerntRow, "B"] = importvm.ProjectCode;
            worksheet.Cells[curerntRow, "C"] = importvm.IndividualProjectName;
            worksheet.Cells[curerntRow, "D"] = importvm.IndividualProjectCode;
            worksheet.Cells[curerntRow, "E"] = importvm.ExpanseCategory;
            worksheet.Cells[curerntRow, "F"] = importvm.WBSCode;
            worksheet.Cells[curerntRow, "G"] = importvm.ConstructionStage;
            worksheet.Cells[curerntRow, "H"] = importvm.PrestandardVersion;
            worksheet.Cells[curerntRow, "I"] = importvm.VLevel;
            worksheet.Cells[curerntRow, "J"] = importvm.SingleProjectClass;
            worksheet.Cells[curerntRow, "K"] = importvm.TotalInvestmentWithTax;
            worksheet.Cells[curerntRow, "L"] = importvm.TotalInvestmentWithoutTax;
            worksheet.Cells[curerntRow, "M"] = importvm.CumulativeCost;
            worksheet.Cells[curerntRow, "N"] = importvm.YearCost;
            worksheet.Cells[curerntRow, "O"] = importvm.CumulativedeDeductibleVAT;
            worksheet.Cells[curerntRow, "P"] = importvm.YearDeductibleVAT;
            worksheet.Cells[curerntRow, "Q"] = importvm.DeductibleVATRatio;
            worksheet.Cells[curerntRow, "R"] = importvm.DepartmentFilledBudgetWithTax;
            worksheet.Cells[curerntRow, "S"] = importvm.YearBudgetWithoutTax;

            Range rng = worksheet.Range["A" + curerntRow.ToString(), "S" + curerntRow.ToString()];
            excelHelper.SetRangeBodersStyle(rng, 1);
            excelHelper.SetRangeBodersThickness(rng, XlBorderWeight.xlThin);
            excelHelper.SetRowHeight(rng, 20);
            rng = worksheet.Range["K" + curerntRow.ToString(), "P" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng, "#,##0.00");
            rng = worksheet.Range["R" + curerntRow.ToString(), "S" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng, "#,##0.00");
            rng = worksheet.Range["Q" + curerntRow.ToString(), "Q" + curerntRow.ToString()];
            rng.NumberFormatLocal = "0.00%";
            if (importvm.ExpanseCategory == "10KV（含20KV）及以下基建项目")
            {
                rng = worksheet.Range["A" + curerntRow.ToString(), "S" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 35);
            }
            else
            {
                rng = worksheet.Range["A" + curerntRow.ToString(), "L" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 15);
            }

            curerntRow++;
        }

        //项目部门填报表
        public void PrintOneDepartmentBudgetFilledBlock(DepartmentBudgetFilled department)
        {
            worksheet.Cells[curerntRow, "A"] = department.NumberOnly;
            worksheet.Cells[curerntRow, "B"] = department.ProjectCode;
            worksheet.Cells[curerntRow, "C"] = department.ProjectName;
            worksheet.Cells[curerntRow, "D"] = department.MaxBudgetWithTax;
            worksheet.Cells[curerntRow, "E"] = department.MaxBudgetWithoutTax;
            worksheet.Cells[curerntRow, "F"] = department.DepartmentFilledBudgetWithTax;
            //worksheet.Cells[curerntRow, "G"] = department.YearBudgetWithoutTax;
           
            //设置格式
            Range rng = worksheet.Range["A" + curerntRow.ToString(), "I" + curerntRow.ToString()];
            excelHelper.SetRangeBodersStyle(rng, 1);
            excelHelper.SetRangeBodersThickness(rng, XlBorderWeight.xlThin);
            excelHelper.SetRowHeight(rng, 20);
            excelHelper.SetFontHVCenter(rng);
            string formula = @"=IF(F"+curerntRow.ToString()+"<=D"+curerntRow.ToString()+",TRUE,FALSE)";
            rng = worksheet.Range["H" + curerntRow.ToString(), "H" + curerntRow.ToString()];
            excelHelper.SetRangeFormula(rng,formula);
            formula = @"=IF(G" + curerntRow.ToString() + "<=E" + curerntRow.ToString() + ",TRUE,FALSE)";
            rng = worksheet.Range["I" + curerntRow.ToString(), "I" +curerntRow.ToString()];
            excelHelper.SetRangeFormula(rng, formula);
            formula = @"=F"+curerntRow.ToString()+" / (1 +"+department.CompositeTaxRate +"/ 100)";
            rng = worksheet.Range["G" + curerntRow.ToString(), "G" + curerntRow.ToString()];
            excelHelper.SetRangeFormula(rng,formula);
            rng = worksheet.Range["D" + curerntRow.ToString(), "G" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng, "#,##0.00");

            //设置条件格式
            formula = "=INDIRECT(CONCATENATE(\"R\",ROW(),\"C8\"),FALSE) = FALSE";
            rng = worksheet.Range["H" + curerntRow.ToString(), "H" + curerntRow.ToString()];
            excelHelper.SetRangeConditionFormat(rng, formula);
            formula = "=INDIRECT(CONCATENATE(\"R\",ROW(),\"C9\"),FALSE) = FALSE";
            rng = worksheet.Range["I" + curerntRow.ToString(), "I" + curerntRow.ToString()];
            excelHelper.SetRangeConditionFormat(rng, formula);
            curerntRow++;
            
        }

        //预算上限计算表
        public void PrintOneBudgetaryUpperLimitBlock(BudgetaryUpperLimit limit)
        {
            worksheet.Cells[curerntRow, "A"] = limit.ProjectCode;
            worksheet.Cells[curerntRow, "B"] = limit.ProjectName;
            worksheet.Cells[curerntRow, "C"] = limit.AccumulativePlan;
            worksheet.Cells[curerntRow, "D"] = limit.ErpHappenedWithoutTax;
            worksheet.Cells[curerntRow, "E"] = limit.DeductibleVAT;
            Range rng = worksheet.Range["A" + curerntRow.ToString(), "E" + curerntRow.ToString()];
            excelHelper.SetRangeBodersStyle(rng, 1);
            excelHelper.SetRangeBodersThickness(rng, XlBorderWeight.xlThin);
            excelHelper.SetRowHeight(rng, 20);
            excelHelper.SetFontHVCenter(rng);
            rng = worksheet.Range["C" + curerntRow.ToString(), "E" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng, "#,##0.00");
            curerntRow++;
        }
    }
}
