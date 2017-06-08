// MvxViewBackgroundColorBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;

namespace MvvmCross.Plugins.Color.Droid.BindingTargets
{
    [Preserve(AllMembers = true)]
    public class MvxViewBackgroundColorBinding
        : MvxViewColorBinding
    {
        public MvxViewBackgroundColorBinding(View view)
            : base(view)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = (View)target;
            view?.SetBackgroundColor((Android.Graphics.Color) value);
        }
    }
}