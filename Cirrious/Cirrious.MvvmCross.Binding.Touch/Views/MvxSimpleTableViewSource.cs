// MvxSimpleTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Touch.Platform;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxSimpleTableViewSource : MvxTableViewSource
    {
        private static bool? _useIos5Form;

        private static bool UseIos5Form
        {
            get
            {
                if (!_useIos5Form.HasValue)
                {
                    IMvxTouchSystem touchSystem;
                    Mvx.TryResolve<IMvxTouchSystem>(out touchSystem);
                    if (touchSystem == null)
                    {
                        Mvx.Warning("MvxTouchSystem not found - assuming we are on iOS6 or later");
                        _useIos5Form = false;
                    }
                    else
                    {
                        _useIos5Form = touchSystem.Version.Major < 6;
                    }
                }

                return _useIos5Form.Value;
            }
        }

        private readonly NSString _cellIdentifier;

        protected virtual NSString CellIdentifier
        {
            get { return _cellIdentifier; }
        }

        public MvxSimpleTableViewSource(UITableView tableView, string nibName, string cellIdentifier = null,
                                        NSBundle bundle = null)
            : base(tableView)
        {            
            cellIdentifier = cellIdentifier ?? "CellId" + nibName;
            _cellIdentifier = new NSString(cellIdentifier);
            tableView.RegisterNibForCellReuse(UINib.FromName(nibName, bundle ?? NSBundle.MainBundle), cellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (UseIos5Form)
                return tableView.DequeueReusableCell(CellIdentifier);

            return tableView.DequeueReusableCell(CellIdentifier, indexPath);
        }
    }
}