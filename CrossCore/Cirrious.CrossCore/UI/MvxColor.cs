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
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

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
    }
}