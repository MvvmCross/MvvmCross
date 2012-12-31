#region Copyright

// <copyright file="ViewElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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