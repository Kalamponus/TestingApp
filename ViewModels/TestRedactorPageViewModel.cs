using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
        public BitmapImage LoadedImage { get; set; }
        #endregion

        TestWriter Writer { get; set; }

        #region Команды

        public ICommand NavigateRedactorMenuCommand { get; }

        public ICommand AddTextQuestion { get; }

        private bool CanAddTextQuestionExecuted(object p) => true;

        private void OnAddTextQuestionExecuted(object p)
        {
            QuestionCount++;
            CurrentTest.Questions.Add(new Question
            {
                QuestionNumber = QuestionCount,
                QuestionType = QuestionType.TextQuestion,
                CorrectAnswers = new List<string>() { "" }
            });
            
        }

        public ICommand AddImageQuestion { get; }

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

        public ICommand AddMultiChoiseQuestion { get; }
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

        public ICommand AddAnswerCommand { get; }
        private bool CanAddAnswerExecuted(object p) => true;

        private void OnAddAnswerExecuted(object p)
        {
            Question question = p as Question;            
            question.Answers.Add(new Answer());
            Debug.WriteLine(question.Answers.Count);
        }

        public ICommand SaveTest { get; }

        private bool CanSaveTestExecuted(object p) => true;

        private void OnSaveTestExecuted(object p)
        {
            if(CheckFields() == true)
            {
                foreach (Question question in CurrentTest.Questions)
                {
                    if (question.QuestionType == QuestionType.TextQuestion)
                    {
                        string[] correctAnswersArr = question.CorrectAnswers[0].Split(',');
                        question.CorrectAnswers.RemoveAt(0);
                        foreach (string answer in correctAnswersArr)
                        {
                            question.CorrectAnswers.Add(answer.ToLower().Trim());
                        }
                    }
                    else if (question.QuestionType == QuestionType.MultiChoiseQuestion)
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
                Writer = new TestWriter(CurrentTest);
                Writer.Save();
                NavigateRedactorMenuCommand.Execute(p);
            }           
        }

        public ICommand UploadQuestionImage { get; }

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

        public ICommand UploadAnswerImage { get; }

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
                File.Copy(op.FileName, "Images\\" + op.SafeFileName);
                answer.AnswerText = "Images\\" + op.SafeFileName;
            }
        }

        #endregion

        #region Методы

        private bool CheckFields()
        {
            if (CurrentTest == null || string.IsNullOrEmpty(CurrentTest.Name) || File.Exists("Tests\\" + CurrentTest.Name)) return false;
            if (CheckAnswers() == false) return false;
            return true;
        }

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
                        if (answer.IsSelected == true) hasCorrectAnswers = true;
                    }
                    if (hasCorrectAnswers == false) return false;
                }
                else
                {                    
                    if (question.CorrectAnswers == null) return false;
                }              
            }
            return true;
        }

        #endregion

        public TestRedactorPageViewModel(NavigationStore navigationStore, Test test)
        {
            CurrentTest = test;

            if (CurrentTest.Questions == null) CurrentTest.Questions = new ObservableCollection<Question>();
            AddTextQuestion = new LambdaCommand(OnAddTextQuestionExecuted, CanAddTextQuestionExecuted);
            AddImageQuestion = new LambdaCommand(OnAddImageQuestionExecuted, CanAddImageQuestionExecuted);
            AddMultiChoiseQuestion = new LambdaCommand(OnAddMultiChoiseQuestionExecuted, CanAddMultiChoiseQuestionExecuted);
            AddAnswerCommand = new LambdaCommand(OnAddAnswerExecuted, CanAddAnswerExecuted);
            UploadAnswerImage = new LambdaCommand(OnUploadAnswerImageExecuted, CanUploadAnswerImageExecuted);
            UploadQuestionImage = new LambdaCommand(OnUploadQuestionImageExecuted, CanUploadQuestionImageExecuted);
            SaveTest = new LambdaCommand(OnSaveTestExecuted, CanSaveTestExecuted);

            NavigateRedactorMenuCommand = new NavigateCommand<TestRedactorMenuViewModel>(navigationStore, () => new TestRedactorMenuViewModel(navigationStore));
        }
    }
}
