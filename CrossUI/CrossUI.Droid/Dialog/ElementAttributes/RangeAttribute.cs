// RangeAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace CrossUI.Droid.Dialog.ElementAttributes
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