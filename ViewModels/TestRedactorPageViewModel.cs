using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.Infrastructure.Services.Interfaces;
using TestingApp.Models.TestModels;
using TestingApp.ViewModels.Base;

namespace TestingApp.ViewModels
{
    internal class TestRedactorPageViewModel : ViewModel
    {
        #region Поля
        public Test CurrentTest { get; set; }
        public int QuestionCount {  get; set; }
        public BitmapImage LoadedImage { get; set; }
        #endregion

        #region Интерфейсы
        private ITestWriter Writer { get; set; }
        #endregion

        #region Команды

        public ICommand NavigateRedactorMenuCommand { get; }

        public ICommand AddTextQuestion { get; }

        #region AddTextQuestion

        private bool CanAddTextQuestionExecuted(object p) => true;

        private void OnAddTextQuestionExecuted(object p)
        {
            QuestionCount++;
            CurrentTest.Questions.Add(new Question
            {
                QuestionNumber = QuestionCount,
                QuestionType = QuestionType.TextQuestion,
                Answers = new ObservableCollection<Answer>(),
                CorrectAnswers = new List<string>() { "" }
            });
            
        }

        #endregion

        public ICommand AddImageQuestion { get; }

        #region AddImageQuestion

        private bool CanAddImageQuestionExecuted(object p) => true;

        private void OnAddImageQuestionExecuted(object p)
        {
            QuestionCount++;
            CurrentTest.Questions.Add(new Question 
            { 
                QuestionNumber = QuestionCount, 
                QuestionType = QuestionType.ImageQuestion, 
                Answers = new ObservableCollection<Answer>()
            });
        }

        #endregion

        public ICommand AddMultiChoiseQuestion { get; }

        #region AddMultiChoiseQuestion

        private bool CanAddMultiChoiseQuestionExecuted(object p) => true;

        private void OnAddMultiChoiseQuestionExecuted(object p)
        {
            QuestionCount++;
            CurrentTest.Questions.Add(new Question 
            { 
                QuestionNumber = QuestionCount, 
                QuestionType = QuestionType.MultiChoiseQuestion, 
                Answers = new ObservableCollection<Answer>()              
            });
        }

        #endregion

        public ICommand AddAnswerCommand { get; }

        #region AddAnswerCommand

        private bool CanAddAnswerExecuted(object p) => true;

        private void OnAddAnswerExecuted(object p)
        {
            Question question = p as Question;            
            question.Answers.Add(new Answer());
            Debug.WriteLine(question.Answers.Count);
        }

        #endregion

        public ICommand SaveTest { get; }

        #region SaveTest

        private bool CanSaveTestExecuted(object p) => true;

        private void OnSaveTestExecuted(object p)
        {
            if(CheckFields() == true)
            {
                foreach (Question question in CurrentTest.Questions)
                {
                    if (question.QuestionType == QuestionType.TextQuestion) question.CorrectAnswers[0] = question.CorrectAnswers[0].ToLower().Trim(); 

                    else
                    {
                        question.CorrectAnswers = new List<string>();
                        foreach (Answer answer in question.Answers)
                        {
                            if (answer.IsSelected == true)
                            {
                                question.CorrectAnswers.Add(answer.AnswerText);
                                answer.IsSelected = false;
                            }
                        }
                    }
                                       
                }
                CurrentTest.Questions = new ObservableCollection<Question>(CurrentTest.Questions.OrderBy(x=>x.QuestionNumber).ToList());
                Writer = new TestWriter();
                Writer.Save(CurrentTest, "Tests");
                NavigateRedactorMenuCommand.Execute(p);
            }           
        }

        #endregion

        public ICommand UploadQuestionImage { get; }

        #region UploadQuestionImage

        private bool CanUploadQuestionImageExecuted(object p) => true;

        private void OnUploadQuestionImageExecuted(object p)
        {
            Question question = p as Question;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                if(!File.Exists("Images\\" + op.SafeFileName)) File.Copy(op.FileName, "Images\\" + op.SafeFileName);
                question.QuestionImage = "Images\\" + op.SafeFileName;
            }
        }

        #endregion        

        public ICommand UploadAnswerImage { get; }

        #region UploadAnswerImage

        private bool CanUploadAnswerImageExecuted(object p) => true;

