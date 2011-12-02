#region Copyright

// <copyright file="MvxViewModel.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;

namespace Cirrious.MvvmCross.ViewModel
{
    public class MvxViewModel 
        : MvxApplicationObject
        , IMvxViewModel
    {
        protected MvxViewModel()
        {
            // nothing to do currently
        }

        public virtual void RequestStop()
        {
            // default behaviour does nothing!
        }
    }
}