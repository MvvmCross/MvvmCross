#region Copyright

// <copyright file="Forwarding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof (System.Collections.ObjectModel.ObservableCollection<>))]
[assembly: TypeForwardedTo(typeof (System.Collections.ObjectModel.ReadOnlyObservableCollection<>))]
[assembly: TypeForwardedTo(typeof (System.Collections.Specialized.INotifyCollectionChanged))]
[assembly: TypeForwardedTo(typeof (System.Collections.Specialized.NotifyCollectionChangedAction))]
[assembly: TypeForwardedTo(typeof (System.Collections.Specialized.NotifyCollectionChangedEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Collections.Specialized.NotifyCollectionChangedEventHandler))]
[assembly: TypeForwardedTo(typeof (System.Windows.Input.ICommand))]