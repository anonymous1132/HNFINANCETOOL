using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using CaoJin.HNFinanceTool.Pages;

namespace CaoJin.HNFinanceTool.Content
{
    /// <summary>
    /// Interaction logic for LoremIpsumList.xaml
    /// </summary>
    public partial class LoremIpsumList : UserControl
    {
        public LoremIpsumList()
        {
            InitializeComponent();
           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           // Link homelink = new Link { DisplayName = "Home", Source = new Uri("/Pages/Introduction.xaml", UriKind.Relative) };
            mylist.Links.Clear();
         //   mylist.Links.Add(homelink);
            string filepath = @"App\data\";
            System.IO.DirectoryInfo dir = new DirectoryInfo(filepath);
            if (dir.Exists)
            {
                FileInfo[] fiList = dir.GetFiles();
               // int i = 1;
                foreach (FileInfo f in fiList)
                {
                    if (f.Extension == ".est")
                    {
                        //if (i > 10) { break; }
                        Link link = new Link();
                        link.DisplayName =f.Name.Split('.')[0];
                        link.Source = new Uri(f.Name, UriKind.Relative);
                        this.mylist.Links.Add(link);
                       
                      //  i++;
                    }

                }
            }
            this.mylist.SelectedSource =new Uri("home", UriKind.Relative);
        }
    }
}
