// MvxSimpleBindableTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.ExtensionMethods;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxSimpleBindableTableViewSource : MvxBindableTableViewSource
    {
        private static readonly NSString DefaultCellIdentifier = new NSString("SimpleBindableTableViewCell");

        private static readonly MvxBindingDescription[] DefaultBindingDescription = new[]
            {
                new MvxBindingDescription
                    {
                        TargetName = "TitleText",
                        SourcePropertyPath = string.Empty
                    },
            };

        private readonly IEnumerable<MvxBindingDescription> _bindingDescriptions;
        private readonly NSString _cellIdentifier;
        private readonly UITableViewCellStyle _cellStyle;
        private readonly UITableViewCellAccessory _tableViewCellAccessory = UITableViewCellAccessory.None;

        protected virtual NSString CellIdentifier
        {
            get { return _cellIdentifier; }
        }

        public MvxSimpleBindableTableViewSource(UITableView tableView)
            : this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, DefaultBindingDescription)
        {
        }

		public MvxSimpleBindableTableViewSource(UITableView tableView, string bindingText)
			: this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, bindingText)
		{
		}

        public MvxSimpleBindableTableViewSource(
            UITableView tableView, 
            UITableViewCellStyle style,
            NSString cellIdentifier, 
            string bindingText,
            UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : this(tableView, style, cellIdentifier, ParseBindingText(bindingText), tableViewCellAccessory)
        {
        }

        public MvxSimpleBindableTableViewSource(
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

        protected IEnumerable<MvxBindingDescription> BindingDescriptions
        {
            get { return _bindingDescriptions; }
        }

        private static IEnumerable<MvxBindingDescription> ParseBindingText(string bindingText)
        {
            if (string.IsNullOrEmpty(bindingText))
                return DefaultBindingDescription;

            return MvxServiceProviderExtensions.GetService<IMvxBindingDescriptionParser>().Parse(bindingText);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(CellIdentifier);
            if (reuse != null)
                return reuse;

            return CreateDefaultBindableCell(tableView, indexPath, item);
        }

        protected virtual MvxBindableTableViewCell CreateDefaultBindableCell(UITableView tableView,
                                                                             NSIndexPath indexPath, object item)
        {
            return new MvxBindableTableViewCell(_bindingDescriptions, _cellStyle, CellIdentifier,
                                                _tableViewCellAccessory);
        }
    }
}