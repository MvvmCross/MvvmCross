// MvxApplication.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Application
{
    public abstract class MvxApplication
        : IMvxViewModelLocatorFinder
    {
        #region IMvxViewModelLocatorFinder Members

        public IMvxViewModelLocator FindLocator(MvxShowViewModelRequest request)
        {
            return CreateDefaultViewModelLocator();
        }

        protected virtual IMvxViewModelLocator CreateDefaultViewModelLocator()
        {
            return new MvxDefaultViewModelLocator();
        }

        #endregion
    }
}