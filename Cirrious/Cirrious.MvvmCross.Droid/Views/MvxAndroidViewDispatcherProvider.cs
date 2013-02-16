// MvxAndroidViewDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewDispatcherProvider
        : IMvxViewDispatcherProvider
          , IMvxServiceConsumer
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
			    var topActivity = this.GetService<IMvxAndroidCurrentTopActivity>().Activity;
                if (topActivity == null)
                {
                    MvxTrace.Trace(MvxTraceLevel.Warning, "No top level activity available - so UI threaded messages will not make it through");
                }
			    return new MvxAndroidViewDispatcher(topActivity, _presenter);
			}
        }
    }
}