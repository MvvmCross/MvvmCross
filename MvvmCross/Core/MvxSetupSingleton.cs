﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Core
{
    public abstract class MvxSetupSingleton
       : MvxSingleton<MvxSetupSingleton>
    {
        private static readonly object LockObject = new object();
        private static TaskCompletionSource<bool> IsInitialisedTaskCompletionSource;
        private IMvxSetup _setup;
        private bool _initialized;
        private IMvxSetupMonitor _currentMonitor;

        protected virtual IMvxSetup Setup
        {
            get
            {
                return _setup;
            }
        }

        public virtual TMvxSetup PlatformSetup<TMvxSetup>() where TMvxSetup : IMvxSetup
        {
            try
            {
                return (TMvxSetup)Setup;
            }
            catch (Exception ex)
            {
                MvxLog.Instance.Error(ex, "Unable to cast setup to {0}", typeof(TMvxSetup));
                throw ex;
            }
        }

        protected static TMvxSetupSingleton EnsureSingletonAvailable<TMvxSetupSingleton>()
           where TMvxSetupSingleton : MvxSetupSingleton, new()
        {
            if (Instance != null)
                return Instance as TMvxSetupSingleton;

            lock (LockObject)
            {
                if (Instance != null)
                    return Instance as TMvxSetupSingleton;

                var instance = new TMvxSetupSingleton();
                instance.CreateSetup();
                return Instance as TMvxSetupSingleton;
            }
        }

        public virtual void EnsureInitialized()
        {
            lock (LockObject)
            {
                if (_initialized)
                    return;

                if (IsInitialisedTaskCompletionSource == null)
                {
                    IsInitialisedTaskCompletionSource = StartSetupInitialization();
                }
                else
                {
                    MvxLog.Instance.Trace("EnsureInitialized has already been called so now waiting for completion");
                }
            }

            IsInitialisedTaskCompletionSource.Task.GetAwaiter().GetResult();
        }

        public virtual void InitializeAndMonitor(IMvxSetupMonitor setupMonitor)
        {
            lock (LockObject)
            {
                _currentMonitor = setupMonitor;
                if (_initialized)
                {
                    _currentMonitor?.InitializationComplete();
                    return;
                }

                if (IsInitialisedTaskCompletionSource != null)
                {
                    return;
                }

                IsInitialisedTaskCompletionSource = StartSetupInitialization();
            }
        }

        public virtual void CancelMonitor(IMvxSetupMonitor setupMonitor)
        {
            lock (LockObject)
            {
                if (setupMonitor == _currentMonitor)
                {
                    _currentMonitor = null;
                }
            }
        }

        protected MvxSetupSingleton()
        {
        }

        protected virtual void CreateSetup()
        {
            try
            {
                _setup = MvxSetup.Instance();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Failed to create setup instance");
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                lock (LockObject)
                {
                    _currentMonitor = null;
                }
            }
            base.Dispose(isDisposing);
        }

        private TaskCompletionSource<bool> StartSetupInitialization()
        {
            var completionSource = new TaskCompletionSource<bool>();
            _setup.InitializePrimary();
            Task.Run(() =>
            {
                _setup.InitializeSecondary();
                lock (LockObject)
                {
                    completionSource.SetResult(true);
                    _initialized = true;
                    var dispatcher = Mvx.GetSingleton<IMvxMainThreadDispatcher>();
                    dispatcher.RequestMainThreadAction(() =>
                    {
                        if (_currentMonitor != null)
                        {
                            _currentMonitor?.InitializationComplete();
                        }
                    });
                }
            });

            return completionSource;
        }
    }
}
