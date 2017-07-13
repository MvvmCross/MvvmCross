using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.iOS.Presenters;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Localization;
using UIKit;
using System.Linq;

namespace MvvmCross.Forms.iOS
{
    public abstract class MvxFormsIosSetup : MvxIosSetup
    {
        protected MvxFormsIosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected MvxFormsIosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, window, presenter)
        {
        }

        private List<Assembly> viewAssemblies;
        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (viewAssemblies == null)
                viewAssemblies = new List<Assembly>(base.GetViewAssemblies());
            
            return viewAssemblies;
        }

        protected override void InitializeApp(Platform.Plugins.IMvxPluginManager pluginManager)
        {
            base.InitializeApp(pluginManager);
            viewAssemblies.AddRange(GetViewModelAssemblies());
        }

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication {
            get
            {
                if(_formsApplication == null)
                    _formsApplication = CreateFormsApplication();
                return _formsApplication;
            }
        }

        protected virtual MvxFormsApplication CreateFormsApplication()
        {
            return new MvxFormsApplication();
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            global::Xamarin.Forms.Forms.Init();
            return new MvxFormsIosPagePresenter(Window, FormsApplication);
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>(base.ValueConverterAssemblies)
                {
                    typeof(MvxLanguageConverter).Assembly
                };
                return toReturn;
            }
        }

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsBindingBuilder();
    }
}
