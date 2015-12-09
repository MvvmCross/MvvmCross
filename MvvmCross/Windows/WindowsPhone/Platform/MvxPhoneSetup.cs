// MvxPhoneSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Platform
{
    using Microsoft.Phone.Controls;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;
    using MvvmCross.WindowsPhone.Views;

    public abstract class MvxPhoneSetup
        : MvxSetup
    {
        private readonly PhoneApplicationFrame _rootFrame;

        protected MvxPhoneSetup(PhoneApplicationFrame rootFrame)
        {
            this._rootFrame = rootFrame;
        }

        protected override void InitializeCommandHelper()
        {
            // WindowsPhone has to have a strong command helper
            // - see https://github.com/MvvmCross/MvvmCross/issues/623
            Mvx.RegisterType<IMvxCommandHelper, MvxStrongCommandHelper>();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override IMvxViewsContainer CreateViewsContainer()
        {
            var container = this.CreateViewsContainer(this._rootFrame);
            Mvx.RegisterSingleton<IMvxPhoneViewModelRequestTranslator>(container);
            return container;
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return this.CreateViewDispatcher(this._rootFrame);
        }

        protected virtual IMvxPhoneViewPresenter CreateViewPresenter(PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewPresenter(rootFrame);
        }

        protected virtual IMvxViewDispatcher CreateViewDispatcher(PhoneApplicationFrame rootFrame)
        {
            var presenter = this.CreateViewPresenter(rootFrame);
            return this.CreateViewDispatcher(presenter, rootFrame);
        }

        protected virtual MvxPhoneViewDispatcher CreateViewDispatcher(IMvxPhoneViewPresenter presenter,
                                                                                      PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewDispatcher(presenter, rootFrame);
        }

        protected virtual IMvxPhoneViewsContainer CreateViewsContainer(PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewsContainer();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxFilePluginManager(".WindowsPhone");
            return toReturn;
        }

        protected override void InitializePlatformServices()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(new MvxPhoneLifetimeMonitor());
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }
    }
}