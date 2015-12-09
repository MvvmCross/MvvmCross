// MvxMissingViewFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Views
{
    using System;

    public class MvxMissingViewFinder : IMvxViewFinder
    {
        public Type MissingViewType { get; set; }

        public MvxMissingViewFinder()
        {
            this.MissingViewType = typeof(MvxMissingViewController);
        }

        public Type GetViewType(Type viewModelType)
        {
            return this.MissingViewType;
        }
    }
}