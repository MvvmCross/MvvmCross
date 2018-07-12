// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using MvvmCross.ViewModels;

namespace MvvmCross.Navigation.EventArguments
{
    public class NavigateEventArgs : EventArgs
    {
        public NavigateEventArgs()
        {
        }

        public NavigateEventArgs(IMvxViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            ViewModel = viewModel;
            CancellationToken = cancellationToken;
        }

        public IMvxViewModel ViewModel { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
