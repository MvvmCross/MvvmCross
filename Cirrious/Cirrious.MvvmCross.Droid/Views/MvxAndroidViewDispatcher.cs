// MvxAndroidViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewDispatcher
        : MvxAndroidMainThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxAndroidViewPresenter _presenter;

        public MvxAndroidViewDispatcher(IMvxAndroidViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            return RequestMainThreadAction(() => _presenter.Show(request));
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            return RequestMainThreadAction(() => _presenter.ChangePresentation(hint));
        }
    }
}