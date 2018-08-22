// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.ViewModels;

namespace MvvmCross.Views
{
    public interface IMvxView
        : IMvxDataConsumer
    {
        IMvxViewModel ViewModel { get; set; }
    }

    public interface IMvxView<TViewModel>
        : IMvxView where TViewModel : class, IMvxViewModel
    {
        new TViewModel ViewModel { get; set; }
    }
}
