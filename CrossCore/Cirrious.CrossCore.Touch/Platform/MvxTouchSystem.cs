// MvxTouchSystem.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Linq;
using MonoTouch.UIKit;

namespace Cirrious.CrossCore.Touch.Platform
{
    public class MvxTouchSystem
        : IMvxTouchSystem
    {
        public MvxTouchVersion Version { get; private set; }

        public MvxTouchSystem()
        {
            BuildVersion();
        }

        private void BuildVersion()
        {
            var version = UIDevice.CurrentDevice.SystemVersion;
            var parts = version.Split('.').Select(x => int.Parse(x)).ToArray();
            Version = new MvxTouchVersion(parts);
        }
    }
}