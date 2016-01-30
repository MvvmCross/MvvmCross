// MvxIosSystem.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.iOS.Platform
{
    using System.Linq;

    using UIKit;

    public class MvxIosSystem
        : IMvxIosSystem
    {
        public MvxIosVersion Version { get; private set; }

        public MvxIosSystem()
        {
            this.BuildVersion();
        }

        private void BuildVersion()
        {
            var version = UIDevice.CurrentDevice.SystemVersion;
            var parts = version.Split('.').Select(int.Parse).ToArray();
            this.Version = new MvxIosVersion(parts);
        }
    }
}