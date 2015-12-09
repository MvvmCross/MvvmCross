// MvxAutoViewSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid
{
    using System;

    using CrossUI.Core.Builder;
    using CrossUI.Droid;

    using MvvmCross.AutoView.Builders;
    using MvvmCross.AutoView.Droid.Builders;
    using MvvmCross.AutoView.Droid.Views;
    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.Platform;

#warning Factor out a base class shared across platfroms (can use Generics)

    public class MvxAutoViewSetup
    {
        public void Initialize(Type resourceType)
        {
            this.RegisterAutomaticViewTextLoader();
            this.RegisterViewFinders();
            this.InitializeDialogBinding(resourceType);
            this.InitializeUserInterfaceBuilder();
        }

        protected virtual void InitializeUserInterfaceBuilder()
        {
            var droidRegistry = this.CreateBuilderRegistry();
            Mvx.RegisterSingleton<IBuilderRegistry>(droidRegistry);
            var userInterfaceFactory = this.CreateUserInterfaceFactory();
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
            this.RegisterSecondaryViewFinders(container);
            this.RegisterLastResortViewFinder(container);
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
            var finder = this.CreateDefaultDialogViewFinder();
            container.AddSecondary(finder);
            var finder2 = this.CreateDefaultListViewFinder();
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
            var loader = this.CreateAutoViewTextLoader();
            Mvx.RegisterSingleton(loader);
        }

        protected virtual IMvxAutoViewTextLoader CreateAutoViewTextLoader()
        {
            return new MvxAutoViewTextLoader();
        }
    }
}