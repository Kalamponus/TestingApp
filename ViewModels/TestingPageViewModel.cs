using System;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.ViewModels.Base;
using TestingApp.Models.TestModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TestingApp.ViewModels
{
    internal class TestingPageViewModel : ViewModel
    {
        #region Поля
        public Test CurrentTest { get; set; }
        public string SomeTest { get; set; }
        public double FinalResult { get; set; }
        public int MaxResult { get; set; }
        #endregion

        #region Комманды       
        public ICommand NavigateMainMenuPageCommand { get; }  
        public ICommand NavigateFinalResultCommand { get; }
        public ICommand FinishTest { get; }
        
        private bool CanFinishTestCommandExecuted(object p) => true;

        private void OnFinishTestCommandExecuted(object p)
        {
            FinalResult = 0;
            foreach (Question question in CurrentTest.Questions)
            {
                int selectedCorrectAnswers = 0;
                foreach(Answer answer in question.Answers)
                {
                    if (!string.IsNullOrEmpty(answer.AnswerText))
                    {
                        if (question.QuestionType == QuestionType.TextQuestion && question.CorrectAnswers.Contains(answer.AnswerText.Trim().ToLower()))
                        {
                            FinalResult++;
                        }
                        else if (answer.IsSelected == true)
                        {
                            if (question.CorrectAnswers.Contains(answer.AnswerText))
                            {
                                selectedCorrectAnswers++;
                            }
                            else if (selectedCorrectAnswers > 0)
                            {
                                selectedCorrectAnswers--;
                            }
                        }
                    }                   
                }
                if (selectedCorrectAnswers > 0) FinalResult += (double)1 / question.CorrectAnswers.Count * selectedCorrectAnswers;
            }

            NavigateFinalResultCommand.Execute(p);
        }
        #endregion

        public TestingPageViewModel(NavigationStore navigationStore, Test test)
        {

            CurrentTest = test;
            foreach (Question question in CurrentTest.Questions.Where(x => x.QuestionType == QuestionType.TextQuestion).ToList())
            {
                question.Answers = new ObservableCollection<Answer>() { new Answer() };
            }
            MaxResult = CurrentTest.Questions.Count;

            NavigateMainMenuPageCommand = new NavigateCommand<MainMenuPageViewModel>(navigationStore, () => new MainMenuPageViewModel(navigationStore));
            NavigateFinalResultCommand = new NavigateCommand<TestResultViewModel>(navigationStore, () => new TestResultViewModel(navigationStore, FinalResult, MaxResult));
            FinishTest = new LambdaCommand(OnFinishTestCommandExecuted, CanFinishTestCommandExecuted);
        }

    }

}
