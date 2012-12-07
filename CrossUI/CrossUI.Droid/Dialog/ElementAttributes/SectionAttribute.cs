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