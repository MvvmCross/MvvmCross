// MvxTouchVersion.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;

namespace Cirrious.CrossCore.Touch.Platform
{
    public class MvxTouchVersion
    {
        public MvxTouchVersion(int[] parts)
        {
            if (parts == null || parts.Length == 0)
                throw new MvxException("Invalid parts in constructor for MvxTouchVersion");

            Parts = parts;
            Major = parts[0];

            if (parts.Length > 1)
            {
                Minor = parts[1];
            }
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int[] Parts { get; private set; }
    }
}