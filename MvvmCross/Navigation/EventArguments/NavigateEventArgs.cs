// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Threading;
using MvvmCross.ViewModels;

namespace MvvmCross.Navigation.EventArguments
{
    public enum NavigationMode
    {
        None,
        Show,
        Close
    }

    public class MvxNavigateEventArgs : MvxCancelEventArgs, IMvxNavigateEventArgs
    {
        public MvxNavigateEventArgs(NavigationMode mode, CancellationToken cancellationToken = default) : base(cancellationToken)
        {
            Mode = mode;
        }

        public MvxNavigateEventArgs(IMvxViewModel viewModel, NavigationMode mode, CancellationToken cancellationToken = default) : this(mode, cancellationToken)
        {
            ViewModel = viewModel;
        }

        public NavigationMode Mode { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}
