using System;
using Android.Content;
using Android.Util;
using Android.Views;

namespace CrossUI.Droid.Dialog.Elements
{
    public class ViewElement : Element
    {
        private int _layoutId;

        public ViewElement(int layoutId)
            : base(string.Empty, null)
        {
            _layoutId = layoutId;
        }

        public ViewElement(string layoutName)
            : base(string.Empty, layoutName)
        {
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
#warning convertView is junk here?
            View view;
            if (_layoutId > 0)
                view = DroidResources.LoadLayout(context, parent, _layoutId);
            else
                view = DroidResources.LoadLayout(context, parent, LayoutName);

            if (view == null)
            {
                Log.Error("Android.Dialog", "ViewElement: Failed to load resource: " + LayoutName);
            }
            else if (Populate != null)
            {
                Populate(view);
            }
            return view;
        }

        /// <summary>
        /// Gets or sets the <see cref="Action{T}"/> that populates the <see cref="View"/> that was inflated from the Layout ID passed in on the constructor.
        /// </summary>
        /// <value>
        /// The <see cref="Action{T}"/> that hydrates the inflated View with data.
        /// </value>
        public Action<View> Populate { get; set; }
    }
}