using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingApp.ViewModels.Base;

namespace TestingApp.Infrastructure.Services
{
    internal class NavigationStore
    {
       public event Action CurrentViewModelChanged;

       private ViewModel? _currentViewModel;
       public ViewModel? CurrentViewModel 
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
