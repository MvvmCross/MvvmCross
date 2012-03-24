using System.Collections.ObjectModel;
using Cirrious.Conference.Core.Interfaces;

namespace Cirrious.Conference.Core.Models.Helpers
{
#warning Not Used
    public class OurObservableCollection<TKey> 
        : ObservableCollection<TKey>
          , IObservableCollection<TKey>
    {        
    }
}