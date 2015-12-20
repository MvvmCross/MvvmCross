// EntryElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using CrossUI.Droid.Dialog.Enums;
using System;

namespace CrossUI.Droid.Dialog.Elements
{
    public class EntryElement : ValueElement<string>, EntryElementHelper.IEntryElementOwner
    {
        public EntryElement(string caption = null, string hint = null, string value = null, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_textfieldright")
        {
            Hint = hint;
        }

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

            if (NoAutoCorrect)
                inputType |= AndroidDialogEnumHelper.KeyboardTypeMap[UIKeyboardType.NoAutoCorrect];

            if (Lines > 1)
            {
                inputType |= InputTypes.TextFlagMultiLine;
                if (_entry.LineCount != Lines)
                    _entry.SetLines(Lines);
            }
            else if (Send != null)
            {
                if (_entry.ImeOptions != ImeAction.Go)
                {
                    _entry.ImeOptions = ImeAction.Go;
                    _entry.SetImeActionLabel("Go", ImeAction.Go);
                }
            }
            else
            {
                var imeOptions = ReturnKeyType.ImeActionFromUIReturnKeyType();
                if (_entry.ImeOptions != imeOptions)
                    _entry.ImeOptions = imeOptions;
            }
            if (_entry.InputType != inputType)
                _entry.InputType = inputType;

            //android can't seem to find the correct NextFocusDown if items are added dynamically, we'll catch the next/previous ourselves
            _entry.EditorAction += (sender, args) =>
                {
                    if (args.ActionId == ImeAction.Next ||
                        (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Honeycomb
                        && args.ActionId == ImeAction.Previous))
                    {
                        ViewGroup group = _entry.Parent as ViewGroup;
                        IViewParent currentLoop = _entry.Parent;
                        while (currentLoop != null)
                        {
                            currentLoop = currentLoop.Parent;
                            if (currentLoop is ViewGroup)
                                group = (ViewGroup)currentLoop;
                        }
                        var focus = FocusFinder.Instance.FindNextFocus(group, _entry, args.ActionId == ImeAction.Next ? FocusSearchDirection.Down : FocusSearchDirection.Up);
                        if (focus != null)
                        {
                            focus.RequestFocus();
                            focus.RequestFocusFromTouch();
                        }
                    }
                };
        }

        public override string Summary()
        {
            return Value;
        }

        private bool _password;

        public bool Password
        {
            get { return _password; }
            set
            {
                _password = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        private bool _isEmail;

        public bool IsEmail
        {
            get { return _isEmail; }
            set
            {
                _isEmail = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        public bool _numeric;

        public bool Numeric
        {
            get { return _numeric; }
            set
            {
                _numeric = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        // EntryElement is clickable by default
        // see discussion in https://github.com/MvvmCross/MvvmCross/pull/363
        private bool _clickable = true;

        public override bool Clickable
        {
            get { return _clickable; }
            set { _clickable = value; }
        }

        private bool _noAutoCorrect;

        public bool NoAutoCorrect
        {
            get { return _noAutoCorrect; }
            set
            {
                _noAutoCorrect = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        private string _hint;

        public string Hint
        {
            get { return _hint; }
            set
            {
                _hint = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        private int _rows;

        public int Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
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
            set
            {
                _send = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            var view = DroidResources.LoadStringEntryLayout(context, parent, LayoutName);
            if (view != null)
            {
                view.FocusableInTouchMode = false;
                view.Focusable = false;

                TextView label;
                EditText _entry;
                DroidResources.DecodeStringEntryLayout(context, view, out label, out _entry);

                if (_entry != null)
                {
                    view.Clickable = Clickable;
                    view.Click += (sender, args) => _entry.RequestFocus();

                    _entry.FocusableInTouchMode = true;
                    _entry.Focusable = true;
                    _entry.Clickable = Clickable;
                    var helper = EntryElementHelper.EnsureTagged(_entry);
                    helper.Owner = this;
                }
            }

            return view;
        }

        public override bool Matches(string text)
        {
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   base.Matches(text);
        }

        #region MonoTouch Dialog Mimicry

        public UIKeyboardType KeyboardType => keyboardType;

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

        #endregion MonoTouch Dialog Mimicry

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