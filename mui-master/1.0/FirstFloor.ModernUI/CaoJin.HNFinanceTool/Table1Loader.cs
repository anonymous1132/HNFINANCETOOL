using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows;
using CaoJin.HNFinanceTool.Content;

namespace CaoJin.HNFinanceTool
{
   public class Table1Loader:DefaultContentLoader
    {
        /// <summary>
        /// Loads the content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri</param>
        /// <returns>The loaded content.</returns>
        protected override object LoadContent(Uri uri)
        {
            if (uri.ToString() == "home")
            {
                return new Pages.Introduction();
            }
            else if (uri.ToString().Contains(".est"))
            {
                ControlsStylesDataGrid cdg = new ControlsStylesDataGrid();
                cdg.DataFileName = uri.ToString();
                cdg.isadd = false;
                return cdg;
            }
            else { return new ControlsStylesDataGrid(); }
            
        }

    }
}
