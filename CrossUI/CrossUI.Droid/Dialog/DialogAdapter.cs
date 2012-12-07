using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Dialog
{
    public class DialogAdapter : BaseAdapter<Section>
    {
        private readonly Context _context;

        public DialogAdapter(Context context, RootElement root, ListView listView = null)
        {
            _context = context;
            _root = root;

            // This is only really required when using a DialogAdapter with a ListView, in a non DialogActivity based activity.
            List = listView;
            RegisterListView();
        }

        public ListView List { get; set; }

        private readonly object _syncLock = new object();
        public void RegisterListView()
        {
            lock (_syncLock)
            {
                if (List == null) return;
                List.ItemClick += ListView_ItemClick;
                List.ItemLongClick += ListView_ItemLongClick;
            }
        }

        public void DeregisterListView()
        {
            lock (_syncLock)
            {
                if (List == null) return;
                List.ItemClick -= ListView_ItemClick;
                List.ItemLongClick -= ListView_ItemLongClick;
                List = null;
            }
        }

        private RootElement _root;
        public RootElement Root
        {
            get { return _root; }
            set
            {
                _root = value;
                ReloadData();
            }
        }

        public override bool IsEnabled(int position)
        {
            var element = ElementAtIndex(position);
            return !(element is Section) && element != null;
        }

        public override int Count
        {
            get
            {
                //Get each adapter's count + 2 for the header and footer
                return Root.Sections.Sum(s => (s as Section).Count() + 2);
            }
        }

        public override int ViewTypeCount
        {
            get
            {
                // ViewTypeCount is the same as Count for these,
                // there are as many ViewTypes as Views as every one is unique!
                return Count > 0 ? Count : 1;
            }
        }

        /// <summary>
        /// Return the Element for the flattened/dereferenced position value.
        /// </summary>
        /// <param name="position">The direct index to the Element.</param>
        /// <returns>The Element object at the specified position or null if too out of bounds.</returns>
        public Element ElementAtIndex(int position)
        {
            int sectionIndex = 0;
            foreach (var s in Root.Sections)
            {
                if (position == 0)
                    return Root.Sections[sectionIndex] as Section;

                // note: plus two for the section header and footer views
                var size = (s as Section).Count() + 2;
                if (position == size - 1)
                    return null;
                if (position < size)
                    return (Root.Sections[sectionIndex] as Section)[position - 1] as Element;
                position -= size;
                sectionIndex++;
            }

            return null;
        }

        public override Section this[int position]
        {
            get { return Root.Sections[position] as Section; }
        }

        public override bool AreAllItemsEnabled()
        {
            return false;
        }

        public override int GetItemViewType(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var element = ElementAtIndex(position);
            if (element == null)
            {
                element = ElementAtIndex(position - 1);
                while (!(element is Section))
                    element = element.Parent;
                return ((Section)element).GetFooterView(_context, convertView, parent);
            }
            return element.GetView(_context, convertView, parent);
        }

        public void ReloadData()
        {
            ((Activity)_context).RunOnUiThread(() =>
            {
                if (Root != null)
                {
                    NotifyDataSetChanged();
                }
            });
        }

        /// <summary>
        /// Handles the ItemClick event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Android.Widget.AdapterView.ItemClickEventArgs"/> instance containing the event data.</param>
        public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var elem = ElementAtIndex(e.Position);
            if (elem == null) return;
            elem.Selected();
            if (elem.Click != null)
                elem.Click(sender, e);
        }

        /// <summary>
        /// Handles the ItemLongClick event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Android.Widget.AdapterView.ItemLongClickEventArgs"/> instance containing the event data.</param>
        public void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var elem = ElementAtIndex(e.Position);
            if (elem != null && elem.LongClick != null)
                elem.LongClick(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            DeregisterListView();
            base.Dispose(disposing);
        }
    }
}