using System.Windows.Input;
using TestingApp.ViewModels.Base;
using System;
using TestingApp.Views.Pages;
using TestingApp.Infrastructure.Services;
using System.IO;

namespace TestingApp.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {

        NavigationStore navigationStore = new NavigationStore();

        public ViewModel? CurrentViewModel => navigationStore.CurrentViewModel;

        public MainWindowViewModel()
        {
            //Создаем папки для хранения тестов и картинок, если их нет
            if (!Directory.Exists("Tests")) Directory.CreateDirectory("Tests");
            if (!Directory.Exists("Images")) Directory.CreateDirectory("Images");

            //Определение объекта для навигации
            navigationStore.CurrentViewModel = new MainMenuPageViewModel(navigationStore);
            navigationStore.CurrentViewModelChanged += () => OnCurrentViewChanged();
            
        }

        private void OnCurrentViewChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
