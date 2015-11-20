// MvxViewHiddenBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Cirrious.MvvmCross.Binding.ExtensionMethods;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxViewHiddenBinding
        : MvxBaseViewVisibleBinding
    {
        public MvxViewHiddenBinding(object target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            ((View)target).Visibility = value.ConvertToBoolean() ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}