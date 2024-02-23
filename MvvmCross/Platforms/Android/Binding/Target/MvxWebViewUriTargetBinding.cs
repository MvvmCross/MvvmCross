#nullable enable
using Android.Webkit;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxWebViewUriTargetBinding(object target)
    : MvxAndroidTargetBinding(target)
{
    public override Type TargetValueType => typeof(string);
    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is WebView webView && value is string uri)
        {
            webView.LoadUrl(uri);
        }
    }
}
