#region Copyright

// <copyright file="MvxSimpleStartApplicationObject.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxSimpleStartApplicationObject<TViewModel>
        : MvxApplicationObject
          , IMvxStartNavigation
        where TViewModel : IMvxViewModel
    {
        public void Start()
        {
            RequestNavigate<TViewModel>();
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return false; }
        }
    }
}