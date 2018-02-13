// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Platform.Uap.Views;
using MvvmCross.ViewModels;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace MvvmCross.Platform.Uap.Core
{
    public class MvxTypedWindowsSetup<TApplication>: MvxWindowsSetup
         where TApplication : IMvxApplication, new()
    {
        protected readonly Assembly viewAssembly;
        public MvxTypedWindowsSetup(Frame rootFrame, string suspensionManagerSessionStateKey = null) : base(rootFrame, suspensionManagerSessionStateKey)
        {
            viewAssembly = Assembly.GetCallingAssembly();
        }

        public MvxTypedWindowsSetup(IMvxWindowsFrame rootFrame) : base(rootFrame)
        {
            viewAssembly = Assembly.GetCallingAssembly();
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            return base.GetViewAssemblies().Union(new[] { viewAssembly });
        }

        protected override IMvxApplication CreateApp()
        {
            return new TApplication();
        }
    }
}
