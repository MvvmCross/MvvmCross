// ParentMenuAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto.Menu
{
    using System.Collections;
    using System.Collections.Generic;

    using CrossUI.Core.Descriptions;
    using CrossUI.Core.Descriptions.Menu;

    public class ParentMenuAuto : KeyedAuto, IEnumerable<MenuAuto>
    {
        public List<MenuAuto> Children { get; set; }

        public ParentMenuAuto(string key = null, string onlyFor = null, string notFor = null)
            : base(key ?? "Root", onlyFor, notFor)
        {
            this.Children = new List<MenuAuto>();
        }

        public sealed override KeyedDescription ToDescription()
        {
            return this.ToParentMenuDescription();
        }

        public virtual ParentMenuDescription ToParentMenuDescription()
        {
            var toReturn = new ParentMenuDescription();
            base.Fill(toReturn);
            foreach (var menuAuto in this.Children)
            {
                var menuDescription = menuAuto.ToMenuDescription();
                toReturn.Children.Add(menuDescription);
            }
            return toReturn;
        }

        public void Add(MenuAuto menuAuto)
        {
            this.Children.Add(menuAuto);
        }

        public IEnumerator<MenuAuto> GetEnumerator()
        {
            return this.Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}