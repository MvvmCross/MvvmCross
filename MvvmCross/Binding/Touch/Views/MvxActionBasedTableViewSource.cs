// MvxActionBasedTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Views
{
    using System;
    using System.Collections.Generic;

    using Foundation;

    using MvvmCross.Binding.Bindings;
    using MvvmCross.Platform;

    using UIKit;

    public class MvxActionBasedTableViewSource : MvxStandardTableViewSource
    {
        protected MvxActionBasedTableViewSource(UITableView tableView)
            : base(tableView)
        {
            this.Initialize();
        }

        public MvxActionBasedTableViewSource(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("MvxActionBasedTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
            this.Initialize();
        }

        public MvxActionBasedTableViewSource(UITableView tableView,
                                             UITableViewCellStyle style,
                                             NSString cellIdentifier,
                                             string bindingText,
                                             UITableViewCellAccessory tableViewCellAccessory)
            : base(tableView, style, cellIdentifier, bindingText, tableViewCellAccessory)
        {
            this.Initialize();
        }

        public MvxActionBasedTableViewSource(UITableView tableView,
                                             UITableViewCellStyle style,
                                             NSString cellIdentifier,
                                             IEnumerable<MvxBindingDescription> descriptions,
                                             UITableViewCellAccessory tableViewCellAccessory)
            : base(tableView, style, cellIdentifier, descriptions, tableViewCellAccessory)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.CellCreator = this.CreateDefaultBindableCell;
            this.CellModifier = (ignored) => { };
        }

        public Func<UITableView, NSIndexPath, object, MvxStandardTableViewCell> CellCreator { get; set; }
        public Action<MvxStandardTableViewCell> CellModifier { get; set; }
        public Func<NSString> CellIdentifierOverride { get; set; }

        protected override NSString CellIdentifier
        {
            get
            {
                if (this.CellIdentifierOverride != null)
                    return this.CellIdentifierOverride();

                return base.CellIdentifier;
            }
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(this.CellIdentifier);
            if (reuse != null)
                return reuse;

            var cell = this.CellCreator(tableView, indexPath, item);
            this.CellModifier?.Invoke(cell);
            return cell;
        }
    }
}