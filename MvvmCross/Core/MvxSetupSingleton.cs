// Licensed to the .NET Foundation under one or more agreements.
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
        private IMvxSetupMonitor _currentMonitor;

        protected virtual IMvxSetup Setup => _setup;

        public virtual TMvxSetup PlatformSetup<TMvxSetup>()
            where TMvxSetup : IMvxSetup
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
                if (IsInitialisedTaskCompletionSource == null)
                {
                    StartSetupInitialization();
                }
                else
                {
                    if (IsInitialisedTaskCompletionSource.Task.IsCompleted)
                        return;

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
                
                // if the tcs is not null, it means the initialization is running
                if (IsInitialisedTaskCompletionSource != null)
                {
                    // If the task is already completed at this point, let the monitor know it has finished. 
                    // but don't do it otherwise because it's done elsewhere
                    if(IsInitialisedTaskCompletionSource.Task.IsCompleted)
                    {
                        _currentMonitor?.InitializationComplete();
                    }

                    return;
                }
                
                StartSetupInitialization();
            }
        }

        public virtual void CancelMonitor(IMvxSetupMonitor setupMonitor)
        {
            lock (LockObject)
            {
                if (setupMonitor != _currentMonitor)
                {
                    throw new MvxException("The specified IMvxSetupMonitor is not the one registered in MvxSetupSingleton");
                }
                _currentMonitor = null;
            }
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

        private void StartSetupInitialization()
        {
            IsInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
            _setup.InitializePrimary();
            Task.Run(() =>
            {
                _setup.InitializeSecondary();
                lock (LockObject)
                {
                    IsInitialisedTaskCompletionSource.SetResult(true);
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
        }
    }
}
