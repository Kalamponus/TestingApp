using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.Infrastructure.Services.Interfaces;
using TestingApp.Models.TestModels;
using TestingApp.ViewModels.Base;

namespace TestingApp.ViewModels
{
    internal class TestMenuViewModel : ViewModel
    {
        #region Поля
        public List<string> Files { get; set; }
        public string SelectedTest { get; set; }
        private Test test = new Test();
        #endregion

        #region Интерфейсы
        private ITestReader testReader { get; set; }
        #endregion

        #region Комманды
        public ICommand StartTest { get; }
        public ICommand NavigateMainMenuCommand { get; }
        public ICommand NavigateTestingPageCommand { get; }

        #region NavigateTestingPageCommand

        private bool CanStartTestExecuted(object p) => true;

        private void OnStartTestExecuted(object p)
        {
            if (!string.IsNullOrEmpty(SelectedTest))
            {
                testReader = new TestReader();
                test = testReader.ReadTestFile(SelectedTest);
                NavigateTestingPageCommand.Execute(p);
            }
        }

        #endregion

        #endregion
        public TestMenuViewModel (NavigationStore navigationStore)
        {
            Files = new List<string>();
            Files.AddRange(Directory.GetFiles("Tests").Where(x => x.EndsWith(".bin")));

            #region Команды

            NavigateTestingPageCommand = new NavigateCommand<TestingPageViewModel>(navigationStore, () => new TestingPageViewModel(navigationStore, test));
            NavigateMainMenuCommand = new NavigateCommand<MainMenuPageViewModel>(navigationStore, () => new MainMenuPageViewModel(navigationStore));
            StartTest = new LambdaCommand(OnStartTestExecuted, CanStartTestExecuted);

            #endregion
        }
    }
}
