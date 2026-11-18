// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Hosting;

namespace MvvmCross.Platforms.Tvos
{
    public class MvxTvosMajorVersionChecker
    {
        public bool IsVersionOrHigher { get; }

        public MvxTvosMajorVersionChecker(int major, bool defaultValue = true)
        {
            IsVersionOrHigher = ReadIsTvosVersionOrHigher(major, defaultValue);
        }

        private static bool ReadIsTvosVersionOrHigher(int target, bool defaultValue)
        {
            var tvosSystem = MvxHost.Current?.Services.GetService<IMvxTvosSystem>();
            if (tvosSystem == null)
            {
                return defaultValue;
            }

            return tvosSystem.Version.Major >= target;
        }
    }
}
