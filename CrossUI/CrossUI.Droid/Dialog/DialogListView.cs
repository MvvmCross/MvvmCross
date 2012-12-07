using System;
using Android.Content;
using Android.Util;
using Android.Widget;
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Dialog
{
    public class DialogListView : ListView
    {
        public RootElement Root
        {
            get { return _dialogAdapter == null ? null : _dialogAdapter.Root; }
            set
            {
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter == null)
                    Adapter = _dialogAdapter = new DialogAdapter(Context, value, this);
                else
                    _dialogAdapter.Root = value;
            }
        }
        private DialogAdapter _dialogAdapter;

        public DialogListView(Context context) :
            base(context, null)
        {
        }

        public DialogListView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
        }

        public DialogListView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
        }

        public event EventHandler ValueChanged;

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}