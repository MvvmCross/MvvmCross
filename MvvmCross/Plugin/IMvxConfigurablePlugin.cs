// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin
{
#nullable enable
    public interface IMvxConfigurablePlugin : IMvxPlugin
    {
        void Configure(IMvxPluginConfiguration configuration);
    }
#nullable restore
}
