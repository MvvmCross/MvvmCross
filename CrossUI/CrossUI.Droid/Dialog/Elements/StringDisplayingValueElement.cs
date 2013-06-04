// StringDisplayingValueElement.cs
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
    public abstract class StringDisplayingValueElement<T> : ValueElement<T>
    {
        public int FontSize { get; set; }

        protected StringDisplayingValueElement(string caption = null, T value = default(T), string layoutName = null)
            : base(caption, value, layoutName)
        {
        }

        protected override void UpdateDetailDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView label;
            TextView value;
            DroidResources.DecodeStringElementLayout(Context, CurrentAttachedCell, out label, out value);

            if (value != null)
                value.Text = Format(Value);
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView label;
            TextView value;
            DroidResources.DecodeStringElementLayout(Context, CurrentAttachedCell, out label, out value);
            label.Text = Caption;
            label.Visibility = Caption == null ? ViewStates.Gone : ViewStates.Visible;
        }

        protected override void UpdateCellDisplay(View cell)
        {
            UpdateDetailDisplay(cell);
            base.UpdateCellDisplay(cell);
        }

        protected abstract string Format(T value);

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            var view = DroidResources.LoadStringElementLayout(context, convertView, parent, LayoutName);
            if (view != null)
            {
                if (FontSize > 0)
                {
                    TextView label;
                    TextView value;
                    DroidResources.DecodeStringElementLayout(Context, CurrentAttachedCell, out label, out value);
                    label.TextSize = FontSize;
                    value.TextSize = FontSize;
                }
            }
            return view;
        }
    }
}