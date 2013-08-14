// MvxViewVisibleBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxViewVisibleBinding : MvxAndroidTargetBinding
    {
        protected View View
        {
            get { return (View)Target; }
        }

        public MvxViewVisibleBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType
        {
            get { return typeof (bool); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override void SetValue(object value)
        {
            var view = View;
            if (view == null)
                return;

            view.Visibility = ((bool) value) ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}