// Group.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Elements.Dialog;

namespace CrossUI.Droid.Dialog.Elements
{
    /// <summary>
    /// Used by root elements to fetch information when they need to
    /// render a summary (Checkbox count or selected radio group).
    /// </summary>
    public class Group : IGroup
    {
        public string Key { get; set; }

        protected Group()
        {
        }

        public Group(string key)
        {
            Key = key;
        }
    }
}