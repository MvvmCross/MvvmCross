// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Reflection;
using System.Windows.Input;

namespace MvvmCross.WeakSubscription;

public class MvxCanExecuteChangedEventSubscription(
        ICommand source,
        EventHandler<EventArgs> eventHandler)
    : MvxWeakEventSubscription<ICommand, EventArgs>(source, CanExecuteChangedEventInfo, eventHandler)
{
    private static readonly EventInfo CanExecuteChangedEventInfo = typeof(ICommand).GetEvent("CanExecuteChanged")!;

    protected override Delegate CreateEventHandler() => new EventHandler(OnSourceEvent);
}
