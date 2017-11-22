using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using XamlControls = Windows.UI.Xaml.Controls;

namespace MvvmCross.Forms.Uwp
{
    public abstract class MvxTypedFormsWindowsSetup<TForms, TApplication> : MvxFormsWindowsSetup
        where TForms : MvxFormsApplication, new()
        where TApplication : IMvxApplication, new()
    {
        private List<Assembly> _viewAssemblies;
        protected MvxTypedFormsWindowsSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame, e)
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
