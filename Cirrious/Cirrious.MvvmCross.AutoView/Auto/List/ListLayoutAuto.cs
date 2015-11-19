// ListLayoutAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Lists;
using System.Collections;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.AutoView.Auto.List
{
    public abstract class ListLayoutAuto : KeyedAuto
    {
        public string LayoutName { get; set; }

        protected ListLayoutAuto(string key = null, string onlyFor = null, string notFor = null,
                                 string layoutName = null)
            : base(key, onlyFor, notFor)
        {
            LayoutName = layoutName;
        }

        public sealed override KeyedDescription ToDescription()
        {
            return ToListItemDescription();
        }

        protected virtual void Fill(ListItemLayoutDescription description)
        {
            description.Properties["LayoutName"] = LayoutName ?? "TitleAndSubTitle";
            base.Fill(description);
        }

        public abstract ListItemLayoutDescription ToListItemDescription();
    }

    public class ListLayoutAuto<T> : ListLayoutAuto, IEnumerable<BindingAuto<T>>
    {
        public ListLayoutAuto(string key = null, string onlyFor = null, string notFor = null, string layoutName = null)
            : base(key, onlyFor, notFor, layoutName)
        {
            Bindings = new List<BindingAuto<T>>();
        }

        public List<BindingAuto<T>> Bindings { get; set; }

        public void Add(BindingAuto<T> auto)
        {
            Bindings.Add(auto);
        }

        public override ListItemLayoutDescription ToListItemDescription()
        {
            var toReturn = new ListItemLayoutDescription();
            base.Fill(toReturn);

            var bindings = new Dictionary<string, string>();
            foreach (var bindingAuto in Bindings)
            {
                bindings[bindingAuto.Target] = bindingAuto.GetValueText();
            }
            toReturn.Properties["Bindings"] = bindings;

            return toReturn;
        }

        public IEnumerator<BindingAuto<T>> GetEnumerator()
        {
            return Bindings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}