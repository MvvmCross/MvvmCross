using System.Globalization;

namespace MvvmCross.Plugin.Color
{
    public static class MvxHexParser
    {
        public static System.Drawing.Color ColorFromHexString(string value) => ColorFromHexString(value, assumeArgb: false);

        public static System.Drawing.Color ColorFromHexString(string value, bool assumeArgb)
        {
            if (string.IsNullOrEmpty(value))
                return new System.Drawing.Color();

            value = value.TrimStart('#');
            if (value.Length == 0)
                return new System.Drawing.Color();

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
                    return new System.Drawing.Color();
            }
        }

        public static void ParseRGBInteger(int rgb, out int red, out int green, out int blue)
        {
            //Parse the individual RGB values from the RGB Integer
            red = (rgb >> 16) & 0xFF;
            green = (rgb >> 8) & 0xFF;
            blue = rgb & 0xFF;
        }

        private static int UpByte(int input)
        {
            var fourBit = input & 0xF;
            var output = fourBit << 4;
            output |= fourBit;
            return output;
        }

        private static System.Drawing.Color Parse3DigitColor(string value)
        {
            var red = int.Parse(value.Substring(0, 1), NumberStyles.HexNumber);
            var green = int.Parse(value.Substring(1, 1), NumberStyles.HexNumber);
            var blue = int.Parse(value.Substring(2, 1), NumberStyles.HexNumber);

            return System.Drawing.Color.FromArgb(UpByte(red), UpByte(green), UpByte(blue));
        }

        private static System.Drawing.Color Parse6DigitColor(string value)
        {
            var rgb = int.Parse(value, NumberStyles.HexNumber);

            //Parse the individual RGB values from the RGB Integer
            int red = (rgb >> 16) & 0xFF;
            int green = (rgb >> 8) & 0xFF;
            int blue = rgb & 0xFF;

            var color = System.Drawing.Color.FromArgb(red, green, blue);

            return color;
        }

        private static System.Drawing.Color Parse4DigitARBGColor(string value)
        {
            var alpha = int.Parse(value.Substring(0, 1), NumberStyles.HexNumber);
            var red = int.Parse(value.Substring(1, 1), NumberStyles.HexNumber);
            var green = int.Parse(value.Substring(2, 1), NumberStyles.HexNumber);
            var blue = int.Parse(value.Substring(3, 1), NumberStyles.HexNumber);

            return System.Drawing.Color.FromArgb(UpByte(alpha), UpByte(red), UpByte(green), UpByte(blue));
        }

        private static System.Drawing.Color Parse4DigitRBGAColor(string value)
        {
            var red = int.Parse(value.Substring(0, 1), NumberStyles.HexNumber);
            var green = int.Parse(value.Substring(1, 1), NumberStyles.HexNumber);
            var blue = int.Parse(value.Substring(2, 1), NumberStyles.HexNumber);
            var alpha = int.Parse(value.Substring(3, 1), NumberStyles.HexNumber);

            return System.Drawing.Color.FromArgb(UpByte(alpha), UpByte(red), UpByte(green), UpByte(blue));
        }

        private static System.Drawing.Color Parse8DigitARGBColor(string value)
        {
            var a = int.Parse(value.Substring(0, 2), NumberStyles.HexNumber);
            var rgb = int.Parse(value.Substring(2, 6), NumberStyles.HexNumber);

            ParseRGBInteger(rgb, out int red, out int green, out int blue);

            return System.Drawing.Color.FromArgb(a, red, green, blue);
        }

        private static System.Drawing.Color Parse8DigitRGBAColor(string value)
        {
            var rgb = int.Parse(value.Substring(0, 6), NumberStyles.HexNumber);
            var a = int.Parse(value.Substring(6, 2), NumberStyles.HexNumber);

            ParseRGBInteger(rgb, out int red, out int green, out int blue);

            return System.Drawing.Color.FromArgb(a, red, green, blue);
        }
    }
}
