// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MvvmCross.ViewModels
{
    public interface IMvxNotifyPropertyChanged : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // this ShouldAlwaysRaiseInpcOnUserInterfaceThread is not a Property so as to avoid Inpc pollution
        bool ShouldAlwaysRaiseInpcOnUserInterfaceThread();

        void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value);

        bool ShouldRaisePropertyChanging();

        void ShouldRaisePropertyChanging(bool value);

        bool RaisePropertyChanging<T>(T newValue, Expression<Func<T>> property);

        bool RaisePropertyChanging<T>(T newValue, string whichProperty);

        bool RaisePropertyChanging<T>(MvxPropertyChangingEventArgs<T> changingArgs);

        Task RaisePropertyChanged<T>(Expression<Func<T>> property);

        Task RaisePropertyChanged(string whichProperty);

        Task RaisePropertyChanged(PropertyChangedEventArgs changedArgs);
    }
}
