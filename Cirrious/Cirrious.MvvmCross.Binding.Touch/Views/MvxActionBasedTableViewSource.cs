// MvxActionBasedTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Bindings;
using Foundation;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxActionBasedTableViewSource : MvxStandardTableViewSource
    {
        protected MvxActionBasedTableViewSource(UITableView tableView)
            : base(tableView)
        {
            Initialize();
        }

        public MvxActionBasedTableViewSource(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("MvxActionBasedTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
            Initialize();
        }

        public MvxActionBasedTableViewSource(UITableView tableView,
                                             UITableViewCellStyle style,
                                             NSString cellIdentifier,
                                             string bindingText,
                                             UITableViewCellAccessory tableViewCellAccessory)
            : base(tableView, style, cellIdentifier, bindingText, tableViewCellAccessory)
        {
            Initialize();
        }

        public MvxActionBasedTableViewSource(UITableView tableView,
                                             UITableViewCellStyle style,
                                             NSString cellIdentifier,
                                             IEnumerable<MvxBindingDescription> descriptions,
                                             UITableViewCellAccessory tableViewCellAccessory)
            : base(tableView, style, cellIdentifier, descriptions, tableViewCellAccessory)
        {
            Initialize();
        }

        private void Initialize()
        {
            CellCreator = CreateDefaultBindableCell;
            CellModifier = (ignored) => { };
        }

        public Func<UITableView, NSIndexPath, object, MvxStandardTableViewCell> CellCreator { get; set; }
        public Action<MvxStandardTableViewCell> CellModifier { get; set; }
        public Func<NSString> CellIdentifierOverride { get; set; }

        protected override NSString CellIdentifier
        {
            get
            {
                if (CellIdentifierOverride != null)
                    return CellIdentifierOverride();

                return base.CellIdentifier;
            }
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(CellIdentifier);
            if (reuse != null)
                return reuse;

            var cell = CellCreator(tableView, indexPath, item);
            CellModifier?.Invoke(cell);
            return cell;
        }
    }
}