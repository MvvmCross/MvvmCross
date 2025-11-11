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

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        void InitializeSecondary();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Assembly> GetViewAssemblies();
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Assembly> GetViewModelAssemblies();
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Assembly> GetPluginAssemblies();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Type> CreatableTypes();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IEnumerable<Type> CreatableTypes(Assembly assembly);

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        void LoadPlugins(IMvxPluginManager pluginManager);

        event EventHandler<MvxSetupStateEventArgs>? StateChanged;
        MvxSetupState State { get; }
    }
#nullable restore
}
