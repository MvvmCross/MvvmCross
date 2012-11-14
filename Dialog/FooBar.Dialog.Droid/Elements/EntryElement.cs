﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace FooBar.Dialog.Droid
{
    public class EntryElement : ValueElement<string>, EntryElementHelper.IEntryElementOwner
    {
        protected override void UpdateCellDisplay(View cell)
        {
            UpdateDetailDisplay(cell);
            base.UpdateCellDisplay(cell);
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView label;
            EditText _entry;
            DroidResources.DecodeStringEntryLayout(Context, cell, out label, out _entry);

            if (label != null)
            {
                // note - Caption and Hint are no longer interchanged!
                label.Text = Caption;
            }
        }

        protected override void UpdateDetailDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView label;
            EditText _entry;
            DroidResources.DecodeStringEntryLayout(Context, cell, out label, out _entry);

            if (_entry == null)
                return;

            if (_entry.Text != Value)
            {
                _entry.Text = Value;
            }
            if (_entry.Hint != Hint)
            {
                _entry.Hint = Hint;
            }

            var inputType = KeyboardType.InputTypesFromUIKeyboardType();

            if (Password)
                inputType |= InputTypes.TextVariationPassword;

            if (IsEmail)
                inputType |= AndroidDialogEnumHelper.KeyboardTypeMap[UIKeyboardType.EmailAddress];

            if (Numeric)
                inputType |= AndroidDialogEnumHelper.KeyboardTypeMap[UIKeyboardType.DecimalPad];

            if (Lines > 1)
            {
                inputType |= InputTypes.TextFlagMultiLine;
                _entry.SetLines(Lines);
            }
            else if (Send != null)
            {
                _entry.ImeOptions = ImeAction.Go;
                _entry.SetImeActionLabel("Go", ImeAction.Go);
            }
            else
            {
                _entry.ImeOptions = ReturnKeyType.ImeActionFromUIReturnKeyType();
            }

            _entry.InputType = inputType;
        }

        public EntryElement(string caption = null, string hint = null, string value = null, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_textfieldright")
        {
            Hint = hint;
        }

        public override string Summary()
        {
            return Value;
        }

        private bool _password;
        public bool Password
        {
            get { return _password; }
            set { _password = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private bool _isEmail;
        public bool IsEmail
        {
            get { return _isEmail; }
            set
            { _isEmail = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        public bool _numeric;
        public bool Numeric
        {
            get { return _numeric; }
            set { _numeric = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private string _hint;
        public string Hint
        {
            get { return _hint; }
            set { _hint = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private int _rows;
        public int Rows
        {
            get { return _rows; }
            set { _rows = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        public int Lines
        {
            get { return Rows; }
            set { Rows = value; }
        }

        /// <summary>
        /// An action to perform when Enter is hit
        /// </summary>
        /// <remarks>This is only meant to be set if this is the last field in your RootElement, to allow the Enter button to be used for submitting the form data.<br>
        /// If you want to perform an action when the text changes, consider hooking into Changed instead.</remarks>
        private Action _send;
        public Action Send
        {
            get { return _send; }
            set { _send = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            var view = DroidResources.LoadStringEntryLayout(context, convertView, parent, LayoutName);
            if (view != null)
            {
                view.FocusableInTouchMode = false;
                view.Focusable = false;
                view.Clickable = false;

                TextView label;
                EditText _entry;
                DroidResources.DecodeStringEntryLayout(context, view, out label, out _entry);

                _entry.FocusableInTouchMode = true;
                _entry.Focusable = true;
                _entry.Clickable = true;

                var helper = EntryElementHelper.EnsureTagged(_entry);
                helper.Owner = this;
            }

            return view;
        }

        public override bool Matches(string text)
        {
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 || base.Matches(text);
        }

        #region MonoTouch Dialog Mimicry

        public UIKeyboardType KeyboardType
        {
            get { return keyboardType; }
            set { keyboardType = value; }
        }
        private UIKeyboardType keyboardType;

        public UIReturnKeyType ReturnKeyType
        {
            get { return returnKeyType; }
            set { returnKeyType = value; }
        }
        private UIReturnKeyType returnKeyType;

        // Not used in any way, just there to match MT Dialog api.
        public UITextFieldViewMode ClearButtonMode
        {
            get { return clearButtonMode; }
            set { clearButtonMode = value; }
        }
        private UITextFieldViewMode clearButtonMode;

        #endregion

        public virtual void OnTextChanged(string newText)
        {
            //Log.Info("Just playing","New text:" + newText);
            OnUserValueChanged(newText);
        }

        public virtual void OnEditorAction(TextView.EditorActionEventArgs e)
        {
            //Log.Info("Just playing", "Action:" + e.ActionId);
            if (e.ActionId == ImeAction.Go)
            {
                Send();
            }
        }
    }
}