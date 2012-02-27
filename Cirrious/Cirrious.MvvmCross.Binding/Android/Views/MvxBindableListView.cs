using System.Collections;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Interfaces.Views;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableListView
        : ListView
    {
        public new MvxBindableListAdapter Adapter
        {
            get { return base.Adapter as MvxBindableListAdapter; }
            set
            {
                var existing = Adapter;
                if (existing == value)
                    return;

                if (existing != null && value != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;
                }

                base.Adapter = value;
            }
        }

        public MvxBindableListView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxBindableListAdapter(context))
        {
        }

        public MvxBindableListView(Context context, IAttributeSet attrs, MvxBindableListAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxBindableListViewHelpers.ReadTemplatePath(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
            SetupItemClickListener();
        }

        private void SetupItemClickListener()
        {
            base.ItemClick += (sender, args) =>
            {
                if (this.ItemClick == null)
                    return;
                var item = Adapter.GetItem(args.Position) as MvxJavaContainer;
                if (item == null)
                    return;

                if (item.Object == null)
                    return;

                if (!this.ItemClick.CanExecute(item.Object))
                    return;

                this.ItemClick.Execute(item.Object);
            };
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

        public new IMvxCommand ItemClick { get; set; }
    }
}