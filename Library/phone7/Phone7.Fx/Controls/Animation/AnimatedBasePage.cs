using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Phone7.Fx.Controls.Animations;
using Phone7.Fx.Extensions;
using System.Diagnostics;
using System.Windows.Controls;

namespace Phone7.Fx.Controls
{
    public class AnimatedBasePage : PhoneApplicationPage
    {
        private static readonly Uri ExternalUri = new Uri(@"app://external/");

        public static readonly DependencyProperty AnimationContextProperty = DependencyProperty.Register("AnimationContext", typeof(FrameworkElement), typeof(AnimatedBasePage), new PropertyMetadata(null));
        public FrameworkElement AnimationContext
        {
            get
            {
                return (FrameworkElement)GetValue(AnimationContextProperty);
            }
            set
            {
                SetValue(AnimationContextProperty, value);
            }
        }

        private static Uri _fromUri;

        private bool _isAnimating;
        private static bool _isNavigating;
        private bool _needsOutroAnimation;
        private Uri _nextUri;
        private Uri _arrivedFromUri;
        private AnimationType _currentAnimationType;
        private NavigationMode? _currentNavigationMode;
        private bool _isActive;
        private bool _isForwardNavigation;
        private bool _loadingAndAnimatingIn;

        public AnimatedBasePage()
            : base()
        {
            _isActive = true;

            _isForwardNavigation = true;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (_isNavigating)
            {
                e.Cancel = true;
                return;
            }

            if (!CanAnimate())
                return;

            if (_isAnimating)
            {
                e.Cancel = true;
                return;
            }

            if (_loadingAndAnimatingIn)
            {
                e.Cancel = true;
                return;
            }

            if (!this.NavigationService.CanGoBack)
                return;

            if (!IsPopupOpen())
            {
                _isNavigating = true;
                e.Cancel = true;
                _needsOutroAnimation = false;
                _currentAnimationType = AnimationType.NavigateBackwardOut;
                _currentNavigationMode = NavigationMode.Back;

                RunAnimation();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            if (_isAnimating)
            {
                e.Cancel = true;
                return;
            }

            if (_loadingAndAnimatingIn)
            {
                e.Cancel = true;
                return;
            }

            _fromUri = this.NavigationService.CurrentSource;

            if (_needsOutroAnimation)
            {
                _needsOutroAnimation = false;

                if (!CanAnimate())
                    return;

                if (_isNavigating)
                {
                    e.Cancel = true;
                    return;
                }

                if (!this.NavigationService.CanGoBack && e.NavigationMode == NavigationMode.Back)
                    return;

                if (IsPopupOpen())
                {
                    return;
                }

                e.Cancel = true;
                _nextUri = e.Uri;

                switch (e.NavigationMode)
                {
                    case NavigationMode.New:
                        _currentAnimationType = AnimationType.NavigateForwardOut;
                        break;

                    case NavigationMode.Back:
                        _currentAnimationType = AnimationType.NavigateBackwardOut;
                        break;

                    case NavigationMode.Forward:
                        _currentAnimationType = AnimationType.NavigateForwardOut;
                        break;
                }
                _currentNavigationMode = e.NavigationMode;

                if (e.Uri != ExternalUri)
                    RunAnimation();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _currentNavigationMode = null;

            //Debug.WriteLine("OnNavigatedTo: {0}", this);

            if (_nextUri != ExternalUri)
            {
                //this.InvokeOnLayoutUpdated(() => OnLayoutUpdated(this, null));
                _loadingAndAnimatingIn = true;
                this.Loaded += new RoutedEventHandler(AnimatedBasePage_Loaded);

                if (AnimationContext != null)
                    AnimationContext.Opacity = 0;
            }

            _needsOutroAnimation = true;
        }

        void AnimatedBasePage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(AnimatedBasePage_Loaded);
            OnLayoutUpdated(this, null);
        }

        void OnLayoutUpdated(object sender, EventArgs e)
        {
            //Debug.WriteLine("OnLayoutUpdated: {0}", this);

            if (_isForwardNavigation)
            {
                _currentAnimationType = AnimationType.NavigateForwardIn;
                _arrivedFromUri = _fromUri != null ? new Uri(_fromUri.OriginalString, UriKind.Relative) : null;
            }
            else
            {
                _currentAnimationType = AnimationType.NavigateBackwardIn;
            }
            
            if (CanAnimate())
            {
                RunAnimation();
            }
            else
            {
                if (AnimationContext != null)
                    AnimationContext.Opacity = 1;

                OnTransitionAnimationCompleted();
            }

            //OnFirstLayoutUpdated(!_isForwardNavigation, _fromUri);

            if (_isForwardNavigation)
                _isForwardNavigation = false;
        }

        protected virtual void OnFirstLayoutUpdated(bool isBackNavigation, Uri from) { }

        private void RunAnimation()
        {
            _isAnimating = true;

            AnimatorHelperBase animation = null;
            
            switch (_currentAnimationType)
            {
                case AnimationType.NavigateForwardIn:
                    animation = GetAnimation(_currentAnimationType, _fromUri);
                    break;
                case AnimationType.NavigateBackwardOut:
                    animation = GetAnimation(_currentAnimationType, _arrivedFromUri);
                    break;
                default:
                    animation = GetAnimation(_currentAnimationType, _nextUri);
                    break;
            }

            Dispatcher.BeginInvoke(() =>
            {
                AnimatorHelperBase transitionAnimation;

                if (animation == null)
                {
                    AnimationContext.Opacity = 1;
                    OnTransitionAnimationCompleted();
                }
                else
                {
                    transitionAnimation = animation;
                    AnimationContext.Opacity = 1;
                    transitionAnimation.Begin(OnTransitionAnimationCompleted);
                }

                //Debug.WriteLine("{0} - {1} - {2} - {3}", this, _currentAnimationType, _currentAnimationType == AnimationType.NavigateForwardOut || _currentAnimationType == AnimationType.NavigateBackwardIn ? _nextUri : _fromUri, transitionAnimation);
            });
        }

        private bool CanAnimate()
        {
            return (_isActive && !_isNavigating && AnimationContext != null);
        }

        void OnTransitionAnimationCompleted()
        {
            _isAnimating = false;
            _loadingAndAnimatingIn = false;

            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    //Debug.WriteLine("{0} - Animation complete: {1}", this, _currentAnimationType);
                    //Debug.WriteLine("nav mode : {0}", _currentNavigationMode);
                    switch (_currentNavigationMode)
                    {
                        case NavigationMode.Forward:
                            Application.Current.GoForward();
                            break;

                        case NavigationMode.Back:
                            Application.Current.GoBack();
                            break;

                        case NavigationMode.New:
                            Application.Current.Navigate(_nextUri);
                            break;
                    }
                    _isNavigating = false;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OnTransitionAnimationCompleted Exception on {0}: {1}", this, ex);
            }

            AnimationsComplete(_currentAnimationType);
        }

