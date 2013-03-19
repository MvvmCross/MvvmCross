// IMvxConsoleView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Console.Views
{
    public interface IMvxConsoleView : IMvxView
    {
        void HackSetViewModel(object viewModel);
        bool HandleInput(string input);
    }
}