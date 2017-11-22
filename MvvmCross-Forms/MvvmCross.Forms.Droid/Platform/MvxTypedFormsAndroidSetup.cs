using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvvmCross.Forms.Droid.Platform
{
    public abstract class MvxTypedFormsAndroidSetup<TForms, TApplication> : MvxFormsAndroidSetup
where TForms : MvxFormsApplication, new()
where TApplication : IMvxApplication, new()
    {
        private List<Assembly> _viewAssemblies;
        protected MvxTypedFormsAndroidSetup(Context applicationContext) : base(applicationContext)
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