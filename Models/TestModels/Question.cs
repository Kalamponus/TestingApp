
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace TestingApp.Models.TestModels
{
    /// <summary>
    /// Модель вопросов в тесте
    /// </summary>
    internal class Question : INotifyPropertyChanged
    {

        private string? questionImage;
        private int questionNumber;

        public string QuestionNumberCheck
        {
            get
            {
                return questionNumber.ToString();
            }
            set
            {
                int number;
                if (Int32.TryParse(value.Trim(), out number))
                {
                    questionNumber = number;
                }
            }
        }

        public int QuestionNumber 
        {
            get
            {
                return questionNumber;
            }
            set
            {
                questionNumber = value;
            }
        }
        public string? QuestionText { get; set; }
        public string? QuestionImage 
        {
            get
            {
                return questionImage;
            }
            set
            {
                questionImage = value;
                OnPropertyChanged("QuestionImage");
            }
        }
        public QuestionType QuestionType { get; set; }
        public List<string>? CorrectAnswers { get; set; }
        public ObservableCollection<Answer>? Answers { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
