// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.ComponentModel;
using System.Reflection;

namespace MvvmCross.WeakSubscription;

public class MvxNotifyPropertyChangedEventSubscription(
        INotifyPropertyChanged source,
        EventHandler<PropertyChangedEventArgs> targetEventHandler)
    : MvxWeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>(
        source, PropertyChangedEventInfo, targetEventHandler)
{
    private static readonly EventInfo PropertyChangedEventInfo =
        typeof(INotifyPropertyChanged).GetEvent("PropertyChanged")!;

    // This code ensures the PropertyChanged event is not stripped by Xamarin linker
    // see https://github.com/MvvmCross/MvvmCross/pull/453
    public static void LinkerPleaseInclude(INotifyPropertyChanged iNotifyPropertyChanged)
    {
        iNotifyPropertyChanged.PropertyChanged += (sender, e) => { };
    }

    protected override Delegate CreateEventHandler()
    {
        return new PropertyChangedEventHandler(OnSourceEvent);
    }
}
