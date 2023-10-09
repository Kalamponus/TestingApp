using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.Models.TestModels;
using TestingApp.ViewModels.Base;

namespace TestingApp.ViewModels
{
    internal class TestRedactorPageViewModel : ViewModel
    {
        #region Поля
        public Test CurrentTest { get; set; }
        public int QuestionCount {  get; set; }
        #endregion


        #region Команды
        public ICommand AddTextQuestion { get; }

        private bool CanAddTextQuestionExecuted(object p) => true;

        private void OnAddTextQuestionExecuted(object p)
        {
            CurrentTest.Questions.Add(new Question 
            { 
                QuestionNumber = QuestionCount, 
                QuestionType = QuestionType.TextQuestion, 
                Answers = new ObservableCollection<Answer>() 
            });
        }

        public ICommand AddImageQuestion { get; }

        private bool CanAddImageQuestionExecuted(object p) => true;

        private void OnAddImageQuestionExecuted(object p)
        {
            CurrentTest.Questions.Add(new Question 
            { 
                QuestionNumber = QuestionCount, 
                QuestionType = QuestionType.ImageQuestion, 
                Answers = new ObservableCollection<Answer>() 
            });
        }

        public ICommand AddMultiChoiseQuestion { get; }
        private bool CanAddMultiChoiseQuestionExecuted(object p) => true;

        private void OnAddMultiChoiseQuestionExecuted(object p)
        {
            CurrentTest.Questions.Add(new Question 
            { 
                QuestionNumber = QuestionCount, 
                QuestionType = QuestionType.MultiChoiseQuestion, 
                Answers = new ObservableCollection<Answer>() 
            });
        }

        public ICommand AddAnswerCommand { get; }
        private bool CanAddAnswerExecuted(object p) => true;

        private void OnAddAnswerExecuted(object p)
        {
            Question question = p as Question;            
            question.Answers.Add(new Answer());
            Debug.WriteLine(question.Answers.Count);
        }

        #endregion

        public TestRedactorPageViewModel(NavigationStore navigationStore)
        {
            CurrentTest = new Test();
            CurrentTest.Questions = new ObservableCollection<Question>();
            AddTextQuestion = new LambdaCommand(OnAddTextQuestionExecuted, CanAddTextQuestionExecuted);
            AddImageQuestion = new LambdaCommand(OnAddImageQuestionExecuted, CanAddImageQuestionExecuted);
            AddMultiChoiseQuestion = new LambdaCommand(OnAddMultiChoiseQuestionExecuted, CanAddMultiChoiseQuestionExecuted);
            AddAnswerCommand = new LambdaCommand(OnAddAnswerExecuted, CanAddAnswerExecuted);
        }

        public TestRedactorPageViewModel()
        {

        }
    }
}
