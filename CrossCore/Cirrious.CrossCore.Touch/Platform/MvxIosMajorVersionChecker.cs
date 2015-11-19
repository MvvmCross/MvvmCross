// MvxIosMajorVersionChecker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.Touch.Platform
{
    public class MvxIosMajorVersionChecker
    {
        public bool IsVersionOrHigher { get; private set; }

        public MvxIosMajorVersionChecker(int major, bool defaultValue = true)
        {
            IsVersionOrHigher = ReadIsIosVersionOrHigher(major, defaultValue);
        }

        private static bool ReadIsIosVersionOrHigher(int target, bool defaultValue)
        {
            IMvxTouchSystem touchSystem;
            Mvx.TryResolve<IMvxTouchSystem>(out touchSystem);
            if (touchSystem == null)
            {
                Mvx.Warning("IMvxTouchSystem not found - so assuming we {1} on iOS {0} or later", target, defaultValue ? "are" : "are not");
                return defaultValue;
            }

            return touchSystem.Version.Major >= target;
        }
    }
}