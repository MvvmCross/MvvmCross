using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cirrious.Conference.Core;
using Cirrious.Conference.Core.Converters;
using Cirrious.Conference.UI.Droid.Bindings;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Android;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;

namespace Cirrious.Conference.UI.Droid
{
    public class Setup
        : MvxBaseAndroidBindingSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override MvxApplication CreateApp()
        {
            return new NoSplashScreenConferenceApp();
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return new[] { typeof(Converters) }; }
        }

        protected override void FillTargetFactories(MvvmCross.Binding.Interfaces.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxCustomBindingFactory<Button>("IsFavorite", (button) => new FavoritesButtonBinding(button)));
        }
    }
}

