// MvxMissingViewFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Views;
using System;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    public class MvxMissingViewFinder : IMvxViewFinder
    {
        public Type MissingViewType { get; set; }

        public MvxMissingViewFinder()
        {
            MissingViewType = typeof(MvxMissingViewController);
        }

        public Type GetViewType(Type viewModelType)
        {
            return MissingViewType;
        }
    }
}