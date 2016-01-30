// MvxCanCreateIosViewExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System.Collections.Generic;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform;

    public static class MvxCanCreateIosViewExtensionMethods
    {
        public static IMvxIosView CreateViewControllerFor<TTargetViewModel>(this IMvxCanCreateIosView view,
                                                                              object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject?.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?

        public static IMvxIosView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateIosView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null,
                                                                    MvxRequestedBy.UserAction);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxIosView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateIosView view,
            MvxViewModelRequest request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return Mvx.Resolve<IMvxIosViewCreator>().CreateView(request);
        }

        public static IMvxIosView CreateViewControllerFor(
            this IMvxCanCreateIosView view,
            MvxViewModelRequest request)
        {
            return Mvx.Resolve<IMvxIosViewCreator>().CreateView(request);
        }

        public static IMvxIosView CreateViewControllerFor(
            this IMvxCanCreateIosView view,
            IMvxViewModel viewModel)
        {
            return Mvx.Resolve<IMvxIosViewCreator>().CreateView(viewModel);
        }
    }
}