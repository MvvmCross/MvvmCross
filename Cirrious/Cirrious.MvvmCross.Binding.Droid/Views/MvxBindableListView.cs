#region Copyright
// <copyright file="MvxBindableListView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Attributes;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBindableListView
        : ListView
    {
        public MvxBindableListView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxBindableListAdapter(context))
        {
        }

        public MvxBindableListView(Context context, IAttributeSet attrs, MvxBindableListAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxBindableListViewHelpers.ReadAttributeValue(context, attrs, MvxAndroidBindingResource.Instance.BindableListViewStylableGroupId, MvxAndroidBindingResource.Instance.BindableListItemTemplateId);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
            SetupItemClickListeners();            
        }

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

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return Adapter.ItemsSource; }
            set { Adapter.ItemsSource = value; }
        }
        
        public int ItemTemplateId
        {
            get { return Adapter.ItemTemplateId; }
            set { Adapter.ItemTemplateId = value; }
        }

        public new ICommand ItemClick { get; set; }

        public new ICommand ItemLongClick { get; set; }

        protected void SetupItemClickListeners()
        {
            base.ItemClick += (sender, args) => ExecuteCommandOnItem(this.ItemClick, args.Position);
            base.ItemLongClick += (sender, args) => ExecuteCommandOnItem(this.ItemLongClick, args.Position);
        }

        protected void ExecuteCommandOnItem(ICommand command, int position)
        {
            if (command == null)
                return;

            var item = Adapter.GetItem(position) as MvxJavaContainer;
            if (item == null)
                return;

            if (item.Object == null)
                return;

            if (!command.CanExecute(item.Object))
                return;

            command.Execute(item.Object);
        }
    }
}