using System;

namespace FooBar.Dialog.Droid
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class CaptionAttribute : Attribute
    {
        public string Caption;

        public CaptionAttribute(string caption)
        {
            Caption = caption;
        }
    }
}