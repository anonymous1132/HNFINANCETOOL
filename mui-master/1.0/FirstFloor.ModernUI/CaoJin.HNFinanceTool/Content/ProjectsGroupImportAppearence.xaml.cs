using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CaoJin.HNFinanceTool.Bll;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using System.Collections.ObjectModel;

namespace CaoJin.HNFinanceTool.Content
{
    /// <summary>
    /// ProjectsGroupImportAppearence.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectsGroupImportAppearence : UserControl
    {
        public ProjectsGroupImportAppearence()
        {
            InitializeComponent();
        }
        private ObservableCollection<ProjectGroupImportViewModel> obc_group;
        private ObservableCollection<ProjectFilesViewModel> obc_file;
        private void button_selectfile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx|Excel 97-2003 Files (*.xls)|*.xls" };
            openFile.Multiselect = true;
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return ;
            foreach (string filename in  openFile.FileNames)
            {
                if (!System.IO.File.Exists(filename)) continue;
                 ProjectGroupImportViewModel importViewModel=new ProjectGroupImportViewModel(filename);
                obc_group.Add(importViewModel);
            }
            this.DG1.ItemsSource = obc_group;
            this.button_import.IsEnabled = true;
        }

        private void button_import_Click(object sender, RoutedEventArgs e)
        {
            foreach (ProjectGroupImportViewModel importvm in obc_group)
            {
                importvm.OutputToFile();
            }
            this.button_import.IsEnabled = false;
            LoadLB1();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.obc_group = new ObservableCollection<ProjectGroupImportViewModel>();
            LoadLB1();
        }

        private void LB1_MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            ControlsStylesDataGrid cdg = new ControlsStylesDataGrid();
            //cdg.DataFileName = uri.ToString();
            cdg.isadd = false;
            cdg.DataFileName=((ProjectFilesViewModel)(LB1.SelectedItem)).ProjectName+".est";
             this.Content = cdg;
            //FirstFloor.ModernUI.Windows.Controls.ModernWindow modernWindow = new FirstFloor.ModernUI.Windows.Controls.ModernWindow();
            //modernWindow.ContentSource = FirstFloor.ModernUI.Windows.Navigation.NavigationHelper.ToUri(cdg);
            //modernWindow.ShowDialog();

        }

        private void LB1_MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {

            foreach (ProjectFilesViewModel pvm in LB1.SelectedItems)
            {
                File.Delete(pvm.FilePath);
            }
            LoadLB1();
            //  LB1.SelectedItem
        }

        private void LoadLB1()
        {
            this.obc_file = new ObservableCollection<ProjectFilesViewModel>();
            string filepath = @"App\data\";
            DirectoryInfo dir = new DirectoryInfo(filepath);
            if (dir.Exists)
            {
                FileInfo[] fiList = dir.GetFiles();
                foreach (FileInfo f in fiList)
                {
                    if (f.Extension == ".est")
                    {
                        ProjectFilesViewModel filesViewModel = new ProjectFilesViewModel(f.FullName);
                        obc_file.Add(filesViewModel);
                    }
                }
            }
            this.LB1.ItemsSource = obc_file;
        }
    }
}
