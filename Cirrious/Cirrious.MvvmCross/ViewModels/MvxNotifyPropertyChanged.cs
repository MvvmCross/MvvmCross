// MvxNotifyPropertyChanged.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxNotifyPropertyChanged
        : MvxMainThreadDispatchingObject
          , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            RaisePropertyChanged(name);
        }

        protected void RaisePropertyChanged(string whichProperty)
        {
            var changedArgs = new PropertyChangedEventArgs(whichProperty);
            RaisePropertyChanged(changedArgs);
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            // check for subscription before going multithreaded
            if (PropertyChanged == null)
                return;

            InvokeOnMainThread(
                () =>
                    {
                        var handler = PropertyChanged;

                        if (handler != null)
                            handler(this, changedArgs);
                    });
        }
    }
}