// MvxSettings.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Platform
{
    public class MvxSettings : IMvxSettings
    {
        public bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }

        public MvxSettings()
        {
            this.AlwaysRaiseInpcOnUserInterfaceThread = true;
        }
    }
}