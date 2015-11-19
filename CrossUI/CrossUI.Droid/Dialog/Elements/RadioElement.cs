// RadioElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Views;
using System;

namespace CrossUI.Droid.Dialog.Elements
{
    public class RadioElement
        : StringElement
        , IRadioElement
    {
        public string Group { get; set; }
        internal int RadioIdx;

        public RadioElement(string caption = null, string group = null)
            : base(caption)
        {
            Group = group;
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            if (!(((RootElement)Parent.Parent).Group is RadioGroup))
                throw new Exception("The RootElement's Group is null or is not a RadioGroup");

            return base.GetViewImpl(context, parent);
        }

        public override string Summary()
        {
            return Caption;
        }
    }
}