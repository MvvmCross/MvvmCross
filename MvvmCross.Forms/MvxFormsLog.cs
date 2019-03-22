// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using MvvmCross.Logging;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("MvvmCross.Forms.Platforms.Wpf")]
[assembly: XmlnsDefinition("http://mvvmcross.com/bind", nameof(MvvmCross.Forms.Bindings))]
[assembly: XmlnsDefinition("http://mvvmcross.com/bind", nameof(MvvmCross.Forms.Converters))]
[assembly: XmlnsDefinition("http://mvvmcross.com/bind", nameof(MvvmCross.Forms.Views))]
namespace MvvmCross.Forms
{
    internal static class MvxFormsLog
    {
        internal static IMvxLog Instance { get; } = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor("MvxForms");
    }
}
