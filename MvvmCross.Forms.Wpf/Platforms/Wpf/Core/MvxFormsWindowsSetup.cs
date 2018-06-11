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
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Wpf.Core
{
    public abstract class MvxFormsWpfSetup : MvxWpfSetup
    {
        private Application _formsApplication;
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
                if(Application.Current != _formsApplication)
                {
                    Application.Current = _formsApplication;
                }
                return _formsApplication;
            }
        }

        protected abstract Application CreateFormsApplication();

        protected override IMvxWpfViewPresenter CreateViewPresenter(ContentControl contentControl)
        {
            var presenter = new MvxFormsWpfViewPresenter(contentControl, FormsApplication);
            Mvx.RegisterSingleton<IMvxFormsViewPresenter>(presenter);
            return presenter;
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

        protected override IMvxApplication CreateApp() => Mvx.IoCConstruct<TApplication>();
    }
}
