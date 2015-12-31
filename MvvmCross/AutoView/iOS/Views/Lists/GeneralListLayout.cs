// GeneralListLayout.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Views.Lists
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using CrossUI.Core.Elements.Lists;

    using MvvmCross.AutoView.iOS.Interfaces.Lists;
    using Binding.iOS.Views;
    using MvvmCross.Platform.Exceptions;

    using UIKit;

    public class GeneralListLayout : IListLayout
    {
        private MvxTableViewSource _source;
        private IEnumerable _itemsSource;
        private ICommand _itemClick;
        private IListItemLayout _defaultLayout;
        private Dictionary<string, IListItemLayout> _itemLayouts;

        public GeneralListLayout()
        {
            this.ItemLayouts = new Dictionary<string, IListItemLayout>();
        }

        public virtual MvxTableViewSource InitializeSource(UITableView tableView)
        {
            if (this._source != null)
            {
                throw new MvxException("You cannot create the source more than once");
            }

            this._source = this.CreateSource(tableView);
            this._source.ItemsSource = this.ItemsSource;
            this._source.SelectionChangedCommand = this.ItemClick;
            return this._source;
        }

        protected virtual MvxTableViewSource CreateSource(UITableView tableView)
        {
            return new GeneralTableViewSource(
                tableView,
                this.DefaultLayout as IMvxLayoutListItemViewFactory,
                this.ItemLayouts.ToDictionary(x => x.Key, x => x.Value as IMvxLayoutListItemViewFactory));
        }

        public IEnumerable ItemsSource
        {
            get { return this._itemsSource; }
            set
            {
                this._itemsSource = value;
                if (this._source != null) this._source.ItemsSource = this._itemsSource;
            }
        }

        public ICommand ItemClick
        {
            get { return this._itemClick; }
            set
            {
                this._itemClick = value;
                if (this._source != null) this._source.SelectionChangedCommand = this._itemClick;
            }
        }

        public IListItemLayout DefaultLayout
        {
            get { return this._defaultLayout; }
            set
            {
                this.CheckListIsNull();
                this._defaultLayout = value;
            }
        }

        public Dictionary<string, IListItemLayout> ItemLayouts
        {
            get { return this._itemLayouts; }
            private set
            {
                this.CheckListIsNull();
                this._itemLayouts = value;
            }
        }

        private void CheckListIsNull()
        {
            if (this._source != null)
            {
                throw new MvxException("You cannot set the default layout after the list has been created.");
            }
        }
    }
}