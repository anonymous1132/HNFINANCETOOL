using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows;
using CaoJin.HNFinanceTool.Content;

namespace CaoJin.HNFinanceTool
{
   public class NewProjectPageLoader:DefaultContentLoader
    {
        /// <summary>
        /// Loads the content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri</param>
        /// <returns>The loaded content.</returns>
        protected override object LoadContent(Uri uri)
        {
            ControlsStylesDataGrid csdg = new ControlsStylesDataGrid();
            csdg.DataFileName = "mould";
            return csdg;
        }
    }
}
