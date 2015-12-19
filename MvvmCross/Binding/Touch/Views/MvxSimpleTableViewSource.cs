// MvxSimpleTableViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Views
{
    using System;

    using Foundation;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Touch.Platform;

    using UIKit;

    public class MvxSimpleTableViewSource : MvxTableViewSource
    {
        private readonly NSString _cellIdentifier;
        private readonly MvxIosMajorVersionChecker _iosVersion6Checker = new MvxIosMajorVersionChecker(6);

        protected virtual NSString CellIdentifier => this._cellIdentifier;

        public MvxSimpleTableViewSource(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("MvxSimpleTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        public MvxSimpleTableViewSource(UITableView tableView, string nibName, string cellIdentifier = null,
                                        NSBundle bundle = null)
            : base(tableView)
        {
            // if no cellIdentifier supplied, then use the nibName as cellId
            cellIdentifier = cellIdentifier ?? nibName;
            this._cellIdentifier = new NSString(cellIdentifier);
            tableView.RegisterNibForCellReuse(UINib.FromName(nibName, bundle ?? NSBundle.MainBundle), cellIdentifier);
        }

        public MvxSimpleTableViewSource(UITableView tableView, Type cellType, string cellIdentifier = null)
            : base(tableView)
        {
            // if no cellIdentifier supplied, then use the cell type name as cellId
            cellIdentifier = cellIdentifier ?? cellType.Name;
            this._cellIdentifier = new NSString(cellIdentifier);
            tableView.RegisterClassForCellReuse(cellType, this._cellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (this._iosVersion6Checker.IsVersionOrHigher)
                return tableView.DequeueReusableCell(this.CellIdentifier, indexPath);

            return tableView.DequeueReusableCell(this.CellIdentifier);
        }
    }
}