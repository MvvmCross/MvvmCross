namespace Cirrious.MvvmCross.Converters.Color
{
    public class MvxColor
    {
        protected int R { get; set; }
        protected int G { get; set; }
        protected int B { get; set; }
        protected int A { get; set; }

        public MvxColor(int rgb, int alpha = 255)
        {
            R = (rgb & 0xFF0000) >> 16;
            G = (rgb & 0xFF00) >> 8;
            B = (rgb & 0xFF);
            A = alpha;
        }

        public MvxColor(int red, int green, int blue, int alpha = 255)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

#if WINDOWS_CONSOLE
        public System.Drawing.Color ToNative()
        {
            var color = System.Drawing.Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
            return color;
        }
#endif

#if WINDOWS_PHONE
        public System.Windows.Media.SolidColorBrush ToNative()
        {
            var color = System.Windows.Media.Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
            return new System.Windows.Media.SolidColorBrush(color);
        }
#endif

#if MonoDroid
        public global::Android.Graphics.Color ToNative()
        {
            return new global::Android.Graphics.Color(R, G, B, A);
        }
#endif

#if MONOTOUCH
        public MonoTouch.UIKit.UIColor ToNative()
        {
            return new MonoTouch.UIKit.UIColor(R, G, B, A);
        }
#endif
    }
}