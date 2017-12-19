﻿// MvxSimpleTableViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Foundation;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.tvOS.Platform;
using UIKit;

namespace MvvmCross.Binding.tvOS.Views
{
    public class MvxSimpleTableViewSource : MvxTableViewSource
    {
        private readonly NSString _cellIdentifier;
        private readonly MvxTvosMajorVersionChecker _iosVersion6Checker = new MvxTvosMajorVersionChecker(6);

        protected virtual NSString CellIdentifier => _cellIdentifier;

        public MvxSimpleTableViewSource(IntPtr handle)
            : base(handle)
        {
            MvxLog.Instance.Warn("MvxSimpleTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
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
