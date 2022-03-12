// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxRatingBarRatingTargetBinding
        : MvxAndroidTargetBinding
    {
        protected RatingBar RatingBar => (RatingBar)Target;

        private IDisposable _subscription;

        public MvxRatingBarRatingTargetBinding(RatingBar target)
            : base(target)
        {
        }

        public override void SubscribeToEvents()
        {
            _subscription = RatingBar.WeakSubscribe<RatingBar, RatingBar.RatingBarChangeEventArgs>(
                nameof(RatingBar.RatingBarChange),
                RatingBar_RatingBarChange);
        }

        private void RatingBar_RatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            var target = Target as RatingBar;

            if (target == null)
                return;

            var value = target.Rating;
            FireValueChanged(value);
        }

        protected override void SetValueImpl(object target, object value)
        {
            var ratingBar = (RatingBar)target;
            ratingBar.Rating = (float)value;
        }

        public override Type TargetValueType => typeof(float);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}
