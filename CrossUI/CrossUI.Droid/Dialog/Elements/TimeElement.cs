// TimeElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace CrossUI.Droid.Dialog.Elements
{
    public class TimeElement : DateTimeElement
    {
        public TimeElement(string caption = null, DateTime? date = null, string layoutName = null)
            : base(caption, date, layoutName)
        {
            Click = delegate { EditTime(); };
            DateTimeFormat = "t";
        }
    }
}