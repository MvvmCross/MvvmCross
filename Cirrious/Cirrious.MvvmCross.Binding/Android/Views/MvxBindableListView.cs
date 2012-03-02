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
using Android.Content;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.MvvmCross.Binding.Android.Views
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
            var itemTemplateId = MvxBindableListViewHelpers.ReadTemplatePath(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            Adapter = adapter;
            SetupItemClickListener();
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