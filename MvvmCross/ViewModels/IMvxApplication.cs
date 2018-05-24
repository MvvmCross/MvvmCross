// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Plugin;

namespace MvvmCross.ViewModels
{
    public interface IMvxApplication : IMvxViewModelLocatorCollection
    {
        void LoadPlugins(IMvxPluginManager pluginManager);

        void Initialize();

        void Startup();

        void Reset();
    }

    public interface IMvxApplication<THint> : IMvxApplication
    {
        THint Startup(THint hint);
    }
}
