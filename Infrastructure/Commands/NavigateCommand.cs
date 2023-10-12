using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingApp.Infrastructure.Commands.Base;
using TestingApp.Infrastructure.Services;
using TestingApp.ViewModels.Base;

namespace TestingApp.Infrastructure.Commands
{
    /// <summary>
    /// Комманда для переключения между viewmodel
    /// </summary>
    /// <typeparam name="TViewModel">Viewmodel, на которую нужно переключиться</typeparam>
    internal class NavigateCommand<TViewModel> : Command where TViewModel : ViewModel
    {

        private readonly NavigationStore? _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigateCommand(NavigationStore? navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public override bool CanExecute(object? parameter) => true;        

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
