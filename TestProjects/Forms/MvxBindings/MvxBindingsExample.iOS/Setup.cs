using System.Collections.Generic;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.iOS.Platform;
using UIKit;
using Xamarin.Forms;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.iOS.Presenters;

namespace MvxBindingsExample.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            Forms.Init();

            var xamarinFormsApp = new MvxFormsApp();

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
