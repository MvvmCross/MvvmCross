// ListLayoutAuto.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto.List
{
    using System.Collections;
    using System.Collections.Generic;

    using CrossUI.Core.Descriptions;
    using CrossUI.Core.Descriptions.Lists;

    public abstract class ListLayoutAuto : KeyedAuto
    {
        public string LayoutName { get; set; }

        protected ListLayoutAuto(string key = null, string onlyFor = null, string notFor = null,
                                 string layoutName = null)
            : base(key, onlyFor, notFor)
        {
            this.LayoutName = layoutName;
        }

        public sealed override KeyedDescription ToDescription()
        {
            return this.ToListItemDescription();
        }

        protected virtual void Fill(ListItemLayoutDescription description)
        {
            description.Properties["LayoutName"] = this.LayoutName ?? "TitleAndSubTitle";
            base.Fill(description);
        }

        public abstract ListItemLayoutDescription ToListItemDescription();
    }

    public class ListLayoutAuto<T> : ListLayoutAuto, IEnumerable<BindingAuto<T>>
    {
        public ListLayoutAuto(string key = null, string onlyFor = null, string notFor = null, string layoutName = null)
            : base(key, onlyFor, notFor, layoutName)
        {
            this.Bindings = new List<BindingAuto<T>>();
        }

        public List<BindingAuto<T>> Bindings { get; set; }

        public void Add(BindingAuto<T> auto)
        {
            this.Bindings.Add(auto);
        }

        public override ListItemLayoutDescription ToListItemDescription()
        {
            var toReturn = new ListItemLayoutDescription();
            base.Fill(toReturn);

            var bindings = new Dictionary<string, string>();
            foreach (var bindingAuto in this.Bindings)
            {
                bindings[bindingAuto.Target] = bindingAuto.GetValueText();
            }
            toReturn.Properties["Bindings"] = bindings;

            return toReturn;
        }

        public IEnumerator<BindingAuto<T>> GetEnumerator()
        {
            return this.Bindings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}