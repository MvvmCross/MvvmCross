// ParentMenuAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Menu;
using System.Collections;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.AutoView.Auto.Menu
{
    public class ParentMenuAuto : KeyedAuto, IEnumerable<MenuAuto>
    {
        public List<MenuAuto> Children { get; set; }

        public ParentMenuAuto(string key = null, string onlyFor = null, string notFor = null)
            : base(key ?? "Root", onlyFor, notFor)
        {
            Children = new List<MenuAuto>();
        }

        public sealed override KeyedDescription ToDescription()
        {
            return ToParentMenuDescription();
        }

        public virtual ParentMenuDescription ToParentMenuDescription()
        {
            var toReturn = new ParentMenuDescription();
            base.Fill(toReturn);
            foreach (var menuAuto in Children)
            {
                var menuDescription = menuAuto.ToMenuDescription();
                toReturn.Children.Add(menuDescription);
            }
            return toReturn;
        }

        public void Add(MenuAuto menuAuto)
        {
            Children.Add(menuAuto);
        }

        public IEnumerator<MenuAuto> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}