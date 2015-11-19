// RootAuto.cs
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
                toReturn.Group = new GroupDescription
                {
                    Key = "Radio",
                    Properties = {["Key"] = GroupName}
                };
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