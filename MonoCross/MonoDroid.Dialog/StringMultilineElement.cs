using System;

using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Runtime;


namespace MonoDroid.Dialog
{
    public class StringMultilineElement : Element
    {
        public string Value
        {
            get { return _value; }
            set { _value = value; if (_text != null) _text.Text = _value; }
        }
        private string _value;

        public object Alignment;

        public StringMultilineElement(string caption)
            : base(caption, (int)DroidResources.ElementLayout.dialog_multiline_labelfieldbelow)
        {
        }

        public StringMultilineElement(string caption, int layoutId)
            : base(caption, layoutId)
        {
        }

        public StringMultilineElement(string caption, string value)
            : base(caption, (int)DroidResources.ElementLayout.dialog_multiline_labelfieldbelow)
        {
            Value = value;
        }

        public StringMultilineElement(string caption, string value, int layoutId)
            : base(caption, layoutId)
        {
            Value = value;
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            View view = DroidResources.LoadStringElementLayout(context, convertView, parent, LayoutId, out _caption, out _text);
            if (view != null)
            {
                _caption.Text = Caption;
                _text.Text = Value;
            }
            return view;
        }

        public override string Summary()
        {
            return Value;
        }

        public override bool Matches(string text)
        {
            return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) ||
                   base.Matches(text);
        }

        protected TextView _caption;
        protected TextView _text;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_caption.Dispose();
                _caption = null;
                //_text.Dispose();
                _text = null;
            }
        }
    }
}