using System.Collections.Generic;
using MvvmCross.Binding;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.iOS.Presenters;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Localization;
using UIKit;

namespace MvvmCross.Forms.iOS
{
    public abstract class MvxFormsIosSetup : MvxIosSetup
    {
        public MvxFormsIosSetup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            Xamarin.Forms.Forms.Init();

            var xamarinFormsApp = new MvxFormsApplication();

            return new MvxFormsIosPagePresenter(Window, xamarinFormsApp);
        }

        protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<System.Reflection.Assembly>(base.ValueConverterAssemblies);
                toReturn.Add(typeof(MvxLanguageConverter).Assembly);
                return toReturn;
            }
        }

        protected override MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxFormsBindingBuilder();
        }
    }
}
