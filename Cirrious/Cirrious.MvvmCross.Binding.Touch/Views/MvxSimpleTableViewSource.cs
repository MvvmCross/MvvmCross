// MvxSimpleTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Touch.Platform;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxSimpleTableViewSource : MvxTableViewSource
    {
        private readonly NSString _cellIdentifier;
        private readonly MvxIosMajorVersionChecker _iosVersion6Checker = new MvxIosMajorVersionChecker(6);

        protected virtual NSString CellIdentifier
        {
            get { return _cellIdentifier; }
        }

        public MvxSimpleTableViewSource(IntPtr handle) : base(handle)
        {
        }

        public MvxSimpleTableViewSource(UITableView tableView, string nibName, string cellIdentifier = null,
                                        NSBundle bundle = null)
            : base(tableView)
        {            
    	    // if no cellIdentifier supplied, then use the nibName as cellId
            cellIdentifier = cellIdentifier ?? nibName;
            _cellIdentifier = new NSString(cellIdentifier);
            tableView.RegisterNibForCellReuse(UINib.FromName(nibName, bundle ?? NSBundle.MainBundle), cellIdentifier);
        }

        public MvxSimpleTableViewSource(UITableView tableView, Type cellType, string cellIdentifier = null)
            : base(tableView)
        {
            // if no cellIdentifier supplied, then use the cell type name as cellId
            cellIdentifier = cellIdentifier ?? cellType.Name;
            _cellIdentifier = new NSString(cellIdentifier);
            tableView.RegisterClassForCellReuse(cellType, _cellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (_iosVersion6Checker.IsVersionOrHigher)
                return tableView.DequeueReusableCell(CellIdentifier, indexPath);

            return tableView.DequeueReusableCell(CellIdentifier);
        }
    }
}