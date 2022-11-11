// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace MvvmCross.ViewModels
{
#nullable enable
    public interface IMvxInpcInterceptor
    {
        MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangedEventArgs args);
        MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangingEventArgs args);
    }
#nullable restore
}
