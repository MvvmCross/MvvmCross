// GeneralTableViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.AutoView.iOS.Interfaces.Lists;

namespace MvvmCross.AutoView.iOS.Views.Lists
{
    using System;
    using System.Collections.Generic;

    using Foundation;

    using iOS.Interfaces.Lists;
    using Binding.iOS.Views;
    using MvvmCross.Platform;

    using UIKit;

    public class GeneralTableViewSource : MvxTableViewSource
    {
        private readonly IMvxLayoutListItemViewFactory _defaultFactory;
        private readonly Dictionary<string, IMvxLayoutListItemViewFactory> _factories;

        public GeneralTableViewSource(UITableView tableView, IMvxLayoutListItemViewFactory defaultFactory,
                                      Dictionary<string, IMvxLayoutListItemViewFactory> factories)
            : base(tableView)
        {
            this._defaultFactory = defaultFactory;
            this._factories = factories;
        }

        public GeneralTableViewSource(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("GeneralTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        private const string BaseCellIdentifier = @"GeneralTabelViewSource_";

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cellId = GetCellIdentifier(item);
            var reuse = tableView.DequeueReusableCell(cellId);

            if (reuse != null)
                return reuse;

            IMvxLayoutListItemViewFactory factory;
            if (this._factories != null || !this._factories.TryGetValue(item.GetType().Name, out factory))
            {
                factory = this._defaultFactory;
            }

            var cell = factory.BuildView(indexPath, item, cellId);
            return cell;
        }

        private static string GetCellIdentifier(object item)
        {
            return BaseCellIdentifier + item;
        }
    }
}