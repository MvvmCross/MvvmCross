// MvxTvosSystem.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Linq;
using UIKit;

namespace MvvmCross.Platform.tvOS.Platform
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