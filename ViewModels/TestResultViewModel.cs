using System;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.ViewModels.Base;

namespace TestingApp.ViewModels
{
    internal class TestResultViewModel : ViewModel
    {
        #region Поля

        public string CurrentFinalResult { get; set; }

        #endregion

        #region Команды

        public ICommand NavigateMainMenuPageCommand { get; }

        #endregion

        public TestResultViewModel(NavigationStore navigationStore, double FinalResult, int MaxResult)
        {
            #region Команды

            CurrentFinalResult = Math.Round(FinalResult, 2).ToString() + "/" + MaxResult.ToString();
            NavigateMainMenuPageCommand = new NavigateCommand<MainMenuPageViewModel>(navigationStore, () => new MainMenuPageViewModel(navigationStore));

            #endregion
        }
    }
}
