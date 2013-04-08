using Cirrious.Conference.Core;
using Cirrious.Conference.Core.Converters;
using Cirrious.Conference.UI.Touch.Bindings;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Touch;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Localization;

namespace Cirrious.Conference.UI.Touch
{
    public class Setup
         : MvxTouchSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
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

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxCustomBindingFactory<UIButton>("IsFavorite", (button) => new FavoritesButtonBinding(button)));
            registry.RegisterFactory(new MvxCustomBindingFactory<SessionCell2>("IsFavorite", (cell) => new FavoritesSessionCellBinding(cell)));
        }

		protected override System.Collections.Generic.List<System.Reflection.Assembly> ValueConverterAssemblies 
		{
			get 
			{
				var toReturn = base.ValueConverterAssemblies;
				toReturn.Add(typeof(MvxLanguageConverter).Assembly);
				return toReturn;
			}
		}
    }
}

