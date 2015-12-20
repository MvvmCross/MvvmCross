// MvxTouchViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
#else
#endif

namespace MvvmCross.Mac.Views.Presenters
{
    using global::MvvmCross.Core.ViewModels;
    using global::MvvmCross.Platform;
    using global::MvvmCross.Platform.Exceptions;

    public class MvxMacViewPresenter
        : MvxBaseMacViewPresenter
    {
        private readonly NSApplicationDelegate _applicationDelegate;
        private readonly NSWindow _window;

        protected virtual NSApplicationDelegate ApplicationDelegate
        {
            get
            {
                return this._applicationDelegate;
            }
        }

        protected virtual NSWindow Window
        {
            get
            {
                return this._window;
            }
        }

        public MvxMacViewPresenter(NSApplicationDelegate applicationDelegate, NSWindow window)
        {
            this._applicationDelegate = applicationDelegate;
            this._window = window;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateView(request);

            this.Show(view, request);
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxClosePresentationHint)
            {
                this.Close((hint as MvxClosePresentationHint).ViewModelToClose);
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

            this.Show(viewController, request);
        }

        protected virtual void Show(NSViewController viewController, MvxViewModelRequest request)
        {
            while (this.Window.ContentView.Subviews.Any())
            {
                this.Window.ContentView.Subviews[0].RemoveFromSuperview();
            }

            this.Window.ContentView.AddSubview(viewController.View);

            this.AddLayoutConstraints(viewController, request);
        }

        protected virtual void AddLayoutConstraints(NSViewController viewController, MvxViewModelRequest request)
        {
            var child = viewController.View;
            var container = this.Window.ContentView;

            // See http://blog.xamarin.com/autolayout-with-xamarin.mac/ for more on constraints
            // as well as https://gist.github.com/garuma/3de3bbeb954ad5679e87 (latter maybe helpful as tools...)

            child.TranslatesAutoresizingMaskIntoConstraints = false;
            container.AddConstraints(new[] {
                NSLayoutConstraint.Create (child, NSLayoutAttribute.Left, NSLayoutRelation.Equal, container, NSLayoutAttribute.Left, 1, 0),
                NSLayoutConstraint.Create (child, NSLayoutAttribute.Right, NSLayoutRelation.Equal, container, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create (child, NSLayoutAttribute.Top, NSLayoutRelation.Equal, container, NSLayoutAttribute.Top, 1, 0),
                NSLayoutConstraint.Create (child, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, container, NSLayoutAttribute.Bottom, 1, 0),
            });
        }

        public virtual void Close(IMvxViewModel toClose)
        {
            Mvx.Error("Sorry - don't know how to close a view!");

            /*
			 * this code won't work - it needs view controllers rather than views :/
			foreach (var subview in Window.ContentView.Subviews) {
				var mvxView = subview as IMvxMacView;
				if (mvxView == null)
					continue;

				if (mvxView.ViewModel != toClose)
					continue;

				subview.RemoveFromSuperview ();
				return;
			}
			*/
        }
    }
}