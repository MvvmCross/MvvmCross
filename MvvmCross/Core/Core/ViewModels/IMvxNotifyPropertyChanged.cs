// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxNotifyPropertyChanged : INotifyPropertyChanged
    {
        // this ShouldAlwaysRaiseInpcOnUserInterfaceThread is not a Property so as to avoid Inpc pollution
        bool ShouldAlwaysRaiseInpcOnUserInterfaceThread();

        void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value);

        void RaisePropertyChanged<T>(Expression<Func<T>> property);

        void RaisePropertyChanged(string whichProperty);

        void RaisePropertyChanged(PropertyChangedEventArgs changedArgs);
    }
}