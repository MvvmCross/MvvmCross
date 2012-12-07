using System;

namespace CrossUI.Droid.Dialog.ElementAttributes
{
#warning Are these used?
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class EntryAttribute : Attribute
    {
        public string Placeholder;

        public EntryAttribute()
            : this(null)
        {
        }

        public EntryAttribute(string placeholder)
        {
            Placeholder = placeholder;
        }
    }
}