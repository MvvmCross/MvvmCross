// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("MvvmCross.Forms.Platforms.Wpf")]

[assembly: Xamarin.Forms.Internals.Preserve(AllMembers = true)]

#if NETSTANDARD
[assembly: XmlnsPrefix("http://mvvmcross.com/bind", "mvx")]
[assembly: XmlnsDefinition("http://mvvmcross.com/bind", nameof(MvvmCross.Forms.Bindings))]
[assembly: XmlnsDefinition("http://mvvmcross.com/bind", nameof(MvvmCross.Forms.Converters))]
[assembly: XmlnsDefinition("http://mvvmcross.com/bind", nameof(MvvmCross.Forms.Views))]
#endif
