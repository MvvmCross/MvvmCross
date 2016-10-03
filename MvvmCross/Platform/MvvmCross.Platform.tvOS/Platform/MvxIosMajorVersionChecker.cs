// MvxTvosMajorVersionChecker.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.tvOS.Platform
{
    public class MvxTvosMajorVersionChecker
    {
        public bool IsVersionOrHigher { get; private set; }

        public MvxTvosMajorVersionChecker(int major, bool defaultValue = true)
        {
            this.IsVersionOrHigher = ReadIsTvosVersionOrHigher(major, defaultValue);
        }

        private static bool ReadIsTvosVersionOrHigher(int target, bool defaultValue)
        {
            IMvxTvosSystem touchSystem;
            Mvx.TryResolve<IMvxTvosSystem>(out touchSystem);
            if (touchSystem == null)
            {
                Mvx.Warning("IMvxTvosSystem not found - so assuming we {1} on tvOS {0} or later", target, defaultValue ? "are" : "are not");
                return defaultValue;
            }

            return touchSystem.Version.Major >= target;
        }
    }
}