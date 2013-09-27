// MvxTouchViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MonoMac.AppKit;
using System.Drawing;

namespace Cirrious.MvvmCross.Mac.Views.Presenters
{
    public class MvxMacViewPresenter
        : MvxBaseMacViewPresenter
    {
        private readonly NSApplicationDelegate _applicationDelegate;
        private readonly NSWindow _window;

		protected virtual NSApplicationDelegate ApplicationDelegate{
			get{
				return _applicationDelegate;
			}
		}

		protected virtual NSWindow Window{
			get{
				return _window;
			}
		}

        public MvxMacViewPresenter(NSApplicationDelegate applicationDelegate, NSWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = CreateView(request);

#warning Need to reinsert ClearTop type functionality here
            //if (request.ClearTop)
            //    ClearBackStack();

            Show(view, request);
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }

            base.ChangePresentation(hint);
        }

        private IMvxMacView CreateView(MvxViewModelRequest request)
        {
            return Mvx.Resolve<IMvxMacViewCreator>().CreateView(request);
        }

        protected virtual void Show(IMvxMacView view, MvxViewModelRequest request)
        {
            var viewController = view as NSViewController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxTouchView is not a UIViewController");

			Show (viewController, request);
        }

		protected virtual void Show(NSViewController viewController, MvxViewModelRequest request)
		{
			var cv = Window.ContentView;
			cv.AutoresizesSubviews = true;

			var v = viewController.View;
			var rect = v.Frame;
			v.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;

			var tmp = new NSView (rect);
			tmp.AddSubview (v);
			tmp.AutoresizesSubviews = true;
			tmp.Frame = Window.ContentView.Frame;
			v.RemoveFromSuperview ();

			cv.AddSubview (v);
		}


		public virtual void Close(IMvxViewModel toClose)
        {
			Mvx.Error("Sorry - don't know how to close a view!");
        }
    }
}