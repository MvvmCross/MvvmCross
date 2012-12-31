#region Copyright

// <copyright file="ParentMenuAuto.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Collections;
using System.Collections.Generic;
using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Menu;

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

        public override sealed KeyedDescription ToDescription()
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