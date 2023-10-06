using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.ViewModels.Base;

namespace TestingApp.ViewModels
{
    internal class TestingPageViewModel : ViewModel
    {

        public ICommand NavigateMainMenuPageCommand { get; }

        public TestingPageViewModel(NavigationStore navigationStore)
        {
            NavigateMainMenuPageCommand = new NavigateCommand<MainMenuPageViewModel>(navigationStore, () => new MainMenuPageViewModel(navigationStore));
        }
    }
}
