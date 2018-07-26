using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;
using System.Collections.ObjectModel;
using System.Data;

namespace CaoJin.HNFinanceTool.Bll
{
   public class ProjectEstimateSetViewModel:NotifyPropertyChanged
    {
        public ProjectEstimateSetViewModel()
        { }

        private ProjectTotalEstimateViewModel _totalEstimateViewModel;
        public ProjectTotalEstimateViewModel TotalEstimateViewModel
        {
            get { return _totalEstimateViewModel; }
            set { _totalEstimateViewModel = value;OnPropertyChanged("TotalEstimateViewModel"); }
        }

        public ObservableCollection<ProjectEstimateViewModel> EstimateViewModels;

        public void SetEstimateViewModels(DataTable dataTable)
        {
            EstimateViewModels.Add(_totalEstimateViewModel);
        }
    }
}
