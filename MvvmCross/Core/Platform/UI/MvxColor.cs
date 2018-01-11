// MvxColor.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Globalization;

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
            var maskedOriginal = original & mask;
            var newBits = value << shift;
            return (int)(maskedOriginal | newBits);
        }

        public int R
        {
            get { return MaskAndShiftRight(ARGB, 0xFF0000, 16); }
            set { ARGB = ShiftOverwrite(ARGB, 0xFF00FFFF, value, 16); }
        }

        public int G
        {
            get { return MaskAndShiftRight(ARGB, 0xFF00, 8); }
            set { ARGB = ShiftOverwrite(ARGB, 0xFFFF00FF, value, 8); }
        }

        public int B
        {
            get { return MaskAndShiftRight(ARGB, 0xFF, 0); }
            set { ARGB = ShiftOverwrite(ARGB, 0xFFFFFF00, value, 0); }
        }

        public int A
        {
            get { return MaskAndShiftRight(ARGB, 0xFF000000, 24); }
            set { ARGB = ShiftOverwrite(ARGB, 0x00FFFFFF, value, 24); }
        }

        public MvxColor(uint argb)
            : this((int)argb)
        {
        }

        public MvxColor(int argb)
        {
            ARGB = argb;
        }

        public MvxColor(uint rgb, int alpha)
            : this((int)rgb, alpha)
        {
        }

        public MvxColor(int rgb, int alpha)
        {
            ARGB = rgb;
            A = alpha;
        }

        public MvxColor(int red, int green, int blue, int alpha = 255)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public override string ToString()
        {
            return $"argb: #{A:X2}{R:X2}{G:X2}{B:X2}";
        }

        public static MvxColor ParseHexString(string value)
            => ParseHexString(value, assumeArgb: false);

        public static MvxColor ParseHexString(string value, bool assumeArgb)
        {
            if (string.IsNullOrEmpty(value))
                return new MvxColor(0);

            value = value.TrimStart('#');
            if (value.Length == 0)
                return new MvxColor(0);

            switch (value.Length)
            {
                case 3:
                    return Parse3DigitColor(value);

                case 4:
                    return assumeArgb ? Parse4DigitARBGColor(value) : Parse4DigitRBGAColor(value);

                case 6:
                    return Parse6DigitColor(value);

                case 8:
                    return assumeArgb ? Parse8DigitARGBColor(value) : Parse8DigitRGBAColor(value);

                default:
                    return new MvxColor(0);
            }
        }

        private static int UpByte(int input)
        {
            var fourBit = input & 0xF;
            var output = fourBit << 4;
            output |= fourBit;
            return output;
        }

        private static MvxColor Parse3DigitColor(string value)
        {
            var red = int.Parse(value.Substring(0, 1), NumberStyles.HexNumber);
            var green = int.Parse(value.Substring(1, 1), NumberStyles.HexNumber);
            var blue = int.Parse(value.Substring(2, 1), NumberStyles.HexNumber);
            return new MvxColor(UpByte(red), UpByte(green), UpByte(blue));
        }

        private static MvxColor Parse6DigitColor(string value)
        {
            var rgb = int.Parse(value, NumberStyles.HexNumber);
            return new MvxColor(rgb, 255);
        }

        private static MvxColor Parse4DigitARBGColor(string value)
        {
            var alpha = int.Parse(value.Substring(0, 1), NumberStyles.HexNumber);
            var red = int.Parse(value.Substring(1, 1), NumberStyles.HexNumber);
            var green = int.Parse(value.Substring(2, 1), NumberStyles.HexNumber);
            var blue = int.Parse(value.Substring(3, 1), NumberStyles.HexNumber);
            return new MvxColor(UpByte(red), UpByte(green), UpByte(blue), UpByte(alpha));
        }

        private static MvxColor Parse4DigitRBGAColor(string value)
        {
            var red = int.Parse(value.Substring(0, 1), NumberStyles.HexNumber);
            var green = int.Parse(value.Substring(1, 1), NumberStyles.HexNumber);
            var blue = int.Parse(value.Substring(2, 1), NumberStyles.HexNumber);
            var alpha = int.Parse(value.Substring(3, 1), NumberStyles.HexNumber);
            return new MvxColor(UpByte(red), UpByte(green), UpByte(blue), UpByte(alpha));
        }

        private static MvxColor Parse8DigitARGBColor(string value)
        {
            var a = int.Parse(value.Substring(0, 2), NumberStyles.HexNumber);
            var rgb = int.Parse(value.Substring(2, 6), NumberStyles.HexNumber);
            return new MvxColor(rgb, a);
        }

        private static MvxColor Parse8DigitRGBAColor(string value)
        {
            var rgb = int.Parse(value.Substring(0, 6), NumberStyles.HexNumber);
            var a = int.Parse(value.Substring(6, 2), NumberStyles.HexNumber);
            return new MvxColor(rgb, a);
        }
    }
}
