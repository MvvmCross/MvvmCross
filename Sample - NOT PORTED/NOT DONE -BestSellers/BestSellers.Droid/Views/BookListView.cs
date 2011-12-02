using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;

using BestSellers;

using MonoCross.Navigation;
using MonoCross.Droid;
using MonoDroid.Dialog;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "Best Sellers in Category", LaunchMode=Android.Content.PM.LaunchMode.SingleTop)]
    public class BookListView : MXListActivityView<BookList>
    {
        public override void Render()
        {
            ListView.Adapter = new CustomListAdapter(this, Model);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            string url = string.Format("{0}/{1}", Model[position].Category, Model[position].ISBN10);

            this.Navigate(url);
        }

        public class CustomListAdapter : BaseAdapter
        {
            BookList _items;
            Activity _context;

            public CustomListAdapter(Activity context, BookList list) : base()
            {
                _context = context;
                _items = list;
            }

            public override int Count
            {
                get { return _items.Count; }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return position;
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                //Get our object for this position
                var item = _items[position];

                //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
                // This gives us some performance gains by not always inflating a new view
                // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
                object o = _context.LayoutInflater.Inflate(Resource.Layout.ListItem, parent, false);
                var view = (convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.ListItem, parent, false)) as ViewGroup;

                //Find references to each subview in the list item's view
                //var imageItem = view.FindViewById(Resource.id.imageItem) as ImageView;
                var textTop = view.FindViewById<TextView>(Resource.Id.text1);
                var textBottom = view.FindViewById<TextView>(Resource.Id.text2);

                //Assign this item's values to the various subviews
                if (null != textTop)
                    textTop.SetText(item.Title, TextView.BufferType.Normal);
                if (null != textBottom)
                    textBottom.SetText(item.Author, TextView.BufferType.Normal);

                //Finally return the view
                return view;
            }

            public Book GetItemAtPosition(int position)
            {
                return _items[position];
            }
        }

    }
}