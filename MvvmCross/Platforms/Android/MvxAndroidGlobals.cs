// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Android.App;
using Android.Content;

namespace MvvmCross.Platforms.Android;

/// <summary>
/// Default implementation of <see cref="IMvxAndroidGlobals"/> that wraps the Android
/// <see cref="Application"/> instance to provide context and assembly information.
/// </summary>
internal sealed class MvxAndroidGlobals : IMvxAndroidGlobals
{
    private readonly Application _application;

    public MvxAndroidGlobals(Application application)
    {
        _application = application;
    }

    public Assembly ExecutableAssembly => _application.GetType().Assembly;

    public Context ApplicationContext => _application.ApplicationContext!;
}
