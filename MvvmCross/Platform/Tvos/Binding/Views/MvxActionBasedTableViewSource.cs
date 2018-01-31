// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding.Bindings;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using UIKit;

namespace MvvmCross.Binding.tvOS.Views
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
            MvxLog.Instance.Warn("MvxActionBasedTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
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
