#region Copyright
// <copyright file="MvxNotifyCollectionChangedEventHandler.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

// External credit:
// This file relies heavily on the Mono project - used under MIT license:
// https://github.com/mono/mono

namespace Cirrious.MvvmCross.ViewModels
{
    public delegate void MvxNotifyCollectionChangedEventHandler(object sender, MvxNotifyCollectionChangedEventArgs e);
}