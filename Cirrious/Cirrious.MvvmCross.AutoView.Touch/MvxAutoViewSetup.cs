// MvxAutoViewSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Builders;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.AutoView.Touch.Builders;
using Cirrious.MvvmCross.AutoView.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using CrossUI.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Touch
{
    public class MvxAutoViewSetup
        : IMvxServiceProducer
          , IMvxServiceConsumer
    {
        public void Initialize()
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
            this.RegisterServiceInstance(loader);
        }

        protected virtual IMvxAutoViewTextLoader CreateAutoViewTextLoader()
        {
            return new MvxAutoViewTextLoader();
        }
    }
}