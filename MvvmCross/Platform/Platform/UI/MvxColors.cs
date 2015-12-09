// MvxColors.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.UI
{
    // The colors in this file are base on the list from:
    // https://github.com/mono/sysdrawing-coregraphics/blob/master/System.Drawing/KnownColors.cs
    // This list used here under Apache license - their copyright is:

    //
    // Copyright (C) 2007 Novell, Inc (http://www.novell.com)
    //
    // Permission is hereby granted, free of charge, to any person obtaining
    // a copy of this software and associated documentation files (the
    // "Software"), to deal in the Software without restriction, including
    // without limitation the rights to use, copy, modify, merge, publish,
    // distribute, sublicense, and/or sell copies of the Software, and to
    // permit persons to whom the Software is furnished to do so, subject to
    // the following conditions:
    //
    // The above copyright notice and this permission notice shall be
    // included in all copies or substantial portions of the Software.
    //
    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    // MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    // LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    // OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    // WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    //

    public static class MvxColors
    {
        public static MvxColor Transparent = new MvxColor(0x00FFFFFF);
        public static MvxColor AliceBlue = new MvxColor(0xFFF0F8FF);
        public static MvxColor AntiqueWhite = new MvxColor(0xFFFAEBD7);
        public static MvxColor Aqua = new MvxColor(0xFF00FFFF);
        public static MvxColor Aquamarine = new MvxColor(0xFF7FFFD4);
        public static MvxColor Azure = new MvxColor(0xFFF0FFFF);
        public static MvxColor Beige = new MvxColor(0xFFF5F5DC);
        public static MvxColor Bisque = new MvxColor(0xFFFFE4C4);
        public static MvxColor Black = new MvxColor(0xFF000000);
        public static MvxColor BlanchedAlmond = new MvxColor(0xFFFFEBCD);
        public static MvxColor Blue = new MvxColor(0xFF0000FF);
        public static MvxColor BlueViolet = new MvxColor(0xFF8A2BE2);
        public static MvxColor Brown = new MvxColor(0xFFA52A2A);
        public static MvxColor BurlyWood = new MvxColor(0xFFDEB887);
        public static MvxColor CadetBlue = new MvxColor(0xFF5F9EA0);
        public static MvxColor Chartreuse = new MvxColor(0xFF7FFF00);
        public static MvxColor Chocolate = new MvxColor(0xFFD2691E);
        public static MvxColor Coral = new MvxColor(0xFFFF7F50);
        public static MvxColor CornflowerBlue = new MvxColor(0xFF6495ED);
        public static MvxColor Cornsilk = new MvxColor(0xFFFFF8DC);
        public static MvxColor Crimson = new MvxColor(0xFFDC143C);
        public static MvxColor Cyan = new MvxColor(0xFF00FFFF);
        public static MvxColor DarkBlue = new MvxColor(0xFF00008B);
        public static MvxColor DarkCyan = new MvxColor(0xFF008B8B);
        public static MvxColor DarkGoldenrod = new MvxColor(0xFFB8860B);
        public static MvxColor DarkGray = new MvxColor(0xFFA9A9A9);
        public static MvxColor DarkGreen = new MvxColor(0xFF006400);
        public static MvxColor DarkKhaki = new MvxColor(0xFFBDB76B);
        public static MvxColor DarkMagenta = new MvxColor(0xFF8B008B);
        public static MvxColor DarkOliveGreen = new MvxColor(0xFF556B2F);
        public static MvxColor DarkOrange = new MvxColor(0xFFFF8C00);
        public static MvxColor DarkOrchid = new MvxColor(0xFF9932CC);
        public static MvxColor DarkRed = new MvxColor(0xFF8B0000);
        public static MvxColor DarkSalmon = new MvxColor(0xFFE9967A);
        public static MvxColor DarkSeaGreen = new MvxColor(0xFF8FBC8B);
        public static MvxColor DarkSlateBlue = new MvxColor(0xFF483D8B);
        public static MvxColor DarkSlateGray = new MvxColor(0xFF2F4F4F);
        public static MvxColor DarkTurquoise = new MvxColor(0xFF00CED1);
        public static MvxColor DarkViolet = new MvxColor(0xFF9400D3);
        public static MvxColor DeepPink = new MvxColor(0xFFFF1493);
        public static MvxColor DeepSkyBlue = new MvxColor(0xFF00BFFF);
        public static MvxColor DimGray = new MvxColor(0xFF696969);
        public static MvxColor DodgerBlue = new MvxColor(0xFF1E90FF);
        public static MvxColor Firebrick = new MvxColor(0xFFB22222);
        public static MvxColor FloralWhite = new MvxColor(0xFFFFFAF0);
        public static MvxColor ForestGreen = new MvxColor(0xFF228B22);
        public static MvxColor Fuchsia = new MvxColor(0xFFFF00FF);
        public static MvxColor Gainsboro = new MvxColor(0xFFDCDCDC);
        public static MvxColor GhostWhite = new MvxColor(0xFFF8F8FF);
        public static MvxColor Gold = new MvxColor(0xFFFFD700);
        public static MvxColor Goldenrod = new MvxColor(0xFFDAA520);
        public static MvxColor Gray = new MvxColor(0xFF808080);
        public static MvxColor Green = new MvxColor(0xFF008000);
        public static MvxColor GreenYellow = new MvxColor(0xFFADFF2F);
        public static MvxColor Honeydew = new MvxColor(0xFFF0FFF0);
        public static MvxColor HotPink = new MvxColor(0xFFFF69B4);
        public static MvxColor IndianRed = new MvxColor(0xFFCD5C5C);
        public static MvxColor Indigo = new MvxColor(0xFF4B0082);
        public static MvxColor Ivory = new MvxColor(0xFFFFFFF0);
        public static MvxColor Khaki = new MvxColor(0xFFF0E68C);
        public static MvxColor Lavender = new MvxColor(0xFFE6E6FA);
        public static MvxColor LavenderBlush = new MvxColor(0xFFFFF0F5);
        public static MvxColor LawnGreen = new MvxColor(0xFF7CFC00);
        public static MvxColor LemonChiffon = new MvxColor(0xFFFFFACD);
        public static MvxColor LightBlue = new MvxColor(0xFFADD8E6);
        public static MvxColor LightCoral = new MvxColor(0xFFF08080);
        public static MvxColor LightCyan = new MvxColor(0xFFE0FFFF);
        public static MvxColor LightGoldenrodYellow = new MvxColor(0xFFFAFAD2);
        public static MvxColor LightGray = new MvxColor(0xFFD3D3D3);
        public static MvxColor LightGreen = new MvxColor(0xFF90EE90);
        public static MvxColor LightPink = new MvxColor(0xFFFFB6C1);
        public static MvxColor LightSalmon = new MvxColor(0xFFFFA07A);
        public static MvxColor LightSeaGreen = new MvxColor(0xFF20B2AA);
        public static MvxColor LightSkyBlue = new MvxColor(0xFF87CEFA);
        public static MvxColor LightSlateGray = new MvxColor(0xFF778899);
        public static MvxColor LightSteelBlue = new MvxColor(0xFFB0C4DE);
        public static MvxColor LightYellow = new MvxColor(0xFFFFFFE0);
        public static MvxColor Lime = new MvxColor(0xFF00FF00);
        public static MvxColor LimeGreen = new MvxColor(0xFF32CD32);
        public static MvxColor Linen = new MvxColor(0xFFFAF0E6);
        public static MvxColor Magenta = new MvxColor(0xFFFF00FF);
        public static MvxColor Maroon = new MvxColor(0xFF800000);
        public static MvxColor MediumAquamarine = new MvxColor(0xFF66CDAA);
        public static MvxColor MediumBlue = new MvxColor(0xFF0000CD);
        public static MvxColor MediumOrchid = new MvxColor(0xFFBA55D3);
        public static MvxColor MediumPurple = new MvxColor(0xFF9370DB);
        public static MvxColor MediumSeaGreen = new MvxColor(0xFF3CB371);
        public static MvxColor MediumSlateBlue = new MvxColor(0xFF7B68EE);
        public static MvxColor MediumSpringGreen = new MvxColor(0xFF00FA9A);
        public static MvxColor MediumTurquoise = new MvxColor(0xFF48D1CC);
        public static MvxColor MediumVioletRed = new MvxColor(0xFFC71585);
        public static MvxColor MidnightBlue = new MvxColor(0xFF191970);
        public static MvxColor MintCream = new MvxColor(0xFFF5FFFA);
        public static MvxColor MistyRose = new MvxColor(0xFFFFE4E1);
        public static MvxColor Moccasin = new MvxColor(0xFFFFE4B5);
        public static MvxColor NavajoWhite = new MvxColor(0xFFFFDEAD);
        public static MvxColor Navy = new MvxColor(0xFF000080);
        public static MvxColor OldLace = new MvxColor(0xFFFDF5E6);
        public static MvxColor Olive = new MvxColor(0xFF808000);
        public static MvxColor OliveDrab = new MvxColor(0xFF6B8E23);
        public static MvxColor Orange = new MvxColor(0xFFFFA500);
        public static MvxColor OrangeRed = new MvxColor(0xFFFF4500);
        public static MvxColor Orchid = new MvxColor(0xFFDA70D6);
        public static MvxColor PaleGoldenrod = new MvxColor(0xFFEEE8AA);
        public static MvxColor PaleGreen = new MvxColor(0xFF98FB98);
        public static MvxColor PaleTurquoise = new MvxColor(0xFFAFEEEE);
        public static MvxColor PaleVioletRed = new MvxColor(0xFFDB7093);
        public static MvxColor PapayaWhip = new MvxColor(0xFFFFEFD5);
        public static MvxColor PeachPuff = new MvxColor(0xFFFFDAB9);
        public static MvxColor Peru = new MvxColor(0xFFCD853F);
        public static MvxColor Pink = new MvxColor(0xFFFFC0CB);
        public static MvxColor Plum = new MvxColor(0xFFDDA0DD);
        public static MvxColor PowderBlue = new MvxColor(0xFFB0E0E6);
        public static MvxColor Purple = new MvxColor(0xFF800080);
        public static MvxColor Red = new MvxColor(0xFFFF0000);
        public static MvxColor RosyBrown = new MvxColor(0xFFBC8F8F);
        public static MvxColor RoyalBlue = new MvxColor(0xFF4169E1);
        public static MvxColor SaddleBrown = new MvxColor(0xFF8B4513);
        public static MvxColor Salmon = new MvxColor(0xFFFA8072);
        public static MvxColor SandyBrown = new MvxColor(0xFFF4A460);
        public static MvxColor SeaGreen = new MvxColor(0xFF2E8B57);
        public static MvxColor SeaShell = new MvxColor(0xFFFFF5EE);
        public static MvxColor Sienna = new MvxColor(0xFFA0522D);
        public static MvxColor Silver = new MvxColor(0xFFC0C0C0);
        public static MvxColor SkyBlue = new MvxColor(0xFF87CEEB);
        public static MvxColor SlateBlue = new MvxColor(0xFF6A5ACD);
        public static MvxColor SlateGray = new MvxColor(0xFF708090);
        public static MvxColor Snow = new MvxColor(0xFFFFFAFA);
        public static MvxColor SpringGreen = new MvxColor(0xFF00FF7F);
        public static MvxColor SteelBlue = new MvxColor(0xFF4682B4);
        public static MvxColor Tan = new MvxColor(0xFFD2B48C);
        public static MvxColor Teal = new MvxColor(0xFF008080);
        public static MvxColor Thistle = new MvxColor(0xFFD8BFD8);
        public static MvxColor Tomato = new MvxColor(0xFFFF6347);
        public static MvxColor Turquoise = new MvxColor(0xFF40E0D0);
        public static MvxColor Violet = new MvxColor(0xFFEE82EE);
        public static MvxColor Wheat = new MvxColor(0xFFF5DEB3);
        public static MvxColor White = new MvxColor(0xFFFFFFFF);
        public static MvxColor WhiteSmoke = new MvxColor(0xFFF5F5F5);
        public static MvxColor Yellow = new MvxColor(0xFFFFFF00);
        public static MvxColor YellowGreen = new MvxColor(0xFF9ACD32);
    }
}