﻿#region Copyright

// <copyright file="BooleanElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.Content;
using Android.Views;
using Android.Widget;

namespace CrossUI.Droid.Dialog.Elements
{
    /// <summary>
    /// Used to display toggle button on the screen.
    /// </summary>
    public class BooleanElement : BoolElement, CompoundButton.IOnCheckedChangeListener
    {
        public BooleanElement(string caption = null, bool value = false, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_onofffieldright")
        {
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            View view = DroidResources.LoadBooleanElementLayout(context, convertView, parent, LayoutName);
            return view;
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

            View _rawToggleButton;
            TextView _caption;
            TextView _subCaption;

            DroidResources.DecodeBooleanElementLayout(Context, cell, out _caption, out _subCaption, out _rawToggleButton);
            _caption.Text = Caption;
        }

        protected override void UpdateDetailDisplay(View cell)
        {
            if (cell == null)
                return;

            View _rawToggleButton;
            TextView _caption;
            TextView _subCaption;

            DroidResources.DecodeBooleanElementLayout(Context, cell, out _caption, out _subCaption, out _rawToggleButton);
            var _toggleButton = (ToggleButton) _rawToggleButton;
            _toggleButton.SetOnCheckedChangeListener(null);
            _toggleButton.Checked = Value;
            _toggleButton.SetOnCheckedChangeListener(this);

            if (TextOff != null)
            {
                _toggleButton.TextOff = TextOff;
                if (!Value)
                    _toggleButton.Text = TextOff;
            }

            if (TextOn != null)
            {
                _toggleButton.TextOn = TextOn;
                if (Value)
                    _toggleButton.Text = TextOn;
            }
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            OnUserValueChanged(isChecked);
        }

        public override void Selected()
        {
            if (CurrentAttachedCell == null)
            {
                // how did this happen?!
                return;
            }

            View _rawToggleButton;
            TextView _caption;
            TextView _subCaption;

            DroidResources.DecodeBooleanElementLayout(Context, CurrentAttachedCell, out _caption, out _subCaption,
                                                      out _rawToggleButton);
            var _toggleButton = (ToggleButton) _rawToggleButton;

            if (_toggleButton != null)
                _toggleButton.Toggle();
        }
    }
}