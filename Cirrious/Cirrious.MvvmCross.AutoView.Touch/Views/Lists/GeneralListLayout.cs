// GeneralListLayout.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Touch.Views;
using CrossUI.Core.Elements.Lists;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Lists
{
    public class GeneralListLayout : IListLayout
    {
        private MvxTableViewSource _source;
        private IEnumerable _itemsSource;
        private ICommand _itemClick;
        private IListItemLayout _defaultLayout;
        private Dictionary<string, IListItemLayout> _itemLayouts;

        public GeneralListLayout()
        {
            ItemLayouts = new Dictionary<string, IListItemLayout>();
        }

        public virtual MvxTableViewSource InitializeSource(UITableView tableView)
        {
            if (_source != null)
            {
                throw new MvxException("You cannot create the source more than once");
            }

            _source = CreateSource(tableView);
            _source.ItemsSource = this.ItemsSource;
            _source.SelectionChangedCommand = this.ItemClick;
            return _source;
        }

        protected virtual MvxTableViewSource CreateSource(UITableView tableView)
        {
            return new GeneralTableViewSource(
                tableView,
                DefaultLayout as IMvxLayoutListItemViewFactory,
                ItemLayouts.ToDictionary(x => x.Key, x => x.Value as IMvxLayoutListItemViewFactory));
        }

        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                _itemsSource = value;
                if (_source != null) _source.ItemsSource = _itemsSource;
            }
        }

        public ICommand ItemClick
        {
            get { return _itemClick; }
            set
            {
                _itemClick = value;
                if (_source != null) _source.SelectionChangedCommand = _itemClick;
            }
        }

        public IListItemLayout DefaultLayout
        {
            get { return _defaultLayout; }
            set
            {
                CheckListIsNull();
                _defaultLayout = value;
            }
        }

        public Dictionary<string, IListItemLayout> ItemLayouts
        {
            get { return _itemLayouts; }
            private set
            {
                CheckListIsNull();
                _itemLayouts = value;
            }
        }

        private void CheckListIsNull()
        {
            if (_source != null)
            {
                throw new MvxException("You cannot set the default layout after the list has been created.");
            }
        }
    }
}