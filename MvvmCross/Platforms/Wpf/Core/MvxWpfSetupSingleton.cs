// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using System.Windows.Threading;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Presenters;

namespace MvvmCross.Platforms.Wpf.Core
{
    public class MvxWpfSetupSingleton
        : MvxSetupSingleton
    {
        public static MvxWpfSetupSingleton EnsureSingletonAvailable(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
        {
            var instance = EnsureSingletonAvailable<MvxWpfSetupSingleton>();
            instance.PlatformSetup<MvxWpfSetup>()?.PlatformInitialize(uiThreadDispatcher, presenter);
            return instance;
        }

        public static MvxWpfSetupSingleton EnsureSingletonAvailable(Dispatcher uiThreadDispatcher, ContentControl root)
        {
            var instance = EnsureSingletonAvailable<MvxWpfSetupSingleton>();
            instance.PlatformSetup<MvxWpfSetup>()?.PlatformInitialize(uiThreadDispatcher, root);
            return instance;
        }
    }
}
