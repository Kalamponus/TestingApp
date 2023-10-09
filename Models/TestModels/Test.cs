using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.Models.TestModels
{
    internal class Test : INotifyCollectionChanged
    {

        public string? Name { get; set; }
        public ObservableCollection<Question>? Questions { get; set; }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
    }
}
