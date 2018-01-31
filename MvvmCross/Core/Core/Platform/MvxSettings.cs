// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Core.Platform
{
    public class MvxSettings : IMvxSettings
    {
        public bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }

        public MvxSettings()
        {
            AlwaysRaiseInpcOnUserInterfaceThread = true;
        }
    }
}