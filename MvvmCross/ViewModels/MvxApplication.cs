// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.IoC;
using MvvmCross.Plugin;

namespace MvvmCross.ViewModels
{
    public abstract class MvxApplication : IMvxApplication
    {
        private IMvxViewModelLocator _defaultLocator;

        private IMvxViewModelLocator DefaultLocator
        {
            get
            {
                _defaultLocator = _defaultLocator ?? CreateDefaultViewModelLocator();
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
        /// <param name="hint"></param>
        public virtual void Startup(object hint)
        {
            // do nothing
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

        protected void RegisterCustomAppStart<TMvxAppStart>()
            where TMvxAppStart : class, IMvxAppStart
        {
            Mvx.ConstructAndRegisterSingleton<IMvxAppStart, TMvxAppStart>();
        }

        protected void RegisterAppStart<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            Mvx.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel>>();
        }

        protected void RegisterAppStart(IMvxAppStart appStart)
        {
            Mvx.RegisterSingleton(appStart);
        }

        protected IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        protected IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }
    }

    public abstract class MvxApplication<TStartParameter> : MvxApplication, IMvxApplication<TStartParameter>
    {
        protected void RegisterAppStart<TViewModel, TParameter>()
            where TViewModel : IMvxViewModel<TParameter>
        {
            Mvx.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel, TParameter>>();
        }

        /// <summary>
        /// Return a custom app start hint object from the subclass
        /// </summary>
        public abstract TStartParameter StartParameter { get; }
    }
}
