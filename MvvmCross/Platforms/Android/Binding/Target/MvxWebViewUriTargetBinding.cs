#nullable enable
using System.Diagnostics.CodeAnalysis;
using Android.Webkit;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxWebViewUriTargetBinding(WebView webView) : MvxAndroidTargetBinding(webView)
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public override Type TargetValueType => typeof(string);
    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is WebView view && value is string uri)
        {
            view.LoadUrl(uri);
        }
    }
}
