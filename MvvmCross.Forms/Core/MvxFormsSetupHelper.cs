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

        public virtual IMvxViewPresenter SetupFormsViewPresenter(IMvxFormsViewPresenter presenter, Application formsApplication)
        {
            presenter.FormsApplication = formsApplication;
            presenter.FormsPagePresenter = CreateFormsPagePresenter(presenter);
            return presenter;
        }

        protected virtual IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            var formsPagePresenter = Mvx.Resolve<IMvxFormsPagePresenter>();
            formsPagePresenter.PlatformPresenter = viewPresenter;
            return formsPagePresenter;
        }

        public void InitializeIoC()
        {
            Mvx.LazyConstructAndRegisterSingleton<IMvxFormsPagePresenter, MvxFormsPagePresenter>();
        }
    }
}
