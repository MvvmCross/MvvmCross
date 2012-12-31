#region Copyright

// <copyright file="RootAuto.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;
using CrossUI.Core.Descriptions.Dialog;

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    public class RootAuto : ElementAuto, IEnumerable<SectionAuto>
    {
        public string GroupName { get; set; }
#warning Radio buttons really not done...
        public List<SectionAuto> Sections { get; set; }

        public RootAuto(string groupName = null, string key = null, string caption = null, string onlyFor = null,
                        string notFor = null, Expression<Func<ICommand>> selectedCommand = null)
            : base(key ?? "Root", caption, onlyFor, notFor, selectedCommand)
        {
            GroupName = groupName;
            Sections = new List<SectionAuto>();
        }

        public void Add(SectionAuto section)
        {
            Sections.Add(section);
        }

        public IEnumerator<SectionAuto> GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override ElementDescription ToElementDescription()
        {
            var toReturn = base.ToElementDescription();
            if (!string.IsNullOrEmpty(GroupName))
            {
                toReturn.Group = new GroupDescription {Key = "Radio"};
                toReturn.Group.Properties["Key"] = GroupName;
            }
            foreach (var sectionAuto in Sections)
            {
                var sectionDescription = sectionAuto.ToSectionDescription();
                toReturn.Sections.Add(sectionDescription);
            }
            return toReturn;
        }
    }
}