// Forwarding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof (System.Collections.ObjectModel.ObservableCollection<>))]
[assembly: TypeForwardedTo(typeof (System.Collections.ObjectModel.ReadOnlyObservableCollection<>))]
[assembly: TypeForwardedTo(typeof (System.Collections.Specialized.INotifyCollectionChanged))]
[assembly: TypeForwardedTo(typeof (System.Collections.Specialized.NotifyCollectionChangedAction))]
[assembly: TypeForwardedTo(typeof (System.Collections.Specialized.NotifyCollectionChangedEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Collections.Specialized.NotifyCollectionChangedEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Windows.Input.ICommand))]
