// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Hosting;
using MvvmCross.Platforms.WinUi;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility.Platforms.WinUi
{
    public class MvxVisibilityDesignTimeHelper
        : MvxDesignTimeHelper
    {
        public MvxVisibilityDesignTimeHelper()
        {
            // Design-time plugin loading is no longer needed.
            // Register IMvxNativeVisibility at startup with services.AddMvxVisibility().
        }
    }
}
