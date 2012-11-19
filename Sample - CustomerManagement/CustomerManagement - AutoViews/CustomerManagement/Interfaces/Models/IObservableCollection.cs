using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CustomerManagement.Core.Models
{
    public interface IObservableCollection<T>
        : IList<T>
        , INotifyCollectionChanged
    {
    }
}