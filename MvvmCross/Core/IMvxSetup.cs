// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Plugin;
using MvvmCross.Logging;
using System;
using static MvvmCross.Core.MvxSetup;

namespace MvvmCross.Core
{
    public interface IMvxSetup
    {
        void InitializePrimary();
        void InitializeSecondary();

        IEnumerable<Assembly> GetViewAssemblies();
        IEnumerable<Assembly> GetViewModelAssemblies();
        IEnumerable<Assembly> GetPluginAssemblies();

        IEnumerable<Type> CreatableTypes();
        IEnumerable<Type> CreatableTypes(Assembly assembly);

        void LoadPlugins(IMvxPluginManager pluginManager);

        MvxLogProviderType GetDefaultLogProviderType();

        event EventHandler<MvxSetupStateEventArgs> StateChanged;
        MvxSetupState State { get; }
    }
}
