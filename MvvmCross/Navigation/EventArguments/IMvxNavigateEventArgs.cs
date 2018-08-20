// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;

namespace MvvmCross.Navigation.EventArguments
{
    public interface IMvxNavigateEventArgs
    {
        bool Cancel { get; set; }
        NavigationMode Mode { get; set; }
        IMvxViewModel ViewModel { get; set; }
    }
}
