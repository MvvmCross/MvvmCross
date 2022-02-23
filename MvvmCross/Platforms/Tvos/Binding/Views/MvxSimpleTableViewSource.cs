// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Foundation;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Views
{
    public class MvxSimpleTableViewSource : MvxTableViewSource
    {
        private readonly NSString _cellIdentifier;
        private readonly MvxTvosMajorVersionChecker _iosVersion6Checker = new MvxTvosMajorVersionChecker(6);

        protected virtual NSString CellIdentifier => _cellIdentifier;

        public MvxSimpleTableViewSource(IntPtr handle)
            : base(handle)
        {
            MvxLogHost.GetLog<MvxBaseTableViewSource>()?.Log(LogLevel.Warning,
                "MvxSimpleTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        public MvxSimpleTableViewSource(UITableView tableView, string nibName, string cellIdentifier = null,
                                        NSBundle bundle = null, bool registerNibForCellReuse = true)
            : base(tableView)
        {
            // if no cellIdentifier supplied, then use the nibName as cellId
            cellIdentifier = cellIdentifier ?? nibName;
            _cellIdentifier = new NSString(cellIdentifier);

            if (registerNibForCellReuse)
            {
                tableView.RegisterNibForCellReuse(UINib.FromName(nibName, bundle ?? NSBundle.MainBundle), cellIdentifier);
            }
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
