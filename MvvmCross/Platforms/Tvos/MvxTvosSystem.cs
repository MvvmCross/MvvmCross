// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using UIKit;

namespace MvvmCross.Platforms.Tvos
{
    public class MvxTvosSystem
        : IMvxTvosSystem
    {
        public MvxTvosVersion Version { get; private set; }

        public MvxTvosSystem()
        {
            BuildVersion();
        }

        private void BuildVersion()
        {
            var version = UIDevice.CurrentDevice.SystemVersion;
            var parts = version.Split('.').Select(int.Parse).ToArray();
            Version = new MvxTvosVersion(parts);
        }
    }
}
