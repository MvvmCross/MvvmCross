// MvxMissingViewFinder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Views
{
    using System;

    using MvvmCross.Core.Views;

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