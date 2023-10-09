using System;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.ViewModels.Base;

namespace TestingApp.ViewModels
{
    internal class TestResultViewModel : ViewModel
    {
        public string CurrentFinalResult { get; set; }

        public ICommand NavigateMainMenuPageCommand { get; }

        public TestResultViewModel(NavigationStore navigationStore, double FinalResult, int MaxResult)
        {
            CurrentFinalResult = Math.Round(FinalResult, 2).ToString() + "/" + MaxResult.ToString();
            NavigateMainMenuPageCommand = new NavigateCommand<MainMenuPageViewModel>(navigationStore, () => new MainMenuPageViewModel(navigationStore));
        }
    }
}
