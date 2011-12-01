#region Copyright

// <copyright file="MvxBaseSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Conventions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.IoC;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Platform
{
    public abstract class MvxBaseSetup
        : IMvxServiceProducer<IMvxViewsContainer>
          , IMvxServiceProducer<IMvxViewDispatcherProvider>
          , IMvxServiceProducer<IMvxApplicationTitle>
          , IMvxServiceProducer<IMvxViewModelLocatorFinder>
          , IMvxServiceProducer<IMvxViewModelLocatorStore>
          , IMvxServiceConsumer<IMvxViewsContainer>

    {
        public virtual void Initialize()
        {
            InitializeIoC();
            InitializeConventions();
            InitializeApp();
            InitializeViewsContainer();
            InitializeViews();
        }

        protected virtual void InitializeIoC()
        {
            // initialise the IoC service registry
            // this also pulls in all platform services too
            MvxOpenNetCfServiceProviderSetup.Initialize();
        }

        protected virtual void InitializeConventions()
        {
            // initialize default conventions
            MvxDefaultConventionSetup.Initialize();
        }

        protected virtual void InitializeApp()
        {
            var app = CreateApp();
            this.RegisterServiceInstance<IMvxApplicationTitle>(app);
            this.RegisterServiceInstance<IMvxViewModelLocatorFinder>(app);
            this.RegisterServiceInstance<IMvxViewModelLocatorStore>(app);
        }

        protected abstract MvxApplication CreateApp();

        protected virtual void InitializeViewsContainer()
        {
            var container = CreateViewsContainer();
            this.RegisterServiceInstance<IMvxViewsContainer>(container);
            this.RegisterServiceInstance<IMvxViewDispatcherProvider>(container);
        }

        protected abstract MvxViewsContainer CreateViewsContainer();

        protected virtual void InitializeViews()
        {
            var container = this.GetService<IMvxViewsContainer>();

            foreach (var pair in GetViewModelViewLookup())
            {
                Add(container, pair.Key, pair.Value);
            }
        }

        protected abstract IDictionary<Type, Type> GetViewModelViewLookup();

        protected void Add<TViewModel, TView>(IMvxViewsContainer container)
            where TViewModel : IMvxViewModel
        {
            container.Add(typeof (TViewModel), typeof (TView));
        }

        protected void Add(IMvxViewsContainer container, Type viewModelType, Type viewType)
        {
            container.Add(viewModelType, viewType);
        }
    }
}