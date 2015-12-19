// IMvxConsoleNavigation.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Console.Views
{
    using MvvmCross.Core.Views;

    public interface IMvxConsoleNavigation
        : IMvxViewPresenter
    {
        void GoBack();

        void RemoveBackEntry();

        bool CanGoBack();
    }
}