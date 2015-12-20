// ElementDescription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace CrossUI.Core.Descriptions.Dialog
{
    public class ElementDescription : KeyedDescription
    {
        public GroupDescription Group { get; set; }
        public List<SectionDescription> Sections { get; set; }

        public ElementDescription()
        {
            Sections = new List<SectionDescription>();
        }
    }
}