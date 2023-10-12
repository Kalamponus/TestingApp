using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.Infrastructure.Services.Interfaces;
using TestingApp.Models.TestModels;
using TestingApp.ViewModels.Base;

namespace TestingApp.ViewModels
{
    internal class TestRedactorMenuViewModel : ViewModel
    {
        #region Поля
        public List<string> Files { get; set; }
        public string SelectedTest {  get; set; }
        private Test test = new Test();
        #endregion

        #region Интерфейсы
        private ITestReader testReader { get; set; }
        #endregion

        #region Комманды
        public ICommand RedactTest {  get; }
        public ICommand NavigateTestRedactorCommand { get; }
        public ICommand NavigateRedactorMenuCommand { get; }
        public ICommand NavigateTestRedactorEmptyCommand { get; }

        #region NavigateTestRedactorEmptyCommand

        private bool CanRedactTestExecuted(object p) => true;

        private void OnRedactTestExecuted(object p)
        {
            if (!string.IsNullOrEmpty(SelectedTest))
            {
                testReader = new TestReader();
                test = testReader.ReadTestFile(SelectedTest);
                NavigateTestRedactorCommand.Execute(p);
            }            
        }

        #endregion

        #endregion
        public TestRedactorMenuViewModel(NavigationStore navigationStore)
        {
            Files = new List<string>();
            Files.AddRange(Directory.GetFiles("Tests").Where(x => x.EndsWith(".bin")));

            #region Команды

            NavigateTestRedactorCommand = new NavigateCommand<TestRedactorPageViewModel>(navigationStore, () => new TestRedactorPageViewModel(navigationStore, test));
            NavigateTestRedactorEmptyCommand = new NavigateCommand<TestRedactorPageViewModel>(navigationStore, () => new TestRedactorPageViewModel(navigationStore, new Test()));
            NavigateRedactorMenuCommand = new NavigateCommand<MainMenuPageViewModel>(navigationStore, () => new MainMenuPageViewModel(navigationStore));
            RedactTest = new LambdaCommand(OnRedactTestExecuted, CanRedactTestExecuted);

            #endregion
        }
    }
}
