using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;
using Foobar.Dialog.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Auto.List
{
    public class ListAuto : KeyedAuto
    {
        public ListAuto(string key = null, string onlyFor = null, string notFor = null, Expression<Func<ICommand>> selectedCommand = null, Expression<Func<IEnumerable>> itemsSource = null, ListLayoutAuto defaultLayout = null)
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
                list.Properties["ItemsSource"] = string.Format("@MvxBind:{{'Path':'{0}'}}", itemsSource);
            }
            if (SelectedCommand != null)
            {
                var selectedCommand = SelectedCommand.GetPropertyText();
                list.Properties["ItemClick"] = string.Format("@MvxBind:{{'Path':'{0}'}}", selectedCommand);
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