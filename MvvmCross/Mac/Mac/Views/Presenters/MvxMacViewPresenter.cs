// MvxMacViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


using System.Linq;
using AppKit;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Mac.Views.Presenters
{
    public class MvxMacViewPresenter
        : MvxBaseMacViewPresenter
    {
        public MvxMacViewPresenter(NSApplicationDelegate applicationDelegate, NSWindow window)
        {
            ApplicationDelegate = applicationDelegate;
            Window = window;
        }

        protected virtual NSApplicationDelegate ApplicationDelegate { get; }

        protected virtual NSWindow Window { get; }

        public override void Show(MvxViewModelRequest request)
        {
            var view = CreateView(request);

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
                throw new MvxException("Passed in IMvxMacView is not a UIViewController");

            Show(viewController, request);
        }

        protected virtual void Show(NSViewController viewController, MvxViewModelRequest request)
        {
            while (Window.ContentView.Subviews.Any())
                Window.ContentView.Subviews[0].RemoveFromSuperview();

            Window.ContentView.AddSubview(viewController.View);

            AddLayoutConstraints(viewController, request);
        }

        protected virtual void AddLayoutConstraints(NSViewController viewController, MvxViewModelRequest request)
        {
            var child = viewController.View;
            var container = Window.ContentView;

            // See http://blog.xamarin.com/autolayout-with-xamarin.mac/ for more on constraints
            // as well as https://gist.github.com/garuma/3de3bbeb954ad5679e87 (latter may be helpful as tools...)

            child.TranslatesAutoresizingMaskIntoConstraints = false;
            container.AddConstraints(new[]
                    {NSLayoutAttribute.Left, NSLayoutAttribute.Right, NSLayoutAttribute.Top, NSLayoutAttribute.Bottom}
                .Select(attr => NSLayoutConstraint.Create(child, attr, NSLayoutRelation.Equal, container, attr, 1, 0))
                .ToArray());
        }

        public override void Close(IMvxViewModel toClose)
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