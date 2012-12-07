using System;
using Android.Content;
using Android.Views;
using Android.Widget;

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

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            var view = DroidResources.LoadButtonLayout(context, convertView, parent, LayoutName);
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
            if (Click != null)
                Click(this, EventArgs.Empty);

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