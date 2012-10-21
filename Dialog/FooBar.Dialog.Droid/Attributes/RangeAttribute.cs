using System;

namespace FooBar.Dialog.Droid
{
    public class RangeAttribute : Attribute
    {
        public int High;
        public int Low;
        public bool ShowCaption;

        public RangeAttribute(int low, int high)
        {
            Low = low;
            High = high;
        }
    }
}