// DateElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace CrossUI.Droid.Dialog.Elements
{
    public class DateElement : DateTimeElement
    {
        public DateElement(string caption = null, DateTime? date = null, string layoutName = null)
            : base(caption, date, layoutName)
        {
            DateCallback = OnDateSet;
            DateTimeFormat = "MMM d, yyyy";
        }
    }
}