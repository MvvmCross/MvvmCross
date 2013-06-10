﻿// IMvxNotifyPropertyChanged.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.ViewModels
{
    public interface IMvxNotifyPropertyChanged : INotifyPropertyChanged
    {
        // this ShouldAlwaysRaiseInpcOnUserInterfaceThread not a Property so as to avoid Inpc pollution
        bool ShouldAlwaysRaiseInpcOnUserInterfaceThread();
        void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value);
        void RaisePropertyChanged<T>(Expression<Func<T>> property);
        void RaisePropertyChanged(string whichProperty);
        void RaisePropertyChanged(PropertyChangedEventArgs changedArgs);
    }
}