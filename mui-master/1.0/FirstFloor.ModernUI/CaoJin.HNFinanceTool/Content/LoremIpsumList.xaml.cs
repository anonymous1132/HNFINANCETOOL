using System;
using System.Collections.Generic;
using CaoJin.HNFinanceTool.Dal;
using System.Windows;
using System.Windows.Controls;
using System.Data;
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
                foreach (FileInfo f in fiList)
                {
                    if (f.Extension == ".est")
                    {
                        using (DataSet ds = XmlOperate.GetDataSet(f.FullName))
                        {
                            Link link = new Link();

                            link.DisplayName = f.Name.Split('.')[0] +"\n"+ ds.Tables[0].DefaultView[0]["ProjectCode"].ToString();
                            link.Source = new Uri(f.Name, UriKind.Relative);
                            this.mylist.Links.Add(link);
                        }
                       
                       
                    }

                }
            }
            this.mylist.SelectedSource =new Uri("home", UriKind.Relative);
        }
    }
}
