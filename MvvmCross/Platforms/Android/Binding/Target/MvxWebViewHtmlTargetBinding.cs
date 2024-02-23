#nullable enable
using Android.Webkit;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxWebViewHtmlTargetBinding(object target)
    : MvxAndroidTargetBinding(target)
{
    public override Type TargetValueType => typeof(string);
    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is WebView webView && value is string html)
        {
            webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
        }
    }
}
