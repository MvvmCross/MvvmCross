// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Wpf.Views
{
    public interface IMvxWpfView
        : IMvxView
    {
    }

    public interface IMvxWpfView<TViewModel>
        : IMvxWpfView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}