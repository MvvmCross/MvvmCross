using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class CheckboxElement : Element, CompoundButton.IOnCheckedChangeListener
    {
        public bool Value
        {
            get { return _val; }
            set
            {
                bool emit = _val != value;
                _val = value;
                if (emit && ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }
        }
        private bool _val;

        public event EventHandler ValueChanged;

        private CheckBox _checkbox;
        private TextView _caption;
        private TextView _subCaption;

        public string Group;

        public CheckboxElement(string caption)
            : base(caption, (int)DroidResources.ElementLayout.dialog_boolfieldright)
        {
            Value = false;
        }

        public CheckboxElement(string caption, bool value)
            : base(caption, (int)DroidResources.ElementLayout.dialog_boolfieldright)
        {
            Value = value;
        }

        public CheckboxElement(string caption, bool value, string group)
            : base(caption, (int)DroidResources.ElementLayout.dialog_boolfieldright)
        {
            Value = value;
            Group = group;
        }

        public CheckboxElement(string caption, bool value, string group, int layoutId)
            : base(caption, layoutId)
        {
            Value = value;
            Group = group;
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            View checkboxView;
            View view = DroidResources.LoadBooleanElementLayout(context, convertView, parent, LayoutId, out _caption, out _subCaption, out checkboxView);
            if (view != null)
            {
                _caption.Text = Caption;
                _checkbox = checkboxView as CheckBox;
                _checkbox.SetOnCheckedChangeListener(null);
                _checkbox.Checked = Value;
                _checkbox.SetOnCheckedChangeListener(this);
            }
            return view;
        }


        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            this.Value = isChecked;
        }
    }
}