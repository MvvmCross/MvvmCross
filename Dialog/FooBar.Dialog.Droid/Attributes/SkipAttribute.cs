using System;

namespace FooBar.Dialog.Droid
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class SkipAttribute : Attribute
    {
    }
}