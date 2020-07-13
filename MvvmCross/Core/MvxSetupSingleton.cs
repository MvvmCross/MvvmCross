// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
//using Windows.ApplicationModel.UserDataTasks;

namespace MvvmCross.Core
{
    /// <summary>
    /// The setup singleton is designed to ensure only a single instance
    /// of MvxSetup is created and invoked. There are three important methods
    /// to the MvxSetupSingleton class:
    /// EnsureSingletonAvailable - this is a static method that will return 
    /// the one and only instance of MvxSetupSingleton. This method is protected
    /// as it's assumed that each platform will provide a platform specific
    /// public overload for this method which will include any platform parameters
    /// required
    /// EnsureInitialized - this is an instance method that should be called 
    /// to guarrantee that setup has been created and initialized. This method 
    /// is blocking so make sure it's only called at a point where there
    /// are no other UI methods are being invoked. This method is typically called
    /// in applications where there is no splash screen.
    /// InitializeAndMonitor - this is an instance method that can be called 
    /// to make sure that the initialization of setup has begun. It registers
    /// an object to be notified when setup initialization has completed. The callback
    /// will be raised on the UI thread. This method is not blocking, and doesn't
    /// guarrantee setup initialize has finished when it returns. This method is 
    /// typically called by the splash screen view of an application, passing
    /// itself in as the object to be notified. On notification the splash screen 
    /// view will trigger navigation to the first view
    /// </summary>
    public abstract class MvxSetupSingleton
       : MvxSingleton<MvxSetupSingleton>
    {
        private static readonly object _lockObject = new object();
        private IMvxSetup? _setup;

        protected virtual IMvxSetup Setup => _setup!;

        /// <summary>
        /// Returns a platform specific instance of Setup
        /// A useful overload to allow for platform specific
        /// setup logic to be invoked.
        /// </summary>
        /// <typeparam name="TMvxSetup">The platform specific setup type</typeparam>
        /// <returns>A platform specific instance of Setup</returns>
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
                throw;
            }
        }

        /// <summary>
        /// Returns a singleton object that is used to manage the creation and
        /// execution of setup
        /// </summary>
        /// <typeparam name="TMvxSetupSingleton">The platform specific setup singleton type</typeparam>
        /// <returns>A platform specific setup singleton</returns>
        protected static TMvxSetupSingleton EnsureSingletonAvailable<TMvxSetupSingleton>()
           where TMvxSetupSingleton : MvxSetupSingleton, new()
        {
            // Double null - check before creating the setup singleton object
            if (Instance != null)
                return (TMvxSetupSingleton)Instance;

            lock (_lockObject)
            {
                if (Instance != null)
                    return (TMvxSetupSingleton)Instance;

                // Go ahead and create the setup singleton, and then
                // create the setup instance. 
                // Note that the Instance property is set within the 
                // singleton constructor
                var instance = new TMvxSetupSingleton();
                instance.CreateSetup();
                return (TMvxSetupSingleton)Instance!;
            }
        }

        public virtual ValueTask EnsureInitialized()
        {
            return StartSetupInitialization();
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

        private async ValueTask StartSetupInitialization()
        {
            if (_setup == null) throw new MvxException("Not is initialize 'setup'");

            await _setup.InitializePrimary()
            .ContinueWith(async (t) =>
                {
                    if (t.IsCompleted)
                    {
                        await _setup.InitializeSecondary().ConfigureAwait(false);
                    }
                    else
                    {
                        throw new MvxException("'InitializePrimary' is not completed successfully.");
                    }
                }, TaskScheduler.Default)
            .Unwrap().ConfigureAwait(false);
        }
    }
}
