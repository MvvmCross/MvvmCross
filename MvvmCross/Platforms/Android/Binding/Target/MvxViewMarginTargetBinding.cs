// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content.Res;
using Android.Views;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxViewMarginTargetBinding : MvxAndroidTargetBinding
    {
        private string _whichMargin;

        public MvxViewMarginTargetBinding(View target, string whichMargin) : base(target)
        {
            if (whichMargin == null) throw new ArgumentNullException(nameof(whichMargin));
            _whichMargin = whichMargin;
        }

        public override Type TargetValueType => typeof(float);
        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as View;
            if (view == null) return;

            var layoutParameters = view.LayoutParameters as ViewGroup.MarginLayoutParams;
            if (layoutParameters == null) return;

            var dpMargin = (float)value;
            var pxMargin = DpToPx(dpMargin);

            switch (_whichMargin)
            {
                case MvxAndroidPropertyBinding.View_Margin:
                    layoutParameters.SetMargins(pxMargin, pxMargin, pxMargin, pxMargin);
                    break;
                case MvxAndroidPropertyBinding.View_MarginLeft:
                    layoutParameters.LeftMargin = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginRight:
                    layoutParameters.RightMargin = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginTop:
                    layoutParameters.TopMargin = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginBottom:
                    layoutParameters.BottomMargin = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginEnd:
                    layoutParameters.MarginEnd = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginStart:
                    layoutParameters.MarginStart = pxMargin;
                    break;
            }

            view.LayoutParameters = layoutParameters;
        }

        private int DpToPx(float dp)
            => (int)(dp * Resources.System.DisplayMetrics.Density);
    }
}
