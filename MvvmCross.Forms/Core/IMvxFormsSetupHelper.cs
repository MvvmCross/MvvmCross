// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Presenters;
using MvvmCross.Presenters;
using Xamarin.Forms;

namespace MvvmCross.Forms.Core
{
    public interface IMvxFormsSetupHelper
    {
        void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry);
        void FillBindingNames(IMvxBindingNameRegistry registry);

        IMvxViewPresenter SetupFormsViewPresenter(IMvxFormsViewPresenter presenter, Application formsApplication);
        void InitializeIoC();
    }
}
