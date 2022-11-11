// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Platforms.Ios
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
            IMvxIosSystem iosSystem;
            Mvx.IoCProvider.TryResolve<IMvxIosSystem>(out iosSystem);
            if (iosSystem == null)
            {
                MvxLogHost.Default?.LogWarning(
                    "IMvxIosSystem not found - so assuming we {target} on iOS {default} or later", target, defaultValue ? "are" : "are not");
                return defaultValue;
            }

            return iosSystem.Version.Major >= target;
        }
    }
}
