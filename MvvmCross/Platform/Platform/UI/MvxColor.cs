// MvxColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.UI
{
    public class MvxColor
    {
        public int ARGB { get; set; }

        private static int MaskAndShiftRight(int value, uint mask, int shift)
        {
            return (int)((value & mask) >> shift);
        }

        private static int ShiftOverwrite(int original, uint mask, int value, int shift)
        {
            var maskedOriginal = (original & mask);
            var newBits = value << shift;
            return (int)(maskedOriginal | newBits);
        }

        public int R
        {
            get { return MaskAndShiftRight(this.ARGB, 0xFF0000, 16); }
            set { this.ARGB = ShiftOverwrite(this.ARGB, 0xFF00FFFF, value, 16); }
        }

        public int G
        {
            get { return MaskAndShiftRight(this.ARGB, 0xFF00, 8); }
            set { this.ARGB = ShiftOverwrite(this.ARGB, 0xFFFF00FF, value, 8); }
        }

        public int B
        {
            get { return MaskAndShiftRight(this.ARGB, 0xFF, 0); }
            set { this.ARGB = ShiftOverwrite(this.ARGB, 0xFFFFFF00, value, 0); }
        }

        public int A
        {
            get { return MaskAndShiftRight(this.ARGB, 0xFF000000, 24); }
            set { this.ARGB = ShiftOverwrite(this.ARGB, 0x00FFFFFF, value, 24); }
        }

        public MvxColor(uint argb)
            : this((int)argb)
        {
        }

        public MvxColor(int argb)
        {
            this.ARGB = argb;
        }

        public MvxColor(uint rgb, int alpha)
            : this((int)rgb, alpha)
        {
        }

        public MvxColor(int rgb, int alpha)
        {
            this.ARGB = rgb;
            this.A = alpha;
        }

        public MvxColor(int red, int green, int blue, int alpha = 255)
        {
            this.R = red;
            this.G = green;
            this.B = blue;
            this.A = alpha;
        }

        public override string ToString()
        {
            return $"argb: #{this.A:X2}{this.R:X2}{this.G:X2}{this.B:X2}";
        }
    }
}