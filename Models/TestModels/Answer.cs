using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestingApp.Models.TestModels
{
    /// <summary>
    /// Модель вариантов ответа в вопросе
    /// </summary>
    internal class Answer : INotifyPropertyChanged
    {
        private string? answerText;

        public string? AnswerText 
        {
            get
            {
                return answerText;
            }
            set
            {
                answerText = value;
                OnPropertyChanged("AnswerText");
            }
        }
        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
