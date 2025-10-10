#nullable enable
using System.Diagnostics.CodeAnalysis;
using AndroidUri = Android.Net.Uri;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxVideoViewUriTargetBinding(VideoView videoView) : MvxAndroidTargetBinding(videoView)
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public override Type TargetValueType => typeof(string);

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is VideoView view && value is string uri && !string.IsNullOrWhiteSpace(uri))
        {
            view.SetVideoURI(AndroidUri.Parse(uri));
        }
    }
}
