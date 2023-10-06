using System.Windows.Input;
using TestingApp.ViewModels.Base;
using System;
using TestingApp.Views.Pages;
using TestingApp.Infrastructure.Services;

namespace TestingApp.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {

        NavigationStore navigationStore = new NavigationStore();

        public ViewModel? CurrentViewModel => navigationStore.CurrentViewModel;

        public MainWindowViewModel()
        {
            navigationStore.CurrentViewModel = new MainMenuPageViewModel(navigationStore);
            navigationStore.CurrentViewModelChanged += () => OnCurrentViewChanged();
            
        }

        private void OnCurrentViewChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
