using System;
using Android.Content;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class DialogHelper
    {
        private DialogAdapter DialogAdapter { get; set; }

        public DialogHelper(Context context, ListView dialogView, RootElement root)
        {
            root.Context = context;

            dialogView.Adapter = this.DialogAdapter = new DialogAdapter(context, root);
            dialogView.ItemClick += new EventHandler<ItemEventArgs>(ListView_ItemClick);

            // TODO - sort out long click changes
            /*
            dialogView.ItemLongClick = new AdapterView.ItemLongClickHandler(ListView_ItemLongClick);
            dialogView.ItemLongClick += new EventHandler<ItemEventArgs>(ListView_ItemLongClick);
            */
            dialogView.Tag = root;
        }

        void ListView_ItemLongClick(object sender, ItemEventArgs e)
        {
            Element elem = this.DialogAdapter.ElementAtIndex(e.Position);
            if (elem != null && elem.LongClick != null)
                elem.LongClick(sender, e);
        }

        void ListView_ItemClick(object sender, ItemEventArgs e)
        {
            Element elem = this.DialogAdapter.ElementAtIndex(e.Position);
            if (elem != null && elem.Click != null)
                elem.Click(sender, e);
        }
    }
}