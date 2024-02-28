// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using Android.Content.Res;
using Android.Views;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxViewMarginTargetBinding : MvxAndroidTargetBinding
{
    private readonly string _whichMargin;

    public MvxViewMarginTargetBinding(View target, string whichMargin) : base(target)
    {
        ArgumentException.ThrowIfNullOrEmpty(whichMargin);
        _whichMargin = whichMargin;
    }

    public override Type TargetValueType => typeof(float);
    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is not View view || value == null)
            return;

        if (view.LayoutParameters is not ViewGroup.MarginLayoutParams layoutParameters)
            return;

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

    private static int DpToPx(float dp)
        => (int)(dp * Resources.System?.DisplayMetrics?.Density ?? 1.6f);
}
