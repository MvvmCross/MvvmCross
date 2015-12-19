// IMvxConsoleCurrentView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Console.Views
{
    public interface IMvxConsoleCurrentView
    {
        IMvxConsoleView CurrentView { get; set; }
    }
}