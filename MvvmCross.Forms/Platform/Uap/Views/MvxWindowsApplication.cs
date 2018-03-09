// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Forms.Views.Base;
using MvvmCross.Platform.Uap.Views;
using Windows.ApplicationModel.Activation;

namespace MvvmCross.Forms.Platform.Uap.Views
{
    public abstract class MvxWindowsApplication : MvxApplication
    {
        protected abstract Type HostWindowsPageType();

        protected override void Start(IActivatedEventArgs activationArgs)
        {
            if (RootFrame?.Content == null) {
                var hostType = HostWindowsPageType();

                RootFrame.Navigate(hostType, (activationArgs as LaunchActivatedEventArgs)?.Arguments);
            }
        }
    }
}
