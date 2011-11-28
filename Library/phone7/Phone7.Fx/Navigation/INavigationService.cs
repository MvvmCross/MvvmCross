using System.Collections.Generic;
using System.Windows.Navigation;

namespace Phone7.Fx.Navigation
{
    public interface INavigationService
    {
        void Navigate(string pageName);
        JournalEntry RemoveBackEntry();
        void GoBack();
        void GoForward();
        bool CanGoBack { get; }
        bool CanGoForward { get; }
        IEnumerable<JournalEntry> BackStack { get; }
    }
}