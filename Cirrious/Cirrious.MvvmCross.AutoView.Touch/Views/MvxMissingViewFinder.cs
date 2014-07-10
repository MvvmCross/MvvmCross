// MvxMissingViewFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    public class MvxMissingViewFinder : IMvxViewFinder
    {
        public Type MissingViewType { get; set; }

        public MvxMissingViewFinder()
        {
            MissingViewType = typeof (MvxMissingViewController);
        }

        public Type GetViewType(Type viewModelType)
        {
            return MissingViewType;
        }
    }
}