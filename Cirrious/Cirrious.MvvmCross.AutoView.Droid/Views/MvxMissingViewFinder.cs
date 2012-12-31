#region Copyright

// <copyright file="MvxMissingViewFinder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Droid.Views
{
    public class MvxMissingViewFinder : IMvxViewFinder
    {
        public Type MissingViewType { get; set; }

        public MvxMissingViewFinder()
        {
            MissingViewType = typeof (MvxMissingActivityView);
        }

        public Type GetViewType(Type viewModelType)
        {
            return MissingViewType;
        }
    }
}