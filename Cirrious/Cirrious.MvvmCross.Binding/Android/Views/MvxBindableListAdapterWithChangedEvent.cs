#region Copyright
// <copyright file="MvxBindableListAdapterWithChangedEvent.cs" company="Cirrious">
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

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableListAdapterWithChangedEvent
        : MvxBindableListAdapter 
    {
        public MvxBindableListAdapterWithChangedEvent(Context context)
            : base(context)
        {
        }

        public event EventHandler DataSetChanged;

        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();

            var handler = DataSetChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}