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
        public MvxBindableListAdapterWithChangedEvent Adapter { get; set; }

        public MvxBindableLinearLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            var itemTemplateId = MvxBindableListViewHelpers.ReadTemplatePath(context, attrs);
            Adapter = new MvxBindableListAdapterWithChangedEvent(context);
            Adapter.ItemTemplateId = itemTemplateId;
            Adapter.DataSetChanged += AdapterOnDataSetChanged;
        }

        private void AdapterOnDataSetChanged(object sender, EventArgs eventArgs)
        {
            this.Refill(Adapter);
        }

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
    }
}