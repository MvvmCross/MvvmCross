// IMvxFragmentsPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.FullFragging.Presenter
{
    public interface IMvxFragmentsPresenter
    {
        void RegisterViewModelAtHost<TViewModel>(IMvxFragmentHost host) 
            where TViewModel : IMvxViewModel;

        void UnRegisterViewModelAtHost<TViewModel>() 
            where TViewModel : IMvxViewModel;
    }
}