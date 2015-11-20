// MvxCanCreateTouchViewExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Touch.Views
{
    public static class MvxCanCreateTouchViewExtensionMethods
    {
        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(this IMvxCanCreateTouchView view,
                                                                              object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject?.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?

        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateTouchView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null,
                                                                    MvxRequestedBy.UserAction);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateTouchView view,
            MvxViewModelRequest request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return Mvx.Resolve<IMvxTouchViewCreator>().CreateView(request);
        }

        public static IMvxTouchView CreateViewControllerFor(
            this IMvxCanCreateTouchView view,
            MvxViewModelRequest request)
        {
            return Mvx.Resolve<IMvxTouchViewCreator>().CreateView(request);
        }

        public static IMvxTouchView CreateViewControllerFor(
            this IMvxCanCreateTouchView view,
            IMvxViewModel viewModel)
        {
            return Mvx.Resolve<IMvxTouchViewCreator>().CreateView(viewModel);
        }
    }
}