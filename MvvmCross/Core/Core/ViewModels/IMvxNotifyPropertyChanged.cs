// IMvxNotifyPropertyChanged.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

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