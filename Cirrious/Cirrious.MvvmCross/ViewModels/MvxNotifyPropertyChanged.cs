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
        , IMvxNotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _shouldAlwaysRaiseInpcOnUserInterfaceThread;
        public bool ShouldAlwaysRaiseInpcOnUserInterfaceThread()
        {
            return _shouldAlwaysRaiseInpcOnUserInterfaceThread;
        }

        public void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value)
        {
            _shouldAlwaysRaiseInpcOnUserInterfaceThread = value;
        }

        protected MvxNotifyPropertyChanged()
        {
            ShouldAlwaysRaiseInpcOnUserInterfaceThread(MvxSingletonCache.Instance.Settings.AlwaysRaiseInpcOnUserInterfaceThread);
        }

        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            RaisePropertyChanged(name);
        }

        public void RaisePropertyChanged(string whichProperty)
        {
            var changedArgs = new PropertyChangedEventArgs(whichProperty);
            RaisePropertyChanged(changedArgs);
        }

        public virtual void RaiseAllPropertiesChanged()
        {
            var changedArgs = new PropertyChangedEventArgs(string.Empty);
            RaisePropertyChanged(changedArgs);
        }

        public virtual void RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            // check for interception before broadcasting change
            if (InterceptRaisePropertyChanged(changedArgs)
                == MvxInpcInterceptionResult.DoNotRaisePropertyChanged) 
                return;

            var raiseAction = new Action(() =>
                    {
                        var handler = PropertyChanged;

                        if (handler != null)
                            handler(this, changedArgs);
                    });

            if (ShouldAlwaysRaiseInpcOnUserInterfaceThread())
            {
                // check for subscription before potentially causing a cross-threaded call
                if (PropertyChanged == null)
                    return;

                InvokeOnMainThread(raiseAction);
            }
            else
            {
                raiseAction();
            }
        }

        protected virtual MvxInpcInterceptionResult InterceptRaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            var interceptor = MvxSingletonCache.Instance.InpcInterceptor;
            if (interceptor != null)
            {
                return interceptor.Intercept(this, changedArgs);
            }

            return MvxInpcInterceptionResult.NotIntercepted;
        }
    }
}