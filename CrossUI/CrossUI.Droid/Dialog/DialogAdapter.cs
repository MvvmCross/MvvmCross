// DialogAdapter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using CrossUI.Droid.Dialog.Elements;
using System.Linq;

namespace CrossUI.Droid.Dialog
{
    public class DialogAdapter : BaseAdapter<Section>
    {
        private readonly Context _context;

        public DialogAdapter(Context context, RootElement root, ListView listView = null)
        {
            _context = context;
            _root = root;

            _root.ElementsChanged += OnElementsChanged;

            // This is only really required when using a DialogAdapter with a ListView, in a non DialogActivity based activity.
            List = listView;
            RegisterListView();
        }

        private void OnElementsChanged(object sender, System.EventArgs e)
        {
            ReloadData();
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
                if (_root != value)
                {
                    if (_root != null)
                        _root.ElementsChanged -= OnElementsChanged;
                    _root = value;
                    if (_root != null)
                        _root.ElementsChanged += OnElementsChanged;
                    ReloadData();
                }
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
                return Root.Sections.Sum(s => (s).Count() + 2);
            }
        }

        public override int GetItemViewType(int position)
        {
            return Adapter.IgnoreItemViewType;
        }

        // returning 1 here - I couldn't find any docs on what to return with Adapter.IgnoreItemViewType
        public override int ViewTypeCount => 1;

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
                    return Root.Sections[sectionIndex];

                // note: plus two for the section header and footer views
                var size = (s).Count() + 2;
                if (position == size - 1)
                    return null;
                if (position < size)
                    return (Root.Sections[sectionIndex])[position - 1];
                position -= size;
                sectionIndex++;
            }

            return null;
        }

        public override Section this[int position] => Root.Sections[position];

        public override bool AreAllItemsEnabled()
        {
            return false;
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
            elem.Click?.Invoke(sender, e);
        }

        /// <summary>
        /// Handles the ItemLongClick event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Android.Widget.AdapterView.ItemLongClickEventArgs"/> instance containing the event data.</param>
        public void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var elem = ElementAtIndex(e.Position);
            elem?.LongClick?.Invoke(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (_root != null)
                _root.ElementsChanged -= OnElementsChanged;
            _root = null;
            DeregisterListView();
            base.Dispose(disposing);
        }
    }
}