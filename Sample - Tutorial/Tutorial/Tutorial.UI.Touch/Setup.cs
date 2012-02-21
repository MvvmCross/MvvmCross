using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Services;
using Cirrious.MvvmCross.Binding.Binders;
using Tutorial.Core;
using Tutorial.Core.Converters;
using Tutorial.Core.ViewModels;
using Tutorial.Core.ViewModels.Lessons;
using Tutorial.UI.Touch.Views;
using Tutorial.UI.Touch.Views.Lessons;

namespace Tutorial.UI.Touch
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
            var app = new App();
            return app;
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return new Dictionary<Type, Type>()
                       {
                            { typeof(MainMenuViewModel), typeof(MainMenuView)},
                            { typeof(SimpleTextPropertyViewModel), typeof(SimpleTextPropertyView)},
                       };
        }
		
		protected override void FillValueConverters(Cirrious.MvvmCross.Binding.Interfaces.Binders.IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            var filler = new MvxInstanceBasedValueConverterRegistryFiller(registry);
            filler.AddFieldConverters(typeof(Converters));
        }

        #endregion
    }
}
