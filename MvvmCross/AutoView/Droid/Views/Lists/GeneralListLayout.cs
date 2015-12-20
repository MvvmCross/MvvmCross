// GeneralListLayout.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Views.Lists
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Android.Content;
    using Android.Widget;

    using CrossUI.Core.Elements.Lists;

    using MvvmCross.AutoView.Droid.Interfaces.Lists;
    using MvvmCross.Binding.Droid.Views;
    using MvvmCross.Platform.Exceptions;

    public class GeneralListLayout : IListLayout
    {
        private MvxListView _list;
        private IEnumerable _itemsSource;
        private ICommand _itemClick;
        private IListItemLayout _defaultLayout;
        private Dictionary<string, IListItemLayout> _itemLayouts;

        public GeneralListLayout()
        {
            this.ItemLayouts = new Dictionary<string, IListItemLayout>();
        }

        public virtual ListView InitializeListView(Context context)
        {
            if (this._list != null)
            {
                throw new MvxException("You cannot create the list more than once");
            }

            this._list = this.CreateList(context);
            this._list.ItemsSource = this.ItemsSource;
            this._list.ItemClick = this.ItemClick;
            // for testing some times it helps to see the list!
            // _list.SetBackgroundColor(Color.CornflowerBlue);
            return this._list;
        }

        protected virtual MvxListView CreateList(Context context)
        {
            return new MvxListView(context, null, this.CreateAdapter(context));
        }

        protected virtual MvxLayoutDrivenAdapter CreateAdapter(Context context)
        {
#warning TODO - this "casting" could be more efficient
            return new MvxLayoutDrivenAdapter(
                context,
                this.DefaultLayout as IMvxLayoutListItemViewFactory,
                this.ItemLayouts.ToDictionary(x => x.Key, x => x.Value as IMvxLayoutListItemViewFactory));
        }

        public IEnumerable ItemsSource
        {
            get { return this._itemsSource; }
            set
            {
                this._itemsSource = value;
                if (this._list != null) this._list.ItemsSource = this._itemsSource;
            }
        }

        public ICommand ItemClick
        {
            get { return this._itemClick; }
            set
            {
                this._itemClick = value;
                if (this._list != null) this._list.ItemClick = this._itemClick;
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
            if (this._list != null)
            {
                throw new MvxException("You cannot set the default layout after the list has been created.");
            }
        }
    }
}