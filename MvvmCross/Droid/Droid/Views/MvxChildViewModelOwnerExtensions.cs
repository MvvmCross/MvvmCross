// MvxChildViewModelOwnerExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using System.Collections.Generic;

    using Android.Content;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform;

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
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null,
                                                                        MvxRequestedBy.UserAction);
            return view.CreateIntentFor(request);
        }

        public static Intent CreateIntentFor(this IMvxAndroidView view, MvxViewModelRequest request)
        {
            return Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>().GetIntentFor(request);
        }

        public static Intent CreateIntentFor(this IMvxChildViewModelOwner view, IMvxViewModel subViewModel)
        {
            var intentWithKey =
                Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>().GetIntentWithKeyFor(subViewModel);
            view.OwnedSubViewModelIndicies.Add(intentWithKey.Item2);
            return intentWithKey.Item1;
        }

        public static void ClearOwnedSubIndicies(this IMvxChildViewModelOwner view)
        {
            var translator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();
            foreach (var ownedSubViewModelIndex in view.OwnedSubViewModelIndicies)
            {
                translator.RemoveSubViewModelWithKey(ownedSubViewModelIndex);
            }
            view.OwnedSubViewModelIndicies.Clear();
        }
    }
}