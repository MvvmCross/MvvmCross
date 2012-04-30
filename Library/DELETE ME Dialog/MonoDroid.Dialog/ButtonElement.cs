using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class ButtonElement : StringElement, View.IOnClickListener
    {
        public ButtonElement(string caption, EventHandler tapped)
            : base(caption, (int)DroidResources.ElementLayout.dialog_button)
        {
            this.Click = tapped;
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            Button button;
            var view = DroidResources.LoadButtonLayout(context, convertView, parent, LayoutId, out button);
            if (view != null)
            {
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
        }
    }
}