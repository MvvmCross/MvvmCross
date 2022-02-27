// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using System.Windows.Threading;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Presenters;

namespace MvvmCross.Platforms.Wpf.Core
{
    public interface IMvxWpfSetup : IMvxSetup
    {
        void PlatformInitialize(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter);
        void PlatformInitialize(Dispatcher uiThreadDispatcher, ContentControl root);
    }
}
