﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using MvvmCross.Annotations;
using MvvmCross.Base;

namespace MvvmCross.ViewModels
{
    public abstract class MvxNotifyPropertyChanged
        : MvxMainThreadDispatchingObject
        , IMvxNotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs AllPropertiesChanged = new PropertyChangedEventArgs(string.Empty);
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
            var alwaysOnUIThread = MvxSingletonCache.Instance == null || MvxSingletonCache.Instance.Settings.AlwaysRaiseInpcOnUserInterfaceThread;
            ShouldAlwaysRaiseInpcOnUserInterfaceThread(alwaysOnUIThread);
        }

        [NotifyPropertyChangedInvocator]
        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            RaisePropertyChanged(name);
        }

        [NotifyPropertyChangedInvocator]
        public void RaisePropertyChanged([CallerMemberName] string whichProperty = "")
        {
            var changedArgs = new PropertyChangedEventArgs(whichProperty);
            RaisePropertyChanged(changedArgs);
        }

        public virtual void RaiseAllPropertiesChanged()
        {
            RaisePropertyChanged(AllPropertiesChanged);
        }

        public virtual void RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            // check for interception before broadcasting change
            if (InterceptRaisePropertyChanged(changedArgs)
                == MvxInpcInterceptionResult.DoNotRaisePropertyChanged)
                return;

            if (ShouldAlwaysRaiseInpcOnUserInterfaceThread())
            {
                // check for subscription before potentially causing a cross-threaded call
                if (PropertyChanged == null)
                    return;

                InvokeOnMainThread(() => PropertyChanged?.Invoke(this, changedArgs));
            }
            else
            {
                PropertyChanged?.Invoke(this, changedArgs);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
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
    }
}