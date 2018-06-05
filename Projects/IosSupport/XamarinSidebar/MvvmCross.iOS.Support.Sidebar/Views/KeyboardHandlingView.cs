using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.iOS.Support.Views;
using MvvmCross.iOS.Support.XamarinSidebar;
using MvvmCross.iOS.Support.XamarinSidebarSample.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    [Register("KeyboardHandlingView")]
    [MvxSidebarPresentation(MvxPanelEnum.Center, MvxPanelHintType.PushPanel, true)]
    public class KeyboardHandlingView : MvxBaseViewController<KeyboardHandlingViewModel>
    {
        /// <summary>
        /// Called after the controller’s <see cref="P:UIKit.UIViewController.View"/> is loaded into memory.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called after <c>this</c> <see cref="T:UIKit.UIViewController"/>'s <see cref="P:UIKit.UIViewController.View"/> and its entire view hierarchy have been loaded into memory. This method is called whether the <see cref="T:UIKit.UIView"/> was loaded from a .xib file or programmatically.
        /// </para>
        /// </remarks>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Gray;

            EdgesForExtendedLayout = UIRectEdge.None;

            // setup the keyboard handling
            InitKeyboardHandling();

            var scrollView = new UIScrollView();

            Add(scrollView);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(

                scrollView.AtTopOf(View),
                scrollView.AtLeftOf(View),
                scrollView.WithSameWidth(View),
                scrollView.WithSameHeight(View)

                );

            for(int i = 1; i < 8; i++)
            {
                var textField = new UITextField
                {
                    BorderStyle = UITextBorderStyle.RoundedRect,
                    Placeholder = $"Text Field {i}"
                };

                scrollView.Add(textField);
            }

            scrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            var constraints = scrollView.VerticalStackPanelConstraints(new Margins(15, 15, 15, 15, 5, 80), scrollView.Subviews);
            scrollView.AddConstraints(constraints);
        }

        /// <summary>
        /// Override this and return true to handle the keyboard notifications
        /// </summary>
        /// <returns></returns>
        public override bool HandlesKeyboardNotifications()
        {
            return true;
        }
    }
}