// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Android.Content;

namespace MvvmCross.Platform.Android.Base
{
    public interface IMvxAndroidGlobals
    {
        string ExecutableNamespace { get; }
        Assembly ExecutableAssembly { get; }
        Context ApplicationContext { get; }
    }
}
