// MvxStandardTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
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

        protected virtual NSString CellIdentifier => _cellIdentifier;

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
            _cellStyle = style;
            _cellIdentifier = cellIdentifier;
            _bindingDescriptions = descriptions;
            _tableViewCellAccessory = tableViewCellAccessory;
        }

        protected IEnumerable<MvxBindingDescription> BindingDescriptions => _bindingDescriptions;

        private static IEnumerable<MvxBindingDescription> ParseBindingText(string bindingText)
        {
            if (string.IsNullOrEmpty(bindingText))
                return DefaultBindingDescription;

            return Mvx.Resolve<IMvxBindingDescriptionParser>().Parse(bindingText);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(CellIdentifier);
            if (reuse != null)
                return reuse;

            return CreateDefaultBindableCell(tableView, indexPath, item);
        }

        protected virtual MvxStandardTableViewCell CreateDefaultBindableCell(UITableView tableView,
                                                                             NSIndexPath indexPath, object item)
        {
            return new MvxStandardTableViewCell(_bindingDescriptions, _cellStyle, CellIdentifier,
                                                _tableViewCellAccessory);
        }
    }
}