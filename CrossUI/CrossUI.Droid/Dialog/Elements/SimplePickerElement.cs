using System.Collections;
using System.Linq;
using Android.App;
using Android.Content;

namespace CrossUI.Droid.Dialog.Elements
{
    public class SimplePickerElement : StringDisplayingValueElement<object>
    {
        IList _entries = new object[0];
        public IList Entries
        {
            get { return _entries; }
            set { _entries = value; BindItemArray(); }
        }

        private void BindItemArray()
        {
            var arr = new object[Entries.Count];
            Entries.CopyTo(arr, 0);
            _itemArray = arr.Select(x => (x ?? "").ToString()).ToArray();
        }

        string[] _itemArray = new string[0];

        private int FindSelectedIndex()
        {
            var i = 0;
            foreach (var x in Entries)
            {
                if (x == Value)
                    return i;
                i++;
            }
            return -1;
        }

        public SimplePickerElement(string caption, object value = null, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_multiline_labelfieldbelow")
        {
            Click = delegate { ShowDialog(); };
        }

        private void ShowDialog()
        {
            var context = Context;
            if (context == null)
            {
                Android.Util.Log.Warn("RadioDialogElement", "No Context for Edit");
                return;
            }

            new AlertDialog.Builder(context)
            .SetTitle(base.Caption)
            .SetSingleChoiceItems(_itemArray, FindSelectedIndex(), (o, e) => OnClick(o, e))
            .Create()
            .Show();
        }

        protected virtual void OnClick(object o, DialogClickEventArgs e)
        {
            OnUserValueChanged(Entries[e.Which]);
            ((AlertDialog)o).Dismiss();
        }

        protected override string Format(object value)
        {
            if (value == null)
                return string.Empty;
            return value.ToString();
        }
    }
}