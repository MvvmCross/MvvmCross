// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platform.Console.Views
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
