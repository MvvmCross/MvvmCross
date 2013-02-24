// MvxActivityViewExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.BindingContext;
using Cirrious.MvvmCross.Droid.Interfaces;

namespace Cirrious.MvvmCross.Droid.Views
{
    public static class MvxActivityViewExtensions
    {
        public static void AddEventListeners(this IMvxEventSourceActivity activity)
        {
            if (activity is IMvxAndroidView)
            {
                var adapter = new MvxActivityAdapter(activity);
            }
            if (activity is IMvxBindingContextOwner)
            {
                var bindingAdapter = new MvxBindingActivityAdapter(activity);
            }
            if (activity is IMvxChildViewModelOwner)
            {
                var childOwnerAdapter = new MvxChildViewModelOwnerAdapter(activity);
            }
        }
    }
}