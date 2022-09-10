using System;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;
using AndroidUri = Android.Net.Uri;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxVideoViewUriTargetBinding : MvxAndroidTargetBinding
    {
        public MvxVideoViewUriTargetBinding(object target) : base(target) { }

        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            if (target is VideoView videoView && value is string uri && !string.IsNullOrWhiteSpace(uri))
            {
                videoView.SetVideoURI(AndroidUri.Parse(uri));
            }
        }
    }
}
