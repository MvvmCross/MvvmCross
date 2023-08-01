using Android.Content;
using Android.Runtime;
using Android.Util;
using Bumptech.Glide;

namespace Playground.Droid.Activities;

[Register("playground.droid.GlideImageView")]
public sealed class GlideImageView : ImageView
{
    private string _imagePath;

    public string ImagePath
    {
        get => _imagePath;
        set
        {
            if (value == null)
                return;

            _imagePath = value;
            Glide.With(Context).Load(_imagePath).Into(this);
        }
    }

    public GlideImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }

    public GlideImageView(Context context) : base(context)
    {
    }

    public GlideImageView(Context context, IAttributeSet attrs) : base(context, attrs)
    {
    }

    public GlideImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
    {
    }

    public GlideImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
    {
    }
}
