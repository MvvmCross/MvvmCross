// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Base.Logging;

namespace MvvmCross.Platform.Tvos.Base.Platform
{
    public class MvxTvosMajorVersionChecker
    {
        public bool IsVersionOrHigher { get; private set; }

        public MvxTvosMajorVersionChecker(int major, bool defaultValue = true)
        {
            IsVersionOrHigher = ReadIsTvosVersionOrHigher(major, defaultValue);
        }

        private static bool ReadIsTvosVersionOrHigher(int target, bool defaultValue)
        {
            IMvxTvosSystem touchSystem;
            Mvx.TryResolve<IMvxTvosSystem>(out touchSystem);
            if (touchSystem == null)
            {
                MvxLog.Instance.Warn("IMvxTvosSystem not found - so assuming we {1} on tvOS {0} or later", target, defaultValue ? "are" : "are not");
                return defaultValue;
            }

            return touchSystem.Version.Major >= target;
        }
    }
}
