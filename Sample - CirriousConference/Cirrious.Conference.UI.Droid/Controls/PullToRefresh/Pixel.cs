using Android.Content.Res;

namespace Cirrious.Conference.UI.Droid.Controls.PullToRefresh
{
    public class Pixel
    {
        private readonly Resources _resources;
        private readonly int _value;

        public Pixel(int value, Resources resources)
        {
            this._value = value;
            this._resources = resources;
        }

        public float ToDp()
        {
            return _value*_resources.DisplayMetrics.Density + 0.5f;
        }
    }
}