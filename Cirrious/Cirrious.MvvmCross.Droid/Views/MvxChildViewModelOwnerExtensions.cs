using System.Collections.Generic;
using Android.Content;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public static class MvxChildViewModelOwnerExtensions
    {
        public static Intent CreateIntentFor<TTargetViewModel>(this IMvxAndroidView view, object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return view.CreateIntentFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());
        }

        public static Intent CreateIntentFor<TTargetViewModel>(this IMvxAndroidView view, IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            parameterValues = parameterValues ?? new Dictionary<string, string>();
            var request = new MvxShowViewModelRequest<TTargetViewModel>(parameterValues, false,
                                                                        MvxRequestedBy.UserAction);
            return view.CreateIntentFor(request);
        }

        public static Intent CreateIntentFor(this IMvxAndroidView view, MvxShowViewModelRequest request)
        {
            return view.GetService<IMvxAndroidViewModelRequestTranslator>().GetIntentFor(request);
        }

        public static Intent CreateIntentFor(this IMvxChildViewModelOwner view, IMvxViewModel subViewModel)
        {
            var intentWithKey =
                view.GetService<IMvxAndroidViewModelRequestTranslator>().GetIntentWithKeyFor(subViewModel);
            view.OwnedSubViewModelIndicies.Add(intentWithKey.Item2);
            return intentWithKey.Item1;
        }

        public static void ClearOwnedSubIndicies(this IMvxChildViewModelOwner view)
        {
            var translator = view.GetService<IMvxAndroidViewModelRequestTranslator>();
            foreach (var ownedSubViewModelIndex in view.OwnedSubViewModelIndicies)
            {
                translator.RemoveSubViewModelWithKey(ownedSubViewModelIndex);
            }
            view.OwnedSubViewModelIndicies.Clear();
        }        
    }
}