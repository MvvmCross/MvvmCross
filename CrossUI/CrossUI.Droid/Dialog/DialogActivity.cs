using System;
using System.Linq;
using Android.App;
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Dialog
{
    public class DialogActivity : ListActivity
    {
        public RootElement Root
        {
            get { return _dialogAdapter == null ? null : _dialogAdapter.Root; }
            set
            {
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter != null)
                {
                    _dialogAdapter.DeregisterListView();
                }

                ListAdapter = _dialogAdapter = new DialogAdapter(this, value, ListView);
            }
        }
        private DialogAdapter _dialogAdapter;

        public void HandleValueChangedEvents(EventHandler eventHandler)
        {
            foreach (var element in Root.Sections.SelectMany(section => section as Section))
            {
                if (element is ValueElement)
                    (element as ValueElement).ValueChanged += eventHandler;
            }
        }

        public event EventHandler ValueChanged;
        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            return null;
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}