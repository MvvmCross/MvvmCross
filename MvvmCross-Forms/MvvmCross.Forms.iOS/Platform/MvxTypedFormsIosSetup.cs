using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using System.Collections.Generic;
using System.Reflection;
using UIKit;

namespace MvvmCross.Forms.iOS
{
    public abstract class MvxTypedFormsIosSetup<TForms, TApplication> : MvxFormsIosSetup
    where TForms : MvxFormsApplication, new()
    where TApplication : IMvxApplication, new()
    {
        private List<Assembly> _viewAssemblies;
        protected MvxTypedFormsIosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected MvxTypedFormsIosSetup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (_viewAssemblies == null)
            {
                _viewAssemblies = new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(TForms).GetTypeInfo().Assembly }));
            }

            return _viewAssemblies;
        }

        protected override MvxFormsApplication CreateFormsApplication() => new TForms();

        protected override IMvxApplication CreateApp() => new TApplication();
    }
}
