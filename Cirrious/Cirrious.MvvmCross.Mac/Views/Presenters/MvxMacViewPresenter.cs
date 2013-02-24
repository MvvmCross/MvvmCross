// MvxMacViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxMacViewPresenter
        : MvxBaseMacViewPresenter
          
    {
        private readonly NSApplicationDelegate _applicationDelegate;

        public MvxMacViewPresenter(NSApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        }

        public override void Show(MvxShowViewModelRequest request)
        {
            var view = CreateView(request);

            if (request.ClearTop)
                ClearBackStack();

            Show(view);
        }

        private IMvxMacView CreateView(MvxShowViewModelRequest request)
        {
            return Mvx.Resolve<IMvxMacViewCreator>().CreateView(request);
        }

        public virtual void Show(IMvxMacView view)
        {
            var viewController = view as NSWindowController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxTouchView is not a UIViewController");

            viewController.Window.MakeKeyAndOrderFront(_applicationDelegate);
        }

        public override void ClearBackStack()
        {
            // ? TODO
        }
    }
}