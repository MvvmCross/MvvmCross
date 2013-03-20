// MvxAndroidViewDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        private readonly IMvxAndroidViewPresenter _presenter;

        public MvxAndroidViewDispatcherProvider(IMvxAndroidViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public virtual IMvxViewDispatcher ViewDispatcher
        {
            get
            {
                var topActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
                if (topActivity == null)
                {
                    MvxTrace.Warning(
                                   "No top level activity available - so UI threaded messages will not make it through");
                }
                return new MvxAndroidViewDispatcher(topActivity, _presenter);
            }
        }
    }
}