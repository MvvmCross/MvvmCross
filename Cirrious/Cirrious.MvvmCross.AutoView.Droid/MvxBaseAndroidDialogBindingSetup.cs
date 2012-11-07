using System;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.Droid.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using FooBar.Dialog.Droid;

namespace Cirrious.MvvmCross.AutoView.Droid
{
    public class MvxAutoViewSetup
        : IMvxServiceProducer
        , IMvxServiceConsumer
    {
        public void Initialize(Type resourceType)
        {
            RegisterAutomaticViewTextLoader();
            RegisterDefaultViewFinders();
            InitializeDialogBinding(resourceType);
            InitializeUserInterfaceBuilder();
        }

        private void InitializeUserInterfaceBuilder()
        {
            // TODO...
        }

        protected virtual void InitializeDialogBinding(Type resourceLayoutType)
        {
            DroidResources.Initialise(resourceLayoutType /*typeof(Resource.Layout)*/);
        }

        protected virtual void RegisterDefaultViewFinders()
        {
            var container = this.GetService<IMvxViewsContainer>();
            AddSecondaryViewFinders(container);
        }

        private void RegisterAutomaticViewTextLoader()
        {
            var loader = CreateDefaultViewTextLoader();
            this.RegisterServiceInstance<IMvxDefaultViewTextLoader>(loader);
        }

        protected virtual void AddSecondaryViewFinders(IMvxViewsContainer container)
        {
            var finder = CreateDefaultDialogViewFinder();
            container.AddSecondaryViewFinder(finder);
            var finder2 = CreateDefaultListViewFinder();
            container.AddSecondaryViewFinder(finder2);
        }

        protected virtual IMvxDefaultViewTextLoader CreateDefaultViewTextLoader()
        {
            return new MvxDefaultViewTextLoader();
        }

        protected virtual MvxDefaultDialogViewFinder CreateDefaultDialogViewFinder()
        {
            var finder = new MvxDefaultDialogViewFinder();
            return finder;
        }

        protected virtual MvxDefaultListViewFinder CreateDefaultListViewFinder()
        {
            var finder = new MvxDefaultListViewFinder();
            return finder;
        }
    }
}