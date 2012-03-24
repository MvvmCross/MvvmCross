using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Cirrious.Conference.Core.Interfaces
{
    public interface IObservableCollection<TKey>
        : ICollection<TKey>
          , INotifyPropertyChanged
          , INotifyCollectionChanged
    {        
    }
}