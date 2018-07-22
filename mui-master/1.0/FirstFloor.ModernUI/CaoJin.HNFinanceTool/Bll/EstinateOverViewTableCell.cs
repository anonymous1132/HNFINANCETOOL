using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoJin.HNFinanceTool.Bll
{
   public class EstinateOverViewTableCell
    {
        public EstinateOverViewTableCell()
        { }
        public EstinateOverViewTableCell(string cname,int? x,int? y)
        {
            this.cell.Column = y;
            this.cell.Row = x;
            this.CellName = cname;
        }
        public EstinateOverViewTableCell(string cname)
        {
            CellName = cname;
        }
        public ExcelCellCoordinate cell = new ExcelCellCoordinate();
        public string CellName;

    }
}
