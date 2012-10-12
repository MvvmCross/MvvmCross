using System;

namespace Cirrious.MvvmCross.Binding.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MvxSetToNullAfterBindingAttribute : Attribute
    {
    }
}