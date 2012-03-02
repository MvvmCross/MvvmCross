#region Copyright
// <copyright file="MvxBindableLinearLayout.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public static class MvxBindableLinearLayoutExtensions
    {
        public static void Refill(this LinearLayout layout, IAdapter adapter)
        {
            layout.RemoveAllViews();
            var count = adapter.Count;
            for (var i = 0; i < count; i++)
            {
                layout.AddView(adapter.GetView(i, null, layout));
            }            
        }
    }

    public class MvxBindableLinearLayout
        : LinearLayout
    {
        public MvxBindableLinearLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            var itemTemplateId = MvxBindableListViewHelpers.ReadTemplatePath(context, attrs);
            Adapter = new MvxBindableListAdapterWithChangedEvent(context);
            Adapter.ItemTemplateId = itemTemplateId;
            Adapter.DataSetChanged += AdapterOnDataSetChanged;
        }

        public MvxBindableListAdapterWithChangedEvent Adapter { get; set; }

        public IList ItemsSource
        {
            get { return Adapter.ItemsSource; }
            set { Adapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return Adapter.ItemTemplateId; }
            set { Adapter.ItemTemplateId = value; }
        }

        private void AdapterOnDataSetChanged(object sender, EventArgs eventArgs)
        {
            this.Refill(Adapter);
        }
    }
}