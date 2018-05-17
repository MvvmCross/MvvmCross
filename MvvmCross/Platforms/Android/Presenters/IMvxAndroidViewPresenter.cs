// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Presenters;

namespace MvvmCross.Platforms.Android.Presenters
{
    public interface IMvxAndroidViewPresenter
        : IMvxViewPresenter
    {
        IEnumerable<Assembly> AndroidViewAssemblies { get; set; }
    }
}
