// MvxAutoViewSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch
{
    using CrossUI.Core.Builder;

    using MvvmCross.AutoView.Builders;
    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.AutoView.Touch.Builders;
    using MvvmCross.AutoView.Touch.Views;
    using MvvmCross.Platform;

    public class MvxAutoViewSetup
    {
        public void Initialize()
        {
            this.RegisterAutomaticViewTextLoader();
            this.RegisterViewFinders();
            this.InitializeUserInterfaceBuilder();
        }

        protected virtual void InitializeUserInterfaceBuilder()
        {
            var touchRegistry = this.CreateBuilderRegistry();
            Mvx.RegisterSingleton<IBuilderRegistry>(touchRegistry);
            var userInterfaceFactory = this.CreateUserInterfaceFactory();
            Mvx.RegisterSingleton(userInterfaceFactory);
        }

        protected virtual IMvxUserInterfaceFactory CreateUserInterfaceFactory()
        {
            return new MvxTouchUserInterfaceFactory();
        }

        protected virtual MvxTouchBuilderRegistry CreateBuilderRegistry()
        {
            var touchRegistry = new MvxTouchBuilderRegistry(true);
            return touchRegistry;
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