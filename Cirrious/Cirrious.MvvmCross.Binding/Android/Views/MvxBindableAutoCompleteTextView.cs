using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.Content;
using Android.Util;
using Android.Widget;
using Android.Runtime;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableAutoCompleteTextView<T>
        : AutoCompleteTextView
    {
        public MvxBindableAutoCompleteTextView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxBindableArrayAdapter<T>(context))
        {
        }

        public MvxBindableAutoCompleteTextView(Context context, IAttributeSet attrs, MvxBindableArrayAdapter<T> adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxBindableListViewHelpers.ReadTemplatePath(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
            SetupItemClickListener();
        }

        public new MvxBindableArrayAdapter<T> Adapter
        {
            get { return base.Adapter as MvxBindableArrayAdapter<T>; }
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


        public IList<T> ItemsSource
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
    }
}