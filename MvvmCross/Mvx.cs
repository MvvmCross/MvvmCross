// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Base;
using MvvmCross.IoC;

namespace MvvmCross
{
    public static class Mvx
    {
        /// <summary>
        /// Returns a singleton instance of the default IoC Provider. If possible use dependency injection instead.
        /// </summary>
        public static IMvxIoCProvider IoCProvider => MvxSingleton<IMvxIoCProvider>.Instance;
    }
}
