#region Copyright
// <copyright file="MvxSimpleSelectionChangedEventArgs.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Commands
{
    public class MvxSimpleSelectionChangedEventArgs : EventArgs
    {
        public IList AddedItems { get; set; }
        public IList RemovedItems { get; set; }

        public static MvxSimpleSelectionChangedEventArgs JustAddOneItem<T>(T item)
        {
            return new MvxSimpleSelectionChangedEventArgs() {AddedItems = new List<T>() {item}};
        }
    }
}