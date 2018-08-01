using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;


namespace CaoJin.HNFinanceTool.Bll
{
    public class ProjectFilesViewModel:NotifyPropertyChanged
    {
        public ProjectFilesViewModel(string filePath)
        {
            this.FilePath = filePath;
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; OnPropertyChanged("FilePath"); SetProjectName(); }
        }

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value;OnPropertyChanged("ProjectName"); }
        }

        private void SetProjectName()
        {
            System.IO.FileInfo file = new System.IO.FileInfo(_filePath);
            ProjectName = file.Name.Split('.')[0];
        }
    }
}
