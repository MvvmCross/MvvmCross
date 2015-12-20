// AlignmentAttribute.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace CrossUI.Droid.Dialog.ElementAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class AlignmentAttribute : Attribute
    {
        public AlignmentAttribute(object alignment)
        {
            Alignment = alignment;
        }

        public object Alignment;
    }
}