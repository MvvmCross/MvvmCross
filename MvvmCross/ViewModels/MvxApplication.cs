// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace MvvmCross.ViewModels
{
#nullable enable
    public abstract class MvxApplication : IMvxApplication
    {
        private IMvxViewModelLocator? _defaultLocator;

        private IMvxViewModelLocator DefaultLocator
        {
            get
            {
                _defaultLocator ??= CreateDefaultViewModelLocator();
                return _defaultLocator;
            }
        }

        protected virtual IMvxViewModelLocator CreateDefaultViewModelLocator()
        {
            return new MvxDefaultViewModelLocator();
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
            // do nothing
        }

        /// <summary>
        /// Any initialization steps that can be done in the background
        /// </summary>
        public virtual void Initialize()
        {
            // do nothing
        }

        /// <summary>
        /// Any initialization steps that need to be done on the UI thread
        /// </summary>
        public virtual Task Startup()
        {
            MvxLogHost.Default?.Log(LogLevel.Trace, "AppStart: Application Startup - On UI thread");
            return Task.CompletedTask;
        }

        /// <summary>
        /// If the application is restarted (eg primary activity on Android 
        /// can be restarted) this method will be called before Startup
        /// is called again
        /// </summary>
        public virtual void Reset()
        {
            // do nothing
        }

        public IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            return DefaultLocator;
        }

        protected void RegisterCustomAppStart<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMvxAppStart>()
                where TMvxAppStart : class, IMvxAppStart
        {
            Mvx.IoCProvider?.ConstructAndRegisterSingleton<IMvxAppStart, TMvxAppStart>();
        }

        protected void RegisterAppStart<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>()
            where TViewModel : IMvxViewModel
        {
            Mvx.IoCProvider?.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel>>();
        }

        protected void RegisterAppStart(IMvxAppStart appStart)
        {
            Mvx.IoCProvider?.RegisterSingleton(appStart);
        }

        protected virtual void RegisterAppStart<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>()
          where TViewModel : IMvxViewModel<TParameter> where TParameter : class
        {
            Mvx.IoCProvider?.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel, TParameter>>();
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }
    }

    public class MvxApplication<TParameter> : MvxApplication, IMvxApplication<TParameter>
    {
        public virtual Task<TParameter> Startup(TParameter hint)
        {
            return Task.FromResult(hint);
        }
    }
#nullable restore
}
