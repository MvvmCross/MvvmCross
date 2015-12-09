// IMvxAndroidGlobals.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid
{
    using System.Reflection;

    using Android.Content;

    public interface IMvxAndroidGlobals
    {
        string ExecutableNamespace { get; }
        Assembly ExecutableAssembly { get; }
        Context ApplicationContext { get; }
    }
}