// CheckboxElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Views;
using Android.Widget;

namespace CrossUI.Droid.Dialog.Elements
{
    public class CheckboxElement : BoolElement, CompoundButton.IOnCheckedChangeListener
    {
        public string SubCaption { get; set; }

        public bool ReadOnly { get; set; }

        public string Group { get; private set; }

        private static string SelectLayoutName(string layoutName, string subCaption)
        {
            if (layoutName != null)
                return layoutName;

            return subCaption == null ? "dialog_boolfieldright" : "dialog_boolfieldsubright";
        }

        public CheckboxElement(string caption = null, bool value = false, string subCaption = null, string group = null,
                               string layoutName = null)
            : base(caption, value, SelectLayoutName(layoutName, subCaption))
        {
            Group = group;
            SubCaption = subCaption;
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            View view = DroidResources.LoadBooleanElementLayout(context, parent, LayoutName);
            return view;
        }

        protected override void UpdateCellDisplay(View cell)
        {
            UpdateDetailDisplay(cell);
            base.UpdateCellDisplay(cell);
        }

        protected override void UpdateDetailDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView _caption;
            TextView _subCaption;
            View _rawCheckboxView;
            DroidResources.DecodeBooleanElementLayout(Context, cell, out _caption, out _subCaption, out _rawCheckboxView);

            var _checkbox = (CheckBox)_rawCheckboxView;
            _checkbox.SetOnCheckedChangeListener(null);
            _checkbox.Checked = Value;
            _checkbox.SetOnCheckedChangeListener(this);
            _checkbox.Clickable = !ReadOnly;
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView _caption;
            TextView _subCaption;
            View _rawCheckboxView;
            DroidResources.DecodeBooleanElementLayout(Context, cell, out _caption, out _subCaption, out _rawCheckboxView);
            _caption.Text = Caption;

            if (_subCaption != null)
            {
                _subCaption.Text = SubCaption;
            }
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            OnUserValueChanged(isChecked);
        }

        public override void Selected()
        {
            if (ReadOnly)
                return;

            if (CurrentAttachedCell == null)
            {
                // how on earth did this happen!
                return;
            }

            TextView _caption;
            TextView _subCaption;
            View _rawCheckboxView;
            DroidResources.DecodeBooleanElementLayout(Context, CurrentAttachedCell, out _caption, out _subCaption,
                                                      out _rawCheckboxView);

            var _checkbox = (CheckBox)_rawCheckboxView;
            _checkbox.Toggle();
        }

        public override string Summary()
        {
            return Value ? "On" : "Off"; //Because iOS, that's why.
        }
    }
}