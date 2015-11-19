// ViewElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Util;
using Android.Views;

namespace CrossUI.Droid.Dialog.Elements
{
    public class ViewElement : Element
    {
        private readonly int _layoutId;

        public ViewElement(int layoutId)
            : base(string.Empty, null)
        {
            _layoutId = layoutId;
        }

        public ViewElement(string layoutName = null): base(string.Empty, layoutName)
        {
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
#warning convertView is junk here?
            View view = _layoutId > 0 ? DroidResources.LoadLayout(context, parent, _layoutId) : DroidResources.LoadLayout(context, parent, LayoutName);

            if (view == null)
            {
                Log.Error("Android.Dialog", "ViewElement: Failed to load resource: " + LayoutName);
            }
            else
            {
                Populate?.Invoke(view);
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