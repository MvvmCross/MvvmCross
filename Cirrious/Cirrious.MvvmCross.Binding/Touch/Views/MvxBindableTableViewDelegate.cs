#region Copyright
// <copyright file="MvxBindableTableViewDelegate.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Commands;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBindableTableViewDelegate : UITableViewDelegate
    {
        public IList ItemsSource { get; set; }
        public event EventHandler<MvxSimpleSelectionChangedEventArgs> SelectionChanged;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (ItemsSource == null)
                return;

            var item = ItemsSource[indexPath.Row];
            var selectionChangedArgs = MvxSimpleSelectionChangedEventArgs.JustAddOneItem(item);

            var handler = SelectionChanged;
            if (handler != null)
                handler(this, selectionChangedArgs);
        }
    }
}