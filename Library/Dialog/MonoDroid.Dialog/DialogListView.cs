using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class DialogListView : ListView
    {
        public RootElement Root
        {
            get { return _root; }
            set { _root = value; this.Adapter = _dialogAdapter = new DialogAdapter(Context, _root); }
        }
        private RootElement _root;

        public DialogAdapter DialogAdapter
        {
            get { return _dialogAdapter; }
            set { this.Adapter = _dialogAdapter = value; }
        }
        private DialogAdapter _dialogAdapter;

        public DialogListView(Context context) :
            base(context, null)
        {
            Initialize();
        }

        public DialogListView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public DialogListView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.ItemClick += (sender, eventArgs) => {
                Element elem = _dialogAdapter.ElementAtIndex(eventArgs.Position);
                if (elem != null && elem.Click != null)
                    elem.Click(sender, eventArgs);
            };
            /*
            this.ItemLongClick = delegate() {
                Element elem = _dialogAdapter.ElementAtIndex(eventArgs.Position);
                if (elem != null && elem.LongClick != null)
                    elem.LongClick(sender, eventArgs);
            };
             */
        }
    }
}