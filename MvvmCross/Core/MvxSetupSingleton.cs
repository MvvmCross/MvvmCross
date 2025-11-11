// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

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
    /// to guarantee that setup has been created and initialized. This method
    /// is blocking so make sure it's only called at a point where there
    /// are no other UI methods are being invoked. This method is typically called
    /// in applications where there is no splash screen.
    /// </summary>
    public abstract class MvxSetupSingleton
       : MvxSingleton<MvxSetupSingleton>
    {
        private static readonly object LockObject = new();
        private IMvxSetup _setup;

        protected virtual IMvxSetup Setup => _setup;

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
                MvxLogHost.Default?.Log(LogLevel.Error, ex, "Unable to cast setup to {SetupType}", typeof(TMvxSetup));
                throw;
            }
        }

        /// <summary>
        /// Returns a singleton object that is used to manage the creation and
        /// execution of setup
        /// </summary>
        /// <typeparam name="TMvxSetupSingleton">The platform specific setup singleton type</typeparam>
        /// <returns>A platform specific setup singleton</returns>
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected static TMvxSetupSingleton EnsureSingletonAvailable<TMvxSetupSingleton>()
           where TMvxSetupSingleton : MvxSetupSingleton, new()
        {
            // Double null - check before creating the setup singleton object
            if (Instance != null)
                return Instance as TMvxSetupSingleton;
            lock (LockObject)
            {
                if (Instance != null)
                    return Instance as TMvxSetupSingleton;

                // Go ahead and create the setup singleton, and then
                // create the setup instance. 
                // Note that the Instance property is set within the 
                // singleton constructor
                var instance = new TMvxSetupSingleton();
                instance.CreateSetup();
                return Instance as TMvxSetupSingleton;
            }
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        public virtual void EnsureInitialized()
        {
            lock (LockObject)
            {
                _setup.InitializePrimary();
                _setup.InitializeSecondary();
            }
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
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
    }
}
