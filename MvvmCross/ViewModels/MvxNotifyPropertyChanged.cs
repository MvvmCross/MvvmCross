// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MvvmCross.Annotations;
using MvvmCross.Base;
using MvvmCross.Logging;

namespace MvvmCross.ViewModels
{
    public abstract class MvxNotifyPropertyChanged
        : MvxMainThreadDispatchingObject
        , IMvxNotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs AllPropertiesChanged = new PropertyChangedEventArgs(string.Empty);
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        private bool _shouldAlwaysRaiseInpcOnUserInterfaceThread;
        private bool _shouldRaisePropertyChanging;
        private bool _shouldLogInpc;

        public bool ShouldAlwaysRaiseInpcOnUserInterfaceThread()
        {
            return _shouldAlwaysRaiseInpcOnUserInterfaceThread;
        }

        public void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value)
        {
            _shouldAlwaysRaiseInpcOnUserInterfaceThread = value;
        }

        public bool ShouldRaisePropertyChanging()
        {
            return _shouldRaisePropertyChanging;
        }

        public void ShouldRaisePropertyChanging(bool value)
        {
            _shouldRaisePropertyChanging = value;
        }
        public bool ShouldLogInpc()
        {
            return _shouldLogInpc;
        }

        public void ShouldLogInpc(bool value)
        {
            _shouldLogInpc = value;
        }

        protected MvxNotifyPropertyChanged()
        {
            var alwaysOnUIThread = MvxSingletonCache.Instance == null || MvxSingletonCache.Instance.Settings.AlwaysRaiseInpcOnUserInterfaceThread;
            ShouldAlwaysRaiseInpcOnUserInterfaceThread(alwaysOnUIThread);
            var raisePropertyChanging = MvxSingletonCache.Instance == null || MvxSingletonCache.Instance.Settings.ShouldRaisePropertyChanging;
            ShouldRaisePropertyChanging(raisePropertyChanging);
            var shouldLogInpc = MvxSingletonCache.Instance == null || MvxSingletonCache.Instance.Settings.ShouldLogInpc;
            ShouldLogInpc(shouldLogInpc);
        }

        public bool RaisePropertyChanging<T>(T newValue, Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            return RaisePropertyChanging(newValue, name);
        }

        public bool RaisePropertyChanging<T>(T newValue, [CallerMemberName] string whichProperty = "")
        {
            var changedArgs = new MvxPropertyChangingEventArgs<T>(whichProperty, newValue);
            return RaisePropertyChanging(changedArgs);
        }

        public virtual bool RaisePropertyChanging<T>(MvxPropertyChangingEventArgs<T> changingArgs)
        {
            // check for interception before broadcasting change
            if (InterceptRaisePropertyChanging(changingArgs)
                == MvxInpcInterceptionResult.DoNotRaisePropertyChanging)
                return !changingArgs.Cancel;

            if (ShouldLogInpc())
                MvxLog.Instance.Trace($"Property '{changingArgs.PropertyName}' changing value to {changingArgs.NewValue.ToString()}");

            PropertyChanging?.Invoke(this, changingArgs);

            return !changingArgs.Cancel;
        }

        [NotifyPropertyChangedInvocator]
        public Task RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            return RaisePropertyChanged(name);
        }

        [NotifyPropertyChangedInvocator]
        public virtual Task RaisePropertyChanged([CallerMemberName] string whichProperty = "")
        {
            var changedArgs = new PropertyChangedEventArgs(whichProperty);
            return RaisePropertyChanged(changedArgs);
        }

        public virtual Task RaiseAllPropertiesChanged()
        {
            return RaisePropertyChanged(AllPropertiesChanged);
        }

        public virtual async Task RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            // check for interception before broadcasting change
            if (InterceptRaisePropertyChanged(changedArgs)
                == MvxInpcInterceptionResult.DoNotRaisePropertyChanged)
                return;

            void raiseChange()
            {
                if (ShouldLogInpc())
                    MvxLog.Instance.Trace($"Property '{changedArgs.PropertyName}' value changed");
                PropertyChanged?.Invoke(this, changedArgs);
            }

            if (ShouldAlwaysRaiseInpcOnUserInterfaceThread())
            {
                // check for subscription before potentially causing a cross-threaded call
                if (PropertyChanged == null)
                    return;

                await InvokeOnMainThreadAsync(raiseChange);
            }
            else
            {
                raiseChange();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            if (ShouldRaisePropertyChanging())
            {
                var shouldSetValue = RaisePropertyChanging(value, propertyName);
                if (!shouldSetValue)
                    return false;
            }

            storage = value;
            RaisePropertyChanged(propertyName);
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

        protected virtual MvxInpcInterceptionResult InterceptRaisePropertyChanging(PropertyChangingEventArgs changingArgs)
        {
            if (MvxSingletonCache.Instance != null)
            {
                var interceptor = MvxSingletonCache.Instance.InpcInterceptor;
                if (interceptor != null)
                {
                    return interceptor.Intercept(this, changingArgs);
                }
            }

            return MvxInpcInterceptionResult.NotIntercepted;
        }
    }
}