        public AnimatorHelperBase GetContinuumAnimation(FrameworkElement element, AnimationType animationType)
        {
            TextBlock nameText;

            if (element is TextBlock)
                nameText = element as TextBlock;
            else
                nameText = element.GetVisualDescendants().OfType<TextBlock>().FirstOrDefault();

            if (nameText != null)
            {
                if (animationType == AnimationType.NavigateForwardIn)
                {
                    return new ContinuumForwardInAnimator() { RootElement = nameText, LayoutRoot = AnimationContext };
                }
                if (animationType == AnimationType.NavigateForwardOut)
                {
                    return new ContinuumForwardOutAnimator() { RootElement = nameText, LayoutRoot = AnimationContext };
                }
                if (animationType == AnimationType.NavigateBackwardIn)
                {
                    return new ContinuumBackwardInAnimator() { RootElement = nameText, LayoutRoot = AnimationContext };
                }
                if (animationType == AnimationType.NavigateBackwardOut)
                {
                    return new ContinuumBackwardOutAnimator() { RootElement = nameText, LayoutRoot = AnimationContext };
                }
            }
            return null;
        }

        protected virtual void AnimationsComplete(AnimationType animationType) { }

        protected virtual AnimatorHelperBase GetAnimation(AnimationType animationType, Uri toOrFrom)
        {
            AnimatorHelperBase animation;

            switch (animationType)
            {
                case AnimationType.NavigateBackwardIn:
                    animation = new TurnstileBackwardInAnimator();
                    break;

                case AnimationType.NavigateBackwardOut:
                    animation = new TurnstileBackwardOutAnimator();
                    break;

                case AnimationType.NavigateForwardIn:
                    animation = new TurnstileForwardInAnimator();
                    break;

                default:
                    animation = new TurnstileForwardOutAnimator();
                    break;
            }

            animation.RootElement = AnimationContext;
            return animation;
        }

        protected virtual bool IsPopupOpen()
        {
            return false;
        }

        public void CancelAnimation()
        {
            _isActive = false;
        }

        public void ResumeAnimation()
        {
            _isActive = true;
        }
    }
}
