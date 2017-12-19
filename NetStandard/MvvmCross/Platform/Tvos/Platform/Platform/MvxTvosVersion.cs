// MvxTvosVersion.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Platform.tvOS.Platform
{
    public class MvxTvosVersion
    {
        public MvxTvosVersion(int[] parts)
        {
            if (parts == null || parts.Length == 0)
                throw new MvxException("Invalid parts in constructor for MvxTvosVersion");

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