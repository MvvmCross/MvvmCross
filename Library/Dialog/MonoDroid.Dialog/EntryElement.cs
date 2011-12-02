using System;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace MonoDroid.Dialog
{
    public class EntryElement : Element, ITextWatcher
    {
        public string Value
        {
            get { return _val; }
            set
            {
                _val = value;
                if (_entry != null && _entry.Text != value)
                {
                    _entry.Text = value;
                    if (ValueChanged != null)
                        ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ValueChanged;

        public EntryElement(string caption, string value)
            : this(caption, value, (int)DroidResources.ElementLayout.dialog_textfieldright)
        {
        }

        public EntryElement(string caption, string value, int layoutId)
            : base(caption, layoutId)
        {
            _val = value;
            Lines = 1;
        }

        public override string Summary()
        {
            return _val;
        }

        public bool Password { get; set; }
        public bool Numeric { get; set; }
        public string Hint { get; set; }
        public int Lines { get; set; }

        /// <summary>
        /// An action to perform when Enter is hit
        /// </summary>
        /// <remarks>This is only meant to be set if this is the last field in your RootElement, to allow the Enter button to be used for submitting the form data.<br>
        /// If you want to perform an action when the text changes, consider hooking into ValueChanged instead.</remarks>
        public Action Send { get; set; }

        protected EditText _entry;
        private string _val;

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            TextView label;
            var view = DroidResources.LoadStringEntryLayout(context, convertView, parent, LayoutId, out label, out _entry);
            if (view != null)
            {
                // Warning! Crazy ass hack ahead!
                // since we can't know when out convertedView was was swapped from inside us, we store the
                // old textwatcher in the tag element so it can be removed!!!! (barf, rech, yucky!)
                if (_entry.Tag != null)
                    _entry.RemoveTextChangedListener((ITextWatcher)_entry.Tag);

                _entry.Text = this.Value;
                _entry.Hint = this.Hint;
                _entry.EditorAction = null;
                _entry.ImeOptions = (int)ImeAction.Unspecified;

                if (this.Password)
                    _entry.InputType = (int)(InputTypes.ClassText | InputTypes.TextVariationPassword);
                else if (this.Numeric)
                    _entry.InputType = (int)(InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned);
                else
                    _entry.InputType = (int)InputTypes.ClassText;

                if (Lines > 1)
                {
                    _entry.InputType |= (int)InputTypes.TextFlagMultiLine;
                    _entry.SetLines(Lines);
                }
                else if (Send != null)
                {
                    _entry.ImeOptions = ImeAction.Go;
                    _entry.SetImeActionLabel("Go", (int)ImeAction.Go);
                    _entry.EditorAction = (o, actionId, e) =>
                    {
                        if (actionId == ImeAction.Go)
                        {
                            Send();
                            return true;
                        }
                        return false;
                    };
                }

                // continuation of crazy ass hack, stash away the listener value so we can look it up later
                _entry.Tag = this;
                _entry.AddTextChangedListener(this);
                if (label == null)
                {
                    _entry.Hint = Caption;
                }
                else
                {
                    label.Text = Caption;
                }
            }

            return view;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_entry.Dispose();
                _entry = null;
            }
        }

        public override bool Matches(string text)
        {
            return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) || base.Matches(text);
        }

        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            this.Value = s.ToString();
        }

        public void AfterTextChanged(IEditable s)
        {
            // nothing needed
        }

        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {
            // nothing needed
        }
    }
}