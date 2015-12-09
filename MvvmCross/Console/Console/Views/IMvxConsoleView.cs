// IMvxConsoleView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Console.Views
{
    public interface IMvxConsoleView : IMvxView
    {
        void HackSetViewModel(object viewModel);

        bool HandleInput(string input);
    }

    public interface IMvxConsoleView<TViewModel>
        : IMvxConsoleView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}