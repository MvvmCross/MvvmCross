// MvxIosVersion.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Platform.iOS.Platform
{
    public class MvxIosVersion
    {
        public MvxIosVersion(int[] parts)
        {
            if (parts == null || parts.Length == 0)
                throw new MvxException("Invalid parts in constructor for MvxIosVersion");

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