// MvxStoreSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows.Controls;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Silverlight.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Silverlight.Platform
{
    public abstract class MvxSilverlightSetup
        : MvxSetup
    {
        private readonly Frame _rootFrame;

        protected MvxSilverlightSetup(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }
        
        protected override IMvxPluginManager CreatePluginManager()
        {
            //TODO Silverlight - Verify resolving of plugins
            return new MvxFilePluginManager(".WindowsSilverlight");
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            return CreateSilverlightViewsContainer();
        }

        protected virtual IMvxSilverlightViewsContainer CreateSilverlightViewsContainer()
        {
            var container = new MvxSilverlightViewsContainer();
            Mvx.RegisterSingleton<IMvxSilverlightViewModelRequestTranslator>(container);
            return container;
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return CreateViewDispatcher(_rootFrame);
        }

        protected virtual IMvxSilverlightViewPresenter CreateViewPresenter(Frame rootFrame)
        {
            return new MvxSilverlightViewPresenter(rootFrame);
        }

        protected virtual MvxSilverlightViewDispatcher CreateViewDispatcher(Frame rootFrame)
        {
            var presenter = CreateViewPresenter(_rootFrame);
            return new MvxSilverlightViewDispatcher(presenter, rootFrame);
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }
    }
}