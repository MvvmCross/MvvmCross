// MvxStandardTableViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views
{
    using System;
    using System.Collections.Generic;

    using Foundation;

    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Platform;

    using UIKit;

    public class MvxStandardTableViewSource : MvxTableViewSource
    {
        private static readonly NSString DefaultCellIdentifier = new NSString("SimpleBindableTableViewCell");

        private static readonly MvxBindingDescription[] DefaultBindingDescription = new[]
            {
                new MvxBindingDescription
                    {
                        TargetName = "TitleText",
                        Source = new MvxPathSourceStepDescription()
                            {
                                SourcePropertyPath = string.Empty
                            }
                    },
            };

        private readonly IEnumerable<MvxBindingDescription> _bindingDescriptions;
        private readonly NSString _cellIdentifier;
        private readonly UITableViewCellStyle _cellStyle;
        private readonly UITableViewCellAccessory _tableViewCellAccessory = UITableViewCellAccessory.None;

        protected virtual NSString CellIdentifier => this._cellIdentifier;

        public MvxStandardTableViewSource(UITableView tableView)
            : this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, DefaultBindingDescription)
        {
        }

        public MvxStandardTableViewSource(UITableView tableView, NSString cellIdentifier)
            : this(tableView, UITableViewCellStyle.Default, cellIdentifier, DefaultBindingDescription)
        {
        }

        public MvxStandardTableViewSource(UITableView tableView, string bindingText)
            : this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, bindingText)
        {
        }

        public MvxStandardTableViewSource(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("MvxStandardTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        public MvxStandardTableViewSource(
            UITableView tableView,
            UITableViewCellStyle style,
            NSString cellIdentifier,
            string bindingText,
            UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : this(tableView, style, cellIdentifier, ParseBindingText(bindingText), tableViewCellAccessory)
        {
        }

        public MvxStandardTableViewSource(
            UITableView tableView,
            UITableViewCellStyle style,
            NSString cellIdentifier,
            IEnumerable<MvxBindingDescription> descriptions,
            UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(tableView)
        {
            this._cellStyle = style;
            this._cellIdentifier = cellIdentifier;
            this._bindingDescriptions = descriptions;
            this._tableViewCellAccessory = tableViewCellAccessory;
        }

        protected IEnumerable<MvxBindingDescription> BindingDescriptions => this._bindingDescriptions;

        private static IEnumerable<MvxBindingDescription> ParseBindingText(string bindingText)
        {
            if (string.IsNullOrEmpty(bindingText))
                return DefaultBindingDescription;

            return Mvx.Resolve<IMvxBindingDescriptionParser>().Parse(bindingText);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(this.CellIdentifier);
            if (reuse != null)
                return reuse;

            return this.CreateDefaultBindableCell(tableView, indexPath, item);
        }

        protected virtual MvxStandardTableViewCell CreateDefaultBindableCell(UITableView tableView,
                                                                             NSIndexPath indexPath, object item)
        {
            return new MvxStandardTableViewCell(this._bindingDescriptions, this._cellStyle, this.CellIdentifier,
                                                this._tableViewCellAccessory);
        }
    }
}