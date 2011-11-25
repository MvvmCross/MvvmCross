using Android.Content;
using Android.Views;

namespace MonoDroid.Dialog
{
    public class MultilineEntryElement : EntryElement
    {
        public MultilineEntryElement(string caption, string value)
            : base(caption, value, (int)DroidResources.ElementLayout.dialog_textarea)
        {
            Lines = 3;
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            View view = DroidResources.LoadMultilineElementLayout(context, convertView, parent, LayoutId, out _entry);
            if (_entry != null)
            {
                _entry.SetLines(Lines);
                _entry.Text = Value;
                _entry.Hint = Caption;
            }

            return view;
        }
    }
}