// ButtonElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Views;
using Android.Widget;
using System;

namespace CrossUI.Droid.Dialog.Elements
{
    public class ButtonElement : Element, View.IOnClickListener
    {
        public ButtonElement(string caption = null, EventHandler tapped = null)
            : base(caption, "dialog_button")
        {
            Click = tapped;
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            Button button;
            DroidResources.DecodeButtonLayout(Context, cell, out button);
            button.Text = Caption;
            base.UpdateCaptionDisplay(cell);
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            var view = DroidResources.LoadButtonLayout(context, parent, LayoutName);
            if (view != null)
            {
                Button button;
                DroidResources.DecodeButtonLayout(Context, view, out button);
                button.Text = Caption;
                button.SetOnClickListener(this);
            }
            return view;
        }

        public override string Summary()
        {
            return Caption;
        }

        public void OnClick(View v)
        {
            Click?.Invoke(this, EventArgs.Empty);

            if (SelectedCommand != null)
            {
                // TODO should we have a SelectedCommandParameter here?
                if (SelectedCommand.CanExecute(null))
                {
                    SelectedCommand.Execute(null);
                }
            }
        }
    }
}