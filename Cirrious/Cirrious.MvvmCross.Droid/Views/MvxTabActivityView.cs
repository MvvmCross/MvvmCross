// MvxTabActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Android.App;
using Android.Content;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public abstract partial class MvxTabActivityView<TViewModel> : TabActivity
    {
        private readonly List<int> _ownedSubViewModelIndicies = new List<int>();

        protected MvxTabActivityView()
        {
            IsVisible = true;
        }

        protected Intent CreateIntentFor<TTargetViewModel>(object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return CreateIntentFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());
        }

        protected Intent CreateIntentFor<TTargetViewModel>(IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            parameterValues = parameterValues ?? new Dictionary<string, string>();
            var request = new MvxShowViewModelRequest<TTargetViewModel>(parameterValues, false,
                                                                        MvxRequestedBy.UserAction);
            return CreateIntentFor(request);
        }

        protected Intent CreateIntentFor(MvxShowViewModelRequest request)
        {
            return this.GetService<IMvxAndroidViewModelRequestTranslator>().GetIntentFor(request);
        }

        protected Intent CreateIntentFor(IMvxViewModel subViewModel)
        {
            var intentWithKey =
                this.GetService<IMvxAndroidViewModelRequestTranslator>().GetIntentWithKeyFor(subViewModel);
            _ownedSubViewModelIndicies.Add(intentWithKey.Item2);
            return intentWithKey.Item1;
        }

        private void ClearOwnedSubIndicies()
        {
            var translator = this.GetService<IMvxAndroidViewModelRequestTranslator>();
            foreach (var ownedSubViewModelIndex in _ownedSubViewModelIndicies)
            {
                translator.RemoveSubViewModelWithKey(ownedSubViewModelIndex);
            }
            _ownedSubViewModelIndicies.Clear();
        }
    }
}