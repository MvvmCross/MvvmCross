// ListAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto.List
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Windows.Input;

    using CrossUI.Core.Descriptions;
    using CrossUI.Core.Descriptions.Lists;

    public class ListAuto : KeyedAuto
    {
        public ListAuto(string key = null, string onlyFor = null, string notFor = null,
                        Expression<Func<ICommand>> selectedCommand = null,
                        Expression<Func<IEnumerable>> itemsSource = null, ListLayoutAuto defaultLayout = null)
            : base(key, onlyFor, notFor)
        {
            this.SelectedCommand = selectedCommand;
            this.ItemsSource = itemsSource;
            this.DefaultLayout = defaultLayout;
            this.ItemLayouts = new Dictionary<string, ListLayoutAuto>();
        }

        public Expression<Func<ICommand>> SelectedCommand { get; set; }
        public Expression<Func<IEnumerable>> ItemsSource { get; set; }
        public ListLayoutAuto DefaultLayout { get; set; }
        public Dictionary<string, ListLayoutAuto> ItemLayouts { get; set; }

        public sealed override KeyedDescription ToDescription()
        {
            return this.ToListLayoutDescription();
        }

        public virtual ListLayoutDescription ToListLayoutDescription()
        {
            var list = new ListLayoutDescription();
            base.Fill(list);
            if (this.ItemsSource != null)
            {
                var itemsSource = this.ItemsSource.GetPropertyText();
                list.Properties["ItemsSource"] = $"@MvxBind:{itemsSource}";
            }
            if (this.SelectedCommand != null)
            {
                var selectedCommand = this.SelectedCommand.GetPropertyText();
                list.Properties["ItemClick"] = $"@MvxBind:{selectedCommand}";
            }
            if (this.DefaultLayout != null)
            {
                list.DefaultLayout = this.DefaultLayout.ToListItemDescription();
            }
            foreach (var kvp in this.ItemLayouts)
            {
                list.ItemLayouts[kvp.Key] = kvp.Value.ToListItemDescription();
            }
            return list;
        }
    }
}