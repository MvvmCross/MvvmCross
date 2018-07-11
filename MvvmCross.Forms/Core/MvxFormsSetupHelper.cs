// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Bindings.Target;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using Xamarin.Forms;

namespace MvvmCross.Forms.Core
{
    public class MvxFormsSetupHelper : IMvxFormsSetupHelper
    {
        private Application _formsApplication;
        public virtual Application FormsApplication
        {
            get
            {
                if (_formsApplication == null)
                {
                    _formsApplication = Mvx.IoCProvider.Resolve<Application>();
                }

                if (Application.Current != _formsApplication)
                {
                    Application.Current = _formsApplication;
                }

                return _formsApplication;
            }
        }

        public virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxListViewItemClickPropertyTargetBinding),
                typeof(MvxListView), 
                MvxFormsPropertyBinding.MvxListView_ItemClick);
        }

        public virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            
        }

        public virtual IMvxViewPresenter InitializeFormsViewPresenter(IMvxFormsViewPresenter presenter, Application formsApplication)
        {
            Mvx.IoCProvider.RegisterSingleton(presenter);
            presenter.FormsApplication = formsApplication;
            presenter.FormsPagePresenter = InitializeFormsPagePresenter(presenter);
            return presenter;
        }

        protected virtual IMvxFormsPagePresenter InitializeFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            var formsPagePresenter = Mvx.IoCProvider.Resolve<IMvxFormsPagePresenter>();
            formsPagePresenter.PlatformPresenter = viewPresenter;
            return formsPagePresenter;
        }
    }
}
