// ListAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;
using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Lists;

namespace Cirrious.MvvmCross.AutoView.Auto.List
{
    public class ListAuto : KeyedAuto
    {
        public ListAuto(string key = null, string onlyFor = null, string notFor = null,
                        Expression<Func<ICommand>> selectedCommand = null,
                        Expression<Func<IEnumerable>> itemsSource = null, ListLayoutAuto defaultLayout = null)
            : base(key, onlyFor, notFor)
        {
            SelectedCommand = selectedCommand;
            ItemsSource = itemsSource;
            DefaultLayout = defaultLayout;
            ItemLayouts = new Dictionary<string, ListLayoutAuto>();
        }

        public Expression<Func<ICommand>> SelectedCommand { get; set; }
        public Expression<Func<IEnumerable>> ItemsSource { get; set; }
        public ListLayoutAuto DefaultLayout { get; set; }
        public Dictionary<string, ListLayoutAuto> ItemLayouts { get; set; }

        public sealed override KeyedDescription ToDescription()
        {
            return ToListLayoutDescription();
        }

        public virtual ListLayoutDescription ToListLayoutDescription()
        {
            var list = new ListLayoutDescription();
            base.Fill(list);
            if (ItemsSource != null)
            {
                var itemsSource = ItemsSource.GetPropertyText();
                list.Properties["ItemsSource"] = $"@MvxBind:{itemsSource}";
            }
            if (SelectedCommand != null)
            {
                var selectedCommand = SelectedCommand.GetPropertyText();
                list.Properties["ItemClick"] = $"@MvxBind:{selectedCommand}";
            }
            if (DefaultLayout != null)
            {
                list.DefaultLayout = DefaultLayout.ToListItemDescription();
            }
            foreach (var kvp in ItemLayouts)
            {
                list.ItemLayouts[kvp.Key] = kvp.Value.ToListItemDescription();
            }
            return list;
        }
    }
}