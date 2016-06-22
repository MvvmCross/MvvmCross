// MvxNotifyPropertyChanged.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    using MvvmCross.Platform.Core;

    public abstract class MvxNotifyPropertyChanged
        : MvxMainThreadDispatchingObject
        , IMvxNotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _shouldAlwaysRaiseInpcOnUserInterfaceThread;

        public bool ShouldAlwaysRaiseInpcOnUserInterfaceThread()
        {
            return this._shouldAlwaysRaiseInpcOnUserInterfaceThread;
        }

        public void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value)
        {
            this._shouldAlwaysRaiseInpcOnUserInterfaceThread = value;
        }

        protected MvxNotifyPropertyChanged()
        {
            var alwaysOnUIThread = (MvxSingletonCache.Instance == null) || MvxSingletonCache.Instance.Settings.AlwaysRaiseInpcOnUserInterfaceThread;
            this.ShouldAlwaysRaiseInpcOnUserInterfaceThread(alwaysOnUIThread);
        }

        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            this.RaisePropertyChanged(name);
        }

        public void RaisePropertyChanged([CallerMemberName] string whichProperty = "")
        {
            var changedArgs = new PropertyChangedEventArgs(whichProperty);
            this.RaisePropertyChanged(changedArgs);
        }

        public virtual void RaiseAllPropertiesChanged()
        {
            var changedArgs = new PropertyChangedEventArgs(string.Empty);
            this.RaisePropertyChanged(changedArgs);
        }

        public virtual void RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            // check for interception before broadcasting change
            if (this.InterceptRaisePropertyChanged(changedArgs)
                == MvxInpcInterceptionResult.DoNotRaisePropertyChanged)
                return;

            var raiseAction = new Action(() =>
                    {
                        PropertyChanged?.Invoke(this, changedArgs);
                    });

            if (this.ShouldAlwaysRaiseInpcOnUserInterfaceThread())
            {
                // check for subscription before potentially causing a cross-threaded call
                if (this.PropertyChanged == null)
                    return;

                this.InvokeOnMainThread(raiseAction);
            }
            else
            {
                raiseAction();
            }
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        protected virtual MvxInpcInterceptionResult InterceptRaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            if (MvxSingletonCache.Instance != null)
            {
                var interceptor = MvxSingletonCache.Instance.InpcInterceptor;
                if (interceptor != null)
                {
                    return interceptor.Intercept(this, changedArgs);
                }
            }

            return MvxInpcInterceptionResult.NotIntercepted;
        }
    }
}