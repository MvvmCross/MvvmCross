// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Plugin;

namespace MvvmCross.Core
{
#nullable enable
    public interface IMvxSetup
    {
        void InitializePrimary();
        
        [RequiresUnreferencedCode("Gets types from assemblies")]
        void InitializeSecondary();

        IEnumerable<Assembly> GetViewAssemblies();
        IEnumerable<Assembly> GetViewModelAssemblies();

        [RequiresUnreferencedCode("Gets types from assemblies")]
        IEnumerable<Assembly> GetPluginAssemblies();

        IEnumerable<Type> CreatableTypes();
        IEnumerable<Type> CreatableTypes(Assembly assembly);

        [RequiresUnreferencedCode("Gets types from assemblies")]
        void LoadPlugins(IMvxPluginManager pluginManager);

        event EventHandler<MvxSetupStateEventArgs>? StateChanged;
        MvxSetupState State { get; }
    }
#nullable restore
}
