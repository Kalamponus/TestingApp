using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.ViewModels.Base;

namespace TestingApp.ViewModels
{
    internal class MainMenuPageViewModel : ViewModel
    {
        #region Команды
        public ICommand NavigateTestMenuCommand { get; }
        public ICommand NavigateTestRedactorMenuCommand { get; }
        public ICommand CloseApplicationCommand { get; }

        #region CloseApplicationCommand

        private bool CanCloseApplicationCommandExecuted(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #endregion
        public MainMenuPageViewModel(NavigationStore navigationStore)
        {
            #region Команды
            NavigateTestMenuCommand = new NavigateCommand<TestMenuViewModel>(navigationStore, () => new TestMenuViewModel(navigationStore));
            NavigateTestRedactorMenuCommand = new NavigateCommand<TestRedactorMenuViewModel>(navigationStore, () => new TestRedactorMenuViewModel(navigationStore));
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);
            #endregion
        }


    }
}