        private void OnUploadAnswerImageExecuted(object p)
        {
            Answer answer = p as Answer;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                if (!File.Exists("Images\\" + op.SafeFileName)) File.Copy(op.FileName, "Images\\" + op.SafeFileName);
                answer.AnswerText = "Images\\" + op.SafeFileName;
            }
        }

        #endregion

        public ICommand RemoveAnswers { get; }

        #region RemoveAnswers

        private bool CanRemoveAnswerExecuted(object p) => true;

        private void OnRemoveAnswerExecuted(object p)
        {
            Question question = p as Question;
            List<Answer>removable = new List<Answer>();
            foreach (Answer answer in question.Answers)
            {
                if (answer.IsSelected) removable.Add(answer);
            }
            foreach (Answer answer in removable)
            {
                question.Answers.Remove(answer);
            }
        }

        #endregion

        public ICommand RemoveQuestion { get; }

        #region RemoveQuestion

        private bool CanRemoveQuestionExecuted(object p) => true;

        private void OnRemoveQuestionExecuted(object p)
        {
            Question question = p as Question;
            CurrentTest.Questions.Remove(question);
        }

        #endregion

        #endregion

        #region Методы

        #region CheckFields
        /// <summary>
        /// Проверка корректности вводных данных
        /// </summary>
        private bool CheckFields()
        {
            if (CurrentTest == null || string.IsNullOrEmpty(CurrentTest.Name) || File.Exists("Tests\\" + CurrentTest.Name)) return false;            
            if (CurrentTest.Questions.Select(x => x.QuestionNumber).Distinct().Count() != CurrentTest.Questions.Select(x => x.QuestionNumber).Count()) return false;
            if (CheckAnswers() == false) return false;
            return true;
        }

        #endregion

        #region CheckAnswers
        /// <summary>
        /// Проверка корректности ведённых вариантов ответа
        /// </summary>
        private bool CheckAnswers()
        {
            foreach(Question question in CurrentTest.Questions)
            {
                if (string.IsNullOrEmpty(question.QuestionText) || question.QuestionNumber == 0) return false;
                if (question.QuestionType != QuestionType.TextQuestion)
                {
                    if (question.Answers == null || question.Answers.Count < 2) return false;
                    bool hasCorrectAnswers = false;
                    foreach (Answer answer in question.Answers)
                    {
                        if (answer.IsSelected == true)
                        {
                            hasCorrectAnswers = true;
                            break;
                        }
                    }
                    if (hasCorrectAnswers == false) return false;
                }
                else
                {                    
                    if (string.IsNullOrEmpty(question.CorrectAnswers[0])) return false;
                }              
            }
            return true;
        }

        #endregion

        #endregion

        public TestRedactorPageViewModel(NavigationStore navigationStore, Test test)
        {
            CurrentTest = test;

            if (CurrentTest.Questions == null) CurrentTest.Questions = new ObservableCollection<Question>();
            else QuestionCount = CurrentTest.Questions.Count;

            //Выделение правильных ответов
            foreach (Question question in CurrentTest.Questions)
            {
                if (question.Answers != null)
                {
                    foreach (Answer answer in question.Answers)
                    {
                        if (question.CorrectAnswers.Contains(answer.AnswerText)) answer.IsSelected = true;
                    }
                }
            }

            #region Команды

            #region Обычные команды
            AddTextQuestion = new LambdaCommand(OnAddTextQuestionExecuted, CanAddTextQuestionExecuted);
            AddImageQuestion = new LambdaCommand(OnAddImageQuestionExecuted, CanAddImageQuestionExecuted);
            AddMultiChoiseQuestion = new LambdaCommand(OnAddMultiChoiseQuestionExecuted, CanAddMultiChoiseQuestionExecuted);
            AddAnswerCommand = new LambdaCommand(OnAddAnswerExecuted, CanAddAnswerExecuted);
            UploadAnswerImage = new LambdaCommand(OnUploadAnswerImageExecuted, CanUploadAnswerImageExecuted);
            UploadQuestionImage = new LambdaCommand(OnUploadQuestionImageExecuted, CanUploadQuestionImageExecuted);
            SaveTest = new LambdaCommand(OnSaveTestExecuted, CanSaveTestExecuted);
            RemoveAnswers = new LambdaCommand(OnRemoveAnswerExecuted, CanRemoveAnswerExecuted);
            RemoveQuestion = new LambdaCommand(OnRemoveQuestionExecuted, CanRemoveQuestionExecuted);

            #endregion

            #region Команды навигации

            NavigateRedactorMenuCommand = new NavigateCommand<TestRedactorMenuViewModel>(navigationStore, () => new TestRedactorMenuViewModel(navigationStore));

            #endregion

            #endregion
        }
    }
}
