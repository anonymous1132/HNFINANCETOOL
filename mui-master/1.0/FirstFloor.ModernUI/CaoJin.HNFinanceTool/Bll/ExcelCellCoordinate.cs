using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoJin.HNFinanceTool.Bll
{
    public class ExcelCellCoordinate
    {
        public ExcelCellCoordinate(int? x,int? y)
        {
            this.Row = x;
            this.Column = y;
        }
        public ExcelCellCoordinate()
        { }
       public int? Row;
       public int? Column;
    }
}
