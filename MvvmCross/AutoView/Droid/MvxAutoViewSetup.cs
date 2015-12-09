// MvxAutoViewSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.AutoView.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Views;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Views;
using CrossUI.Core.Builder;
using CrossUI.Droid;
using System;

namespace Cirrious.MvvmCross.AutoView.Droid
{
#warning Factor out a base class shared across platfroms (can use Generics)

    public class MvxAutoViewSetup
    {
        public void Initialize(Type resourceType)
        {
            RegisterAutomaticViewTextLoader();
            RegisterViewFinders();
            InitializeDialogBinding(resourceType);
            InitializeUserInterfaceBuilder();
        }

        protected virtual void InitializeUserInterfaceBuilder()
        {
            var droidRegistry = CreateBuilderRegistry();
            Mvx.RegisterSingleton<IBuilderRegistry>(droidRegistry);
            var userInterfaceFactory = CreateUserInterfaceFactory();
            Mvx.RegisterSingleton(userInterfaceFactory);
        }

        protected virtual IMvxUserInterfaceFactory CreateUserInterfaceFactory()
        {
            return new MvxAndroidUserInterfaceFactory();
        }

        protected virtual MvxAndroidBuilderRegistry CreateBuilderRegistry()
        {
            var droidRegistry = new MvxAndroidBuilderRegistry(true);
            return droidRegistry;
        }

        protected virtual void InitializeDialogBinding(Type resourceLayoutType)
        {
            DroidResources.Initialize(resourceLayoutType /*typeof(Resource.Layout)*/);
        }

        protected virtual void RegisterViewFinders()
        {
            var container = Mvx.Resolve<IMvxViewsContainer>();
            RegisterSecondaryViewFinders(container);
            RegisterLastResortViewFinder(container);
        }

        protected virtual void RegisterLastResortViewFinder(IMvxViewsContainer container)
        {
            var missing = CreateLastResortViewFinder();
            container.SetLastResort(missing);
        }

        private static IMvxViewFinder CreateLastResortViewFinder()
        {
            var missing = new MvxMissingViewFinder();
            return missing;
        }

        protected virtual void RegisterSecondaryViewFinders(IMvxViewsContainer container)
        {
            var finder = CreateDefaultDialogViewFinder();
            container.AddSecondary(finder);
            var finder2 = CreateDefaultListViewFinder();
            container.AddSecondary(finder2);
        }

        protected virtual MvxAutoDialogViewFinder CreateDefaultDialogViewFinder()
        {
            var finder = new MvxAutoDialogViewFinder();
            return finder;
        }

        protected virtual MvxAutoListViewFinder CreateDefaultListViewFinder()
        {
            var finder = new MvxAutoListViewFinder();
            return finder;
        }

        protected virtual void RegisterAutomaticViewTextLoader()
        {
            var loader = CreateAutoViewTextLoader();
            Mvx.RegisterSingleton(loader);
        }

        protected virtual IMvxAutoViewTextLoader CreateAutoViewTextLoader()
        {
            return new MvxAutoViewTextLoader();
        }
    }
}