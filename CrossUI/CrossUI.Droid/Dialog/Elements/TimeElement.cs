// TimeElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace CrossUI.Droid.Dialog.Elements
{
    public class TimeElement : DateTimeElement
    {
        public TimeElement(string caption, DateTime? date, string layoutName = null)
            : base(caption, date, layoutName)
        {
            Click = delegate { EditTime(); };
        }

        protected override string Format(DateTime dt)
        {
            return dt.ToShortTimeString();
        }
    }
}