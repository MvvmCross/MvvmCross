using System;
using Cirrious.MvvmCross.AutoView.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.Droid.Views;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using CrossUI.Core.Builder;
using CrossUI.Droid;

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
            InitializeDialogBinding(resourceType);
            InitializeUserInterfaceBuilder();
        }

        protected virtual void InitializeUserInterfaceBuilder()
        {
            var droidRegistry = CreateBuilderRegistry();
            this.RegisterServiceInstance<IBuilderRegistry>(droidRegistry);
        }

        protected virtual MvxDroidBuilderRegistry CreateBuilderRegistry()
        {
            var droidRegistry = new MvxDroidBuilderRegistry(true);
            return droidRegistry;
        }

        protected virtual void InitializeDialogBinding(Type resourceLayoutType)
        {
            DroidResources.Initialise(resourceLayoutType /*typeof(Resource.Layout)*/);
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