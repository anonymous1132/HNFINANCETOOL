﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaoJin.HNFinanceTool.Basement;
using Microsoft.Office.Interop.Excel;
using System.Data;

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
            excelHelper.QuitExcel(app, workbook);
        }

        int curerntRow = 3;

        //电网基建概算数导入模版
        public void PrintOneProjectEstimateBlcok(ProjectEstimateViewModel project)
        {
  
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
            Range rng= worksheet.Range["A" + curerntRow.ToString(), "N" + curerntRow.ToString()];
            excelHelper.SetRangeBodersStyle(rng, 1);
            excelHelper.SetRangeBodersThickness(rng, XlBorderWeight.xlThin);
            excelHelper.SetRowHeight(rng,20);
            rng = worksheet.Range["G" + curerntRow.ToString(), "H" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng,"0.00");
            rng= worksheet.Range["J" + curerntRow.ToString(), "N" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng,"0.00");
            rng= worksheet.Range["I" + curerntRow.ToString(), "I" + curerntRow.ToString()];
            rng.NumberFormatLocal = "0.00%";
            if(project is ProjectTotalEstimateViewModel)
            {
                rng = worksheet.Range["A" + curerntRow.ToString(), "N" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 34);
            }
            else
            {
                rng = worksheet.Range["A" + curerntRow.ToString(), "F" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 15);
                rng = worksheet.Range["L" + curerntRow.ToString(), "N" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 15);
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
            worksheet.Cells[curerntRow, "L"] = importvm.TotalInvestmentWithooutTax;
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
            excelHelper.SetRangeValueStyleNumber(rng, "0.00");
            rng = worksheet.Range["R" + curerntRow.ToString(), "S" + curerntRow.ToString()];
            excelHelper.SetRangeValueStyleNumber(rng, "0.00");
            rng = worksheet.Range["Q" + curerntRow.ToString(), "Q" + curerntRow.ToString()];
            rng.NumberFormatLocal = "0.00%";
            if (importvm.ExpanseCategory == "10KV（含20KV）及以下基建项目")
            {
                rng = worksheet.Range["A" + curerntRow.ToString(), "S" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 34);
            }
            else
            {
                rng = worksheet.Range["A" + curerntRow.ToString(), "L" + curerntRow.ToString()];
                excelHelper.SetRangeBackground(rng, 15);
            }

            curerntRow++;
        }
        
    }
}