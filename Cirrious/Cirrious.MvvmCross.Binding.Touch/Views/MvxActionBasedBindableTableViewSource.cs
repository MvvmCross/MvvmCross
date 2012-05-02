#region Copyright
// <copyright file="MvxActionBasedBindableTableViewSource.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxActionBasedBindableTableViewSource : MvxBindableTableViewSource
    {
        static readonly NSString CellIdentifier = new NSString("MvxDefaultBindableTableViewCell");


        protected MvxActionBasedBindableTableViewSource(UITableView tableView)
            : base(tableView)
        {
            Initialise();
        }

        public MvxActionBasedBindableTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, string bindingText, UITableViewCellAccessory tableViewCellAccessory)
            : base(tableView, style, cellIdentifier, bindingText, tableViewCellAccessory)
        {
            Initialise();
        }

        public MvxActionBasedBindableTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, IEnumerable<MvxBindingDescription> descriptions, UITableViewCellAccessory tableViewCellAccessory)
            : base(tableView, style, cellIdentifier, descriptions, tableViewCellAccessory)
        {
            Initialise();
        }

        private void Initialise()
        {
            CellCreator = CreateDefaultBindableCell;
            CellModifier = (ignored) => { };
        }

        public Func<UITableView, NSIndexPath, object, MvxBindableTableViewCell> CellCreator { get; set; }
        public Action<MvxBindableTableViewCell> CellModifier { get; set; }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(CellIdentifier);
            if (reuse != null)
                return reuse;

            var cell = CellCreator(tableView, indexPath, item);
            if (CellModifier != null)
                CellModifier(cell);
            return cell;
        }

    }
}