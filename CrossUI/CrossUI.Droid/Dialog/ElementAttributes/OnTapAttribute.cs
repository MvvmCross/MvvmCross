using System;

namespace CrossUI.Droid.Dialog.ElementAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class OnTapAttribute : Attribute
    {
        public string Method;

        public OnTapAttribute(string method)
        {
            Method = method;
        }
    }
}