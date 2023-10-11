
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace TestingApp.Models.TestModels
{
    internal class Question : INotifyPropertyChanged
    {

        private string? questionImage;
        public BitmapImage BitmapImage { 
            get
            {
                if (questionImage == null)
                {
                    return null;
                }
                Uri uri = new Uri(questionImage as string, UriKind.Relative);
                BitmapImage image = new BitmapImage(uri);
                return image;
            }
            }

        public int QuestionNumber { get; set; }
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
