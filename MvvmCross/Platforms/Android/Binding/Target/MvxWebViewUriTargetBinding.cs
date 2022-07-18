using System;
using Android.Webkit;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxWebViewUriTargetBinding : MvxAndroidTargetBinding
    {
        public MvxWebViewUriTargetBinding(object target) : base(target) { }

        public override Type TargetValueType => typeof(string);
        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            if (target is WebView webView && value is string uri)
            {
                webView.LoadUrl(uri);
            }
        }
    }
}
