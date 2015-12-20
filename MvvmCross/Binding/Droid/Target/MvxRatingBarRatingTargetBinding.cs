// MvxAndroidTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Widget;

    public class MvxRatingBarRatingTargetBinding : MvxAndroidTargetBinding
    {
        protected RatingBar RatingBar => (RatingBar)Target;

        public MvxRatingBarRatingTargetBinding(RatingBar target)
            : base(target)
        {
        }

        public override void SubscribeToEvents()
        {
            this.RatingBar.RatingBarChange += this.RatingBar_RatingBarChange;
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

        public override Type TargetType => typeof(float);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = Target as RatingBar;
                if (target != null)
                {
                    target.RatingBarChange -= this.RatingBar_RatingBarChange;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}