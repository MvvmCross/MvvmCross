// MvxViewBackgroundColorBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxViewBackgroundColorBinding
        : MvxViewColorBinding
    {
        public MvxViewBackgroundColorBinding(View view)
            : base(view)
        {
        }

        public override void SetValue(object value)
        {
            var view = TextView;
            if (view == null)
                return;
            view.SetBackgroundColor((Android.Graphics.Color) value);
        }
    }
}