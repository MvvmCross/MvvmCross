using Cirrious.Conference.Core;
using Cirrious.Conference.Core.Converters;
using Cirrious.Conference.UI.Touch.Bindings;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using MonoTouch.UIKit;

namespace Cirrious.Conference.UI.Touch
{
    public class Setup
         : MvxTouchDialogBindingSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        #region Overrides of MvxBaseSetup

        protected override MvxApplication CreateApp()
        {
            var app = new NoSplashScreenConferenceApp();
            return app;
        }

        protected override void InitializeLastChance()
        {
            // create an error displayer - it will sort its own event subscriptions out
            var errorDisplayer = new ErrorDisplayer();

            base.InitializeLastChance();
        }

        protected override void FillValueConverters(Cirrious.MvvmCross.Binding.Interfaces.Binders.IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            var filler = new MvxInstanceBasedValueConverterRegistryFiller(registry);
            filler.AddFieldConverters(typeof(Converters));
        }

        protected override void FillTargetFactories(MvvmCross.Binding.Interfaces.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxCustomBindingFactory<UIButton>("IsFavorite", (button) => new FavoritesButtonBinding(button)));
            registry.RegisterFactory(new MvxCustomBindingFactory<SessionCell2>("IsFavorite", (cell) => new FavoritesSessionCellBinding(cell)));
        }

        #endregion
    }

}

