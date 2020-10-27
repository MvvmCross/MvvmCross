﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    /// <summary>
	/// Mvx base view controller that provides a few extra bits of implementation over the standard View Controllers.
	/// </summary>
	public abstract class MvxBaseViewController<TViewModel> : MvxViewController where TViewModel : IMvxViewModel
    {
        private readonly MvxIosMajorVersionChecker _iosVersion11Checker = new MvxIosMajorVersionChecker(11);
    
        public MvxBaseViewController()
        {
        }

        public MvxBaseViewController(NSCoder coder) : base(coder)
        {
        }

        protected MvxBaseViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxBaseViewController(IntPtr handle) : base(handle)
        {
        }

        public MvxBaseViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
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

        private CGRect _lastKeyboardFrame = CGRect.Empty;
        [Weak] private UIView _lastActiveView;

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
                _lastKeyboardFrame = CGRect.Empty;
                _lastActiveView = null;
                return;
            }

            var scrollView = ScrollToCenterOnKeyboardShown ?? activeView.FindTopSuperviewOfType(View, typeof(UIScrollView)) as UIScrollView;
            if (scrollView == null)
            {
                _lastKeyboardFrame = CGRect.Empty;
                _lastActiveView = null;
                return;
            }

            if (!visible)
            {
                _lastKeyboardFrame = CGRect.Empty;
                _lastActiveView = null;
                scrollView.RestoreScrollPosition();
            }
            else
            {
                //avoid recalculation if the activeView is the same.
                if (_lastKeyboardFrame == keyboardFrame &&
                    _lastActiveView?.Equals(activeView) == true)
                {
                    return;
                }

                _lastKeyboardFrame = keyboardFrame;
                _lastActiveView = activeView;
                if (_iosVersion11Checker.IsVersionOrHigher)
                    keyboardFrame.Height -= scrollView.SafeAreaInsets.Bottom;
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
			UIResponder nextResponder = View.ViewWithTag(nextTag);
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
