// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platforms.Uap;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color.Platforms.Uap
{
    public class MvxColorDesignTimeHelper
        : MvxDesignTimeHelper
    {
        public MvxColorDesignTimeHelper()
        {
            // Design-time plugin loading is no longer needed.
            // Register IMvxNativeColor at startup with services.AddMvxColor().
        }
    }
}
