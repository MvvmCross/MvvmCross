using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Foobar.Dialog.Core.Lists;
using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    public class GeneralListLayout : IListLayout
    {
#if false
        private UITab _list;
        private IEnumerable _itemsSource;
        private ICommand _itemClick;
        private IListItemLayout _defaultLayout;
        private Dictionary<string, IListItemLayout> _itemLayouts;

        public GeneralListLayout()
        {
            ItemLayouts = new Dictionary<string, IListItemLayout>();
        }

        public virtual ListView InitialiseListView(Context context)
        {
            if (_list != null)
            {
                throw new MvxException("You cannot create the list more than once");
            }

            _list = CreateList(context);
            _list.ItemsSource = this.ItemsSource;
            _list.ItemClick = this.ItemClick;
            _list.SetBackgroundColor(Color.CornflowerBlue); 
            _list.LayoutParameters = new ViewGroup.LayoutParams(400, 300);
            return _list;
        }

        protected virtual MvxBindableListView CreateList(Context context)
        {
            return new MvxBindableListView(context, null, CreateAdapter(context));
        }

        protected virtual MvxLayoutDrivenListAdapter CreateAdapter(Context context)
        {
#warning TODO - this "casting" could be more efficient
            return new MvxLayoutDrivenListAdapter(
                            context, 
                            DefaultLayout as IMvxLayoutListItemViewFactory, 
                            ItemLayouts.ToDictionary(x => x.Key, x => x.Value as IMvxLayoutListItemViewFactory));
        }

        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { _itemsSource = value; if (_list != null) _list.ItemsSource = _itemsSource; }
        }

        public ICommand ItemClick
        {
            get { return _itemClick; }
            set { _itemClick = value; if (_list != null) _list.ItemClick = _itemClick; }
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
            if (_list != null)
            {
                throw new MvxException("You cannot set the default layout after the list has been created.");
            }
        }
#endif

        public IListItemLayout DefaultLayout
        {
            get { throw new System.NotImplementedException(); }
        }

        public Dictionary<string, IListItemLayout> ItemLayouts
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
