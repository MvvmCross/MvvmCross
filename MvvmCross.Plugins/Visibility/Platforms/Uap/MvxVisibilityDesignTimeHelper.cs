// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platforms.Uap;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility.Platforms.Uap
{
    public class MvxVisibilityDesignTimeHelper
        : MvxDesignTimeHelper
    {
        public MvxVisibilityDesignTimeHelper()
        {
            if (!IsInDesignTool)
                return;

            if (Mvx.IoCProvider.CanResolve<IMvxNativeVisibility>())
                return;

            var forceVisibilityLoaded = new Plugin();
            forceVisibilityLoaded.Load();
        }
    }
}
