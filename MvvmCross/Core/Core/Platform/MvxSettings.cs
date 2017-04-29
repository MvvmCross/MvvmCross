﻿// MvxSettings.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Platform
{
    public class MvxSettings : IMvxSettings
    {
        public MvxSettings()
        {
            AlwaysRaiseInpcOnUserInterfaceThread = true;
        }

        public bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }
    }
}