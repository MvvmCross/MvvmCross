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