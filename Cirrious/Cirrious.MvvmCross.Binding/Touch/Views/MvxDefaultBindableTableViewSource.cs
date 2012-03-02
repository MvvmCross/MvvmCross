#region Copyright
// <copyright file="MvxDefaultBindableTableViewSource.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxDefaultBindableTableViewSource : MvxBindableTableViewSource
    {
        static readonly NSString CellIdentifier = new NSString("MvxDefaultBindableTableViewCell");
        
        public MvxDefaultBindableTableViewSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(CellIdentifier);
            if (reuse != null)
                return reuse;

            return new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
        }
    }
}