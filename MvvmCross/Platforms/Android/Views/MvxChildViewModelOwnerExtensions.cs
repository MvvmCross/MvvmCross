// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Android.Content;
using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views
{
    public static class MvxChildViewModelOwnerExtensions
    {
        public static Intent CreateIntentFor<TTargetViewModel>(this IMvxAndroidView view, object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return view.CreateIntentFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());
        }

        public static Intent CreateIntentFor<TTargetViewModel>(this IMvxAndroidView view,
                                                               IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateIntentFor(request);
        }

        public static Intent CreateIntentFor(this IMvxAndroidView view, MvxViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>().GetIntentFor(request);
        }

        public static Intent CreateIntentFor(this IMvxChildViewModelOwner view, IMvxViewModel subViewModel)
        {
            var intentWithKey =
                Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>().GetIntentWithKeyFor(subViewModel);
            view.OwnedSubViewModelIndicies.Add(intentWithKey.Item2);
            return intentWithKey.Item1;
        }

        public static void ClearOwnedSubIndicies(this IMvxChildViewModelOwner view)
        {
            var translator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();
            foreach (var ownedSubViewModelIndex in view.OwnedSubViewModelIndicies)
            {
                translator.RemoveSubViewModelWithKey(ownedSubViewModelIndex);
            }
            view.OwnedSubViewModelIndicies.Clear();
        }
    }
}
