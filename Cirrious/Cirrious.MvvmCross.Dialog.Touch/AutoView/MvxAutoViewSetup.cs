using System;
using Cirrious.MvvmCross.AutoView.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.Droid.Views;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Foobar.Dialog.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid
{
    public class MvxAutoViewSetup
        : IMvxServiceProducer
        , IMvxServiceConsumer
    {
        public void Initialize(Type resourceType)
        {
            RegisterAutomaticViewTextLoader();
            RegisterViewFinders();
            InitializeUserInterfaceBuilder();
        }

        protected virtual void InitializeUserInterfaceBuilder()
        {
            var touchRegistry = CreateBuilderRegistry();
            this.RegisterServiceInstance<IBuilderRegistry>(touchRegistry);
        }

        protected virtual MvxTouchBuilderRegistry CreateBuilderRegistry()
        {
            var touchRegistry = new MvxTouchBuilderRegistry(true);
            return touchRegistry;
        }

        protected virtual void RegisterViewFinders()
        {
            var container = this.GetService<IMvxViewsContainer>();
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
            this.RegisterServiceInstance<IMvxAutoViewTextLoader>(loader);
        }

        protected virtual IMvxAutoViewTextLoader CreateAutoViewTextLoader()
        {
            return new MvxAutoViewTextLoader();
        }
    }
}