// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using MvvmCross.Logging;

[assembly: InternalsVisibleTo("MvvmCross.Forms.Platforms.Wpf")]
namespace MvvmCross.Forms
{
    internal static class MvxFormsLog
    {
        internal static IMvxLog Instance { get; } = Mvx.Resolve<IMvxLogProvider>().GetLogFor("MvxForms");
    }
}
