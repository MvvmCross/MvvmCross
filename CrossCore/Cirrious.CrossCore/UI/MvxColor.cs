// MvxColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.UI
{
    public class MvxColor
    {
        #region Predefined Colors
        
        //Colors taken from .Net's System.Drawing
        public static MvxColor AliceBlue = new MvxColor(240, 248, 255, 255);
        public static MvxColor AntiqueWhite = new MvxColor(250, 235, 215, 255);
        public static MvxColor Aqua = new MvxColor(0, 255, 255, 255);
        public static MvxColor Aquamarine = new MvxColor(127, 255, 212, 255);
        public static MvxColor Azure = new MvxColor(240, 255, 255, 255);
        public static MvxColor Beige = new MvxColor(245, 245, 220, 255);
        public static MvxColor Bisque = new MvxColor(255, 228, 196, 255);
        public static MvxColor Black = new MvxColor(0, 0, 0, 255);
        public static MvxColor BlanchedAlmond = new MvxColor(255, 235, 205, 255);
        public static MvxColor Blue = new MvxColor(0, 0, 255, 255);
        public static MvxColor BlueViolet = new MvxColor(138, 43, 226, 255);
        public static MvxColor Brown = new MvxColor(165, 42, 42, 255);
        public static MvxColor BurlyWood = new MvxColor(222, 184, 135, 255);
        public static MvxColor CadetBlue = new MvxColor(95, 158, 160, 255);
        public static MvxColor Chartreuse = new MvxColor(127, 255, 0, 255);
        public static MvxColor Chocolate = new MvxColor(210, 105, 30, 255);
        public static MvxColor Coral = new MvxColor(255, 127, 80, 255);
        public static MvxColor CornflowerBlue = new MvxColor(100, 149, 237, 255);
        public static MvxColor Cornsilk = new MvxColor(255, 248, 220, 255);
        public static MvxColor Crimson = new MvxColor(220, 20, 60, 255);
        public static MvxColor Cyan = new MvxColor(0, 255, 255, 255);
        public static MvxColor DarkBlue = new MvxColor(0, 0, 139, 255);
        public static MvxColor DarkCyan = new MvxColor(0, 139, 139, 255);
        public static MvxColor DarkGoldenrod = new MvxColor(184, 134, 11, 255);
        public static MvxColor DarkGray = new MvxColor(169, 169, 169, 255);
        public static MvxColor DarkGreen = new MvxColor(0, 100, 0, 255);
        public static MvxColor DarkKhaki = new MvxColor(189, 183, 107, 255);
        public static MvxColor DarkMagenta = new MvxColor(139, 0, 139, 255);
        public static MvxColor DarkOliveGreen = new MvxColor(85, 107, 47, 255);
        public static MvxColor DarkOrange = new MvxColor(255, 140, 0, 255);
        public static MvxColor DarkOrchid = new MvxColor(153, 50, 204, 255);
        public static MvxColor DarkRed = new MvxColor(139, 0, 0, 255);
        public static MvxColor DarkSalmon = new MvxColor(233, 150, 122, 255);
        public static MvxColor DarkSeaGreen = new MvxColor(143, 188, 139, 255);
        public static MvxColor DarkSlateBlue = new MvxColor(72, 61, 139, 255);
        public static MvxColor DarkSlateGray = new MvxColor(47, 79, 79, 255);
        public static MvxColor DarkTurquoise = new MvxColor(0, 206, 209, 255);
        public static MvxColor DarkViolet = new MvxColor(148, 0, 211, 255);
        public static MvxColor DeepPink = new MvxColor(255, 20, 147, 255);
        public static MvxColor DeepSkyBlue = new MvxColor(0, 191, 255, 255);
        public static MvxColor DimGray = new MvxColor(105, 105, 105, 255);
        public static MvxColor DodgerBlue = new MvxColor(30, 144, 255, 255);
        public static MvxColor Firebrick = new MvxColor(178, 34, 34, 255);
        public static MvxColor FloralWhite = new MvxColor(255, 250, 240, 255);
        public static MvxColor ForestGreen = new MvxColor(34, 139, 34, 255);
        public static MvxColor Fuchsia = new MvxColor(255, 0, 255, 255);
        public static MvxColor Gainsboro = new MvxColor(220, 220, 220, 255);
        public static MvxColor GhostWhite = new MvxColor(248, 248, 255, 255);
        public static MvxColor Gold = new MvxColor(255, 215, 0, 255);
        public static MvxColor Goldenrod = new MvxColor(218, 165, 32, 255);
        public static MvxColor Gray = new MvxColor(128, 128, 128, 255);
        public static MvxColor Green = new MvxColor(0, 128, 0, 255);
        public static MvxColor GreenYellow = new MvxColor(173, 255, 47, 255);
        public static MvxColor Honeydew = new MvxColor(240, 255, 240, 255);
        public static MvxColor HotPink = new MvxColor(255, 105, 180, 255);
        public static MvxColor IndianRed = new MvxColor(205, 92, 92, 255);
        public static MvxColor Indigo = new MvxColor(75, 0, 130, 255);
        public static MvxColor Ivory = new MvxColor(255, 255, 240, 255);
        public static MvxColor Khaki = new MvxColor(240, 230, 140, 255);
        public static MvxColor Lavender = new MvxColor(230, 230, 250, 255);
        public static MvxColor LavenderBlush = new MvxColor(255, 240, 245, 255);
        public static MvxColor LawnGreen = new MvxColor(124, 252, 0, 255);
        public static MvxColor LemonChiffon = new MvxColor(255, 250, 205, 255);
        public static MvxColor LightBlue = new MvxColor(173, 216, 230, 255);
        public static MvxColor LightCoral = new MvxColor(240, 128, 128, 255);
        public static MvxColor LightCyan = new MvxColor(224, 255, 255, 255);
        public static MvxColor LightGoldenrodYellow = new MvxColor(250, 250, 210, 255);
        public static MvxColor LightGray = new MvxColor(211, 211, 211, 255);
        public static MvxColor LightGreen = new MvxColor(144, 238, 144, 255);
        public static MvxColor LightPink = new MvxColor(255, 182, 193, 255);
        public static MvxColor LightSalmon = new MvxColor(255, 160, 122, 255);
        public static MvxColor LightSeaGreen = new MvxColor(32, 178, 170, 255);
        public static MvxColor LightSkyBlue = new MvxColor(135, 206, 250, 255);
        public static MvxColor LightSlateGray = new MvxColor(119, 136, 153, 255);
        public static MvxColor LightSteelBlue = new MvxColor(176, 196, 222, 255);
        public static MvxColor LightYellow = new MvxColor(255, 255, 224, 255);
        public static MvxColor Lime = new MvxColor(0, 255, 0, 255);
        public static MvxColor LimeGreen = new MvxColor(50, 205, 50, 255);
        public static MvxColor Linen = new MvxColor(250, 240, 230, 255);
        public static MvxColor Magenta = new MvxColor(255, 0, 255, 255);
        public static MvxColor Maroon = new MvxColor(128, 0, 0, 255);
        public static MvxColor MediumAquamarine = new MvxColor(102, 205, 170, 255);
        public static MvxColor MediumBlue = new MvxColor(0, 0, 205, 255);
        public static MvxColor MediumOrchid = new MvxColor(186, 85, 211, 255);
        public static MvxColor MediumPurple = new MvxColor(147, 112, 219, 255);
        public static MvxColor MediumSeaGreen = new MvxColor(60, 179, 113, 255);
        public static MvxColor MediumSlateBlue = new MvxColor(123, 104, 238, 255);
        public static MvxColor MediumSpringGreen = new MvxColor(0, 250, 154, 255);
        public static MvxColor MediumTurquoise = new MvxColor(72, 209, 204, 255);
        public static MvxColor MediumVioletRed = new MvxColor(199, 21, 133, 255);
        public static MvxColor MidnightBlue = new MvxColor(25, 25, 112, 255);
        public static MvxColor MintCream = new MvxColor(245, 255, 250, 255);
        public static MvxColor MistyRose = new MvxColor(255, 228, 225, 255);
        public static MvxColor Moccasin = new MvxColor(255, 228, 181, 255);
        public static MvxColor NavajoWhite = new MvxColor(255, 222, 173, 255);
        public static MvxColor Navy = new MvxColor(0, 0, 128, 255);
        public static MvxColor OldLace = new MvxColor(253, 245, 230, 255);
        public static MvxColor Olive = new MvxColor(128, 128, 0, 255);
        public static MvxColor OliveDrab = new MvxColor(107, 142, 35, 255);
        public static MvxColor Orange = new MvxColor(255, 165, 0, 255);
        public static MvxColor OrangeRed = new MvxColor(255, 69, 0, 255);
        public static MvxColor Orchid = new MvxColor(218, 112, 214, 255);
        public static MvxColor PaleGoldenrod = new MvxColor(238, 232, 170, 255);
        public static MvxColor PaleGreen = new MvxColor(152, 251, 152, 255);
        public static MvxColor PaleTurquoise = new MvxColor(175, 238, 238, 255);
        public static MvxColor PaleVioletRed = new MvxColor(219, 112, 147, 255);
        public static MvxColor PapayaWhip = new MvxColor(255, 239, 213, 255);
        public static MvxColor PeachPuff = new MvxColor(255, 218, 185, 255);
        public static MvxColor Peru = new MvxColor(205, 133, 63, 255);
        public static MvxColor Pink = new MvxColor(255, 192, 203, 255);
        public static MvxColor Plum = new MvxColor(221, 160, 221, 255);
        public static MvxColor PowderBlue = new MvxColor(176, 224, 230, 255);
        public static MvxColor Purple = new MvxColor(128, 0, 128, 255);
        public static MvxColor Red = new MvxColor(255, 0, 0, 255);
        public static MvxColor RosyBrown = new MvxColor(188, 143, 143, 255);
        public static MvxColor RoyalBlue = new MvxColor(65, 105, 225, 255);
        public static MvxColor SaddleBrown = new MvxColor(139, 69, 19, 255);
        public static MvxColor Salmon = new MvxColor(250, 128, 114, 255);
        public static MvxColor SandyBrown = new MvxColor(244, 164, 96, 255);
        public static MvxColor SeaGreen = new MvxColor(46, 139, 87, 255);
        public static MvxColor SeaShell = new MvxColor(255, 245, 238, 255);
        public static MvxColor Sienna = new MvxColor(160, 82, 45, 255);
        public static MvxColor Silver = new MvxColor(192, 192, 192, 255);
        public static MvxColor SkyBlue = new MvxColor(135, 206, 235, 255);
        public static MvxColor SlateBlue = new MvxColor(106, 90, 205, 255);
        public static MvxColor SlateGray = new MvxColor(112, 128, 144, 255);
        public static MvxColor Snow = new MvxColor(255, 250, 250, 255);
        public static MvxColor SpringGreen = new MvxColor(0, 255, 127, 255);
        public static MvxColor SteelBlue = new MvxColor(70, 130, 180, 255);
        public static MvxColor Tan = new MvxColor(210, 180, 140, 255);
        public static MvxColor Teal = new MvxColor(0, 128, 128, 255);
        public static MvxColor Thistle = new MvxColor(216, 191, 216, 255);
        public static MvxColor Tomato = new MvxColor(255, 99, 71, 255);
        public static MvxColor Transparent = new MvxColor(255, 255, 255, 0);
        public static MvxColor Turquoise = new MvxColor(64, 224, 208, 255);
        public static MvxColor Violet = new MvxColor(238, 130, 238, 255);
        public static MvxColor Wheat = new MvxColor(245, 222, 179, 255);
        public static MvxColor White = new MvxColor(255, 255, 255, 255);
        public static MvxColor WhiteSmoke = new MvxColor(245, 245, 245, 255);
        public static MvxColor Yellow = new MvxColor(255, 255, 0, 255);
        public static MvxColor YellowGreen = new MvxColor(154, 205, 50, 255);
        
        #endregion

        public int ARGB { get; set;}

		private static int MaskAndShiftRight(int value, uint mask, int shift)
		{
			return  (int) ((value & mask) >> shift);
		}

		private static int ShiftOverwrite(int original, uint mask, int value, int shift)
		{
			var maskedOriginal = (original & mask);
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

		public MvxColor(int argb)
		{
			ARGB = argb;
		}

		public MvxColor(int rgb, int alpha = 255)
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
            return string.Format("rgba: #{0:X2}{1:X2}{2:X2}{3:X2}", 
                R, G, B, A);
        }
    }
}