// MvxApplication.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Platform;
    using MvvmCross.Platform.IoC;
    using MvvmCross.Platform.Plugins;

    public abstract class MvxApplication
        : IMvxApplication
    {
        private IMvxViewModelLocator _defaultLocator;

        private IMvxViewModelLocator DefaultLocator
        {
            get
            {
                this._defaultLocator = this._defaultLocator ?? this.CreateDefaultViewModelLocator();
                return this._defaultLocator;
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
            return this.DefaultLocator;
        }

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
            return this.CreatableTypes(this.GetType().GetTypeInfo().Assembly);
        }

        protected IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }
    }
}