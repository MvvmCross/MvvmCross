// MvxCanCreateTvosViewExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using System.Collections.Generic;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform;

    public static class MvxCanCreateTvosViewExtensionMethods
    {
        public static IMvxTvosView CreateViewControllerFor<TTargetViewModel>(this IMvxCanCreateTvosView view,
                                                                              object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject?.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?

        public static IMvxTvosView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateTvosView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null,
                                                                    MvxRequestedBy.UserAction);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxTvosView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateTvosView view,
            MvxViewModelRequest request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return Mvx.Resolve<IMvxTvosViewCreator>().CreateView(request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            MvxViewModelRequest request)
        {
            return Mvx.Resolve<IMvxTvosViewCreator>().CreateView(request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            IMvxViewModel viewModel)
        {
            return Mvx.Resolve<IMvxTvosViewCreator>().CreateView(viewModel);
        }
    }
}