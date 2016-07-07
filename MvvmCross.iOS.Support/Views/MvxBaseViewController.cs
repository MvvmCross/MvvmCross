namespace MvvmCross.iOS.Support.Views
{
	using System;
	using Core.ViewModels;
	using CoreGraphics;
	using Foundation;
	using iOS.Views;
	using UIKit;

	/// <summary>
	/// Mvx base view controller that provides a few extra bits of implementation over the standard View Controllers.
	/// </summary>
	public abstract class MvxBaseViewController<TViewModel> : MvxViewController where TViewModel : MvxViewModel
    {
		public MvxBaseViewController()
		{
		}

		public MvxBaseViewController(IntPtr handle) : base(handle)
        {
		}

		protected MvxBaseViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
		}

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        protected new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        /// <summary>
        /// The view to center on keyboard shown
        /// </summary>
        protected UIView ViewToCenterOnKeyboardShown;

        /// <summary>
        /// The scroll to center on keyboard shown
        /// </summary>
        protected UIScrollView ScrollToCenterOnKeyboardShown;

        /// <summary>
		/// Initialises the keyboard handling.  The view must also contain a UIScrollView for this to work.  You must also override HandlesKeyboardNotifications() and return true from that method.
		/// </summary>
		/// <param name="enableAutoDismiss">If set to <c>true</c> enable auto dismiss.</param>
		protected virtual void InitKeyboardHandling(bool enableAutoDismiss = true)
        {
            //Only do this if required
            if (HandlesKeyboardNotifications())
            {
                RegisterForKeyboardNotifications();
            }

            if (enableAutoDismiss)
            {
                DismissKeyboardOnBackgroundTap();
            }
        }

        /// <summary>
        /// Override this in derived Views in order to handle the keyboard.
        /// </summary>
        /// <returns></returns>
        public virtual bool HandlesKeyboardNotifications()
        {
            return false;
        }

        private NSObject _keyboardShowObserver;
        private NSObject _keyboardHideObserver;

        protected virtual void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
            {
                _keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
            }

            if (_keyboardHideObserver == null)
            {
                _keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
            }
        }

        protected virtual void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardShowObserver);
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardHideObserver);
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }

        /// <summary>
        /// Gets the UIView that represents the "active" user input control (e.g. textfield, or button under a text field)
        /// </summary>
        /// <returns>
        /// A <see cref="UIView"/>
        /// </returns>
        protected virtual UIView KeyboardGetActiveView()
        {
            return View.FindFirstResponder();
        }

        /// <summary>
        /// Called when keyboard notifications are produced.
        /// </summary>
        /// <param name="notification">The notification.</param>
        private void OnKeyboardNotification(NSNotification notification)
        {
            if (!IsViewLoaded) return;

            //Check if the keyboard is becoming visible
            var visible = notification.Name == UIKeyboard.WillShowNotification;

            //Start an animation, using values from the keyboard
            UIView.BeginAnimations("AnimateForKeyboard");
            UIView.SetAnimationBeginsFromCurrentState(true);
            UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
            UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

            //Pass the notification, calculating keyboard height, etc.
            var keyboardFrame = visible
                ? UIKeyboard.FrameEndFromNotification(notification)
                : UIKeyboard.FrameBeginFromNotification(notification);

            OnKeyboardChanged(visible, keyboardFrame);

            //Commit the animation
            UIView.CommitAnimations();
        }

        /// <summary>
        /// Override this method to apply custom logic when the keyboard is shown/hidden
        /// </summary>
        /// <param name='visible'>
        /// If the keyboard is visible
        /// </param>
        /// <param name='keyboardFrame'>
        /// Frame of the keyboard
        /// </param>
        protected virtual void OnKeyboardChanged(bool visible, CGRect keyboardFrame)
        {
            var activeView = ViewToCenterOnKeyboardShown ?? KeyboardGetActiveView();
            if (activeView == null)
            {
                return;
            }

            var scrollView = ScrollToCenterOnKeyboardShown ?? activeView.FindTopSuperviewOfType(View, typeof(UIScrollView)) as UIScrollView;
            if (scrollView == null)
            {
                return;
            }

            if (!visible)
            {
                scrollView.RestoreScrollPosition();
            }
            else
            {
                scrollView.CenterView(activeView, keyboardFrame);
            }
        }

        /// <summary>
        /// Call it to force dismiss keyboard when background is tapped
        /// </summary>
        protected void DismissKeyboardOnBackgroundTap()
        {
            // Add gesture recognizer to hide keyboard
            var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
            tap.AddTarget(() => View.EndEditing(true));
            tap.ShouldReceiveTouch = (recognizer, touch) => !(touch.View is UIControl || touch.View.FindSuperviewOfType(View, typeof(UITableViewCell)) != null);
            View.AddGestureRecognizer(tap);
        }


		/// <summary>
		/// Selects next TextField to become FirstResponder.
		/// Usage: textField.ShouldReturn += TextFieldShouldReturn;
		/// </summary>
		/// <returns></returns>
		/// <param name="textField">The TextField</param>
		public bool TextFieldShouldReturn(UITextField textField)
		{
			var nextTag = textField.Tag + 1;
			UIResponder nextResponder = this.View.ViewWithTag(nextTag);
			if (nextResponder != null)
			{
				nextResponder.BecomeFirstResponder();
			}
			else {
				// Not found, so remove keyboard.
				textField.ResignFirstResponder();
			}
			return false; // We do not want UITextField to insert line-breaks.
		}
    }
}