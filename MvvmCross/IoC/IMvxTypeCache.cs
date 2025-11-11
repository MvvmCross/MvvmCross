// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MvvmCross.IoC;

public interface IMvxTypeCache
{
    Dictionary<string, Type> LowerCaseFullNameCache { get; }
    Dictionary<string, Type> FullNameCache { get; }
    Dictionary<string, Type> NameCache { get; }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved in trimming scenarios")]
    void AddAssembly(Assembly assembly);
}
