// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platforms.Wpf.Bindings;
using MvvmCross.Forms.Platforms.Wpf.Presenters;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Wpf.Core
{
    public abstract class MvxFormsWpfSetup : MvxWpfSetup, IMvxFormsSetup
    {
        private Application _formsApplication;
        private IMvxFormsSetupHelper _formsSetupHelper;

        public virtual IMvxFormsSetupHelper FormsSetupHelper
        {
            get
            {
                return _formsSetupHelper ?? (_formsSetupHelper = Mvx.Resolve<IMvxFormsSetupHelper>());
            }
        }

        protected override void RegisterImplementations()
        {
            base.RegisterImplementations();
            Mvx.RegisterSingleton<IMvxFormsSetup>(this);

            Mvx.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxFormsWpfViewPresenter>();
            Mvx.LazyConstructAndRegisterSingleton<IMvxFormsSetupHelper, MvxFormsSetupHelper>();
            Mvx.Resolve<IMvxFormsSetupHelper>().InitializeIoC();
            Mvx.LazyConstructAndRegisterSingleton(() => FormsPresenter);
        }

        protected virtual void RegisterSetupHelper()
        {
            Mvx.LazyConstructAndRegisterSingleton<IMvxFormsSetupHelper, MvxFormsSetupHelper>();
        }


        public virtual Application FormsApplication
        {
            get
            {
                if (!Xamarin.Forms.Forms.IsInitialized)
                    Xamarin.Forms.Forms.Init();

                if (_formsApplication == null)
                {
                    _formsApplication = CreateFormsApplication();
                }
                if (Application.Current != _formsApplication)
                {
                    Application.Current = _formsApplication;
                }
                return _formsApplication;
            }
        }

        protected abstract Application CreateFormsApplication();

        protected IMvxFormsViewPresenter FormsPresenter
        {
            get
            {
                return base.ViewPresenter as IMvxFormsViewPresenter;
            }
        }

        protected override IMvxViewPresenter CreateViewPresenter()
        {
            return FormsSetupHelper.InitializeFormsViewPresenter(base.CreateViewPresenter() as IMvxFormsViewPresenter, FormsApplication);
        }

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsWindowsBindingBuilder();
    }

    public class MvxFormsWpfSetup<TApplication, TFormsApplication> : MvxFormsWpfSetup
        where TApplication : class, IMvxApplication, new()
        where TFormsApplication : Application, new()
    {
        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            return new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(TFormsApplication).GetTypeInfo().Assembly }));
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }

        protected override Application CreateFormsApplication() => new TFormsApplication();

        protected override IMvxApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();
    }
}
