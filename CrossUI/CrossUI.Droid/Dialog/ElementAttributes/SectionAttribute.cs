// SectionAttribute.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace CrossUI.Droid.Dialog.ElementAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class SectionAttribute : Attribute
    {
        public string Caption, Footer;

        public SectionAttribute()
        {
        }

        public SectionAttribute(string caption)
        {
            Caption = caption;
        }

        public SectionAttribute(string caption, string footer)
        {
            Caption = caption;
            Footer = footer;
        }
    }
}