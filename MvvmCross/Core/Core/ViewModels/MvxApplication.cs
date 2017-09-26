// MvxApplication.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Core.ViewModels
{
    public abstract class MvxApplication
        : IMvxApplication
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

        public virtual void Initialize()
        {
            // do nothing
        }

        public IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            return DefaultLocator;
        }

        protected void RegisterNavigationServiceAppStart<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            Mvx.ConstructAndRegisterSingleton<IMvxAppStart, MvxNavigationServiceAppStart<TViewModel>>();
        }

        protected void RegisterCustomAppStart<TMvxAppStart>()
            where TMvxAppStart : IMvxAppStart
        {
            Mvx.ConstructAndRegisterSingleton<IMvxAppStart, TMvxAppStart>();
        }

        [Obsolete("Please use RegisterNavigationServiceAppStart instead")]
        protected void RegisterAppStart<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<TViewModel>());
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
}