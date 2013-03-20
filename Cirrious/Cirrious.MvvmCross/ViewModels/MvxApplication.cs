// MvxApplication.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxApplication
        : IMvxApplication
    {
        public IMvxViewModelLocator FindLocator(MvxShowViewModelRequest request)
        {
            return CreateDefaultViewModelLocator();
        }

        protected virtual IMvxViewModelLocator CreateDefaultViewModelLocator()
        {
            return new MvxDefaultViewModelLocator();
        }
    }
}