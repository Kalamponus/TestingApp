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
    internal class MainMenuPageViewModel : ViewModel
    {

        public ICommand NavigateTestingPageCommand { get; }
        public ICommand NavigateTestRedactorCommand { get; }

        public MainMenuPageViewModel(NavigationStore navigationStore)
        {
            NavigateTestingPageCommand = new NavigateCommand<TestingPageViewModel>(navigationStore, () => new TestingPageViewModel(navigationStore));
            NavigateTestRedactorCommand = new NavigateCommand<TestRedactorPageViewModel>(navigationStore, () => new TestRedactorPageViewModel(navigationStore));
        }

        
    }
}
