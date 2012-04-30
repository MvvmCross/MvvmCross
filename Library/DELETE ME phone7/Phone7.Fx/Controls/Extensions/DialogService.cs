using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Shell;
using Primitives = System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Phone7.Fx.Extensions;
using Microsoft.Phone.Controls;
using Phone7.Fx.Controls;

namespace Phone7.Fx.Extensions
{
    public class DialogService
    {
        public enum AnimationTypes
        {
            Slide,
            Swivel
        }

        private static readonly string SlideUpStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.Y)"" 
                                           Storyboard.TargetName=""LayoutRoot"">
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""150""/>
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                        <EasingDoubleKeyFrame.EasingFunction>
                            <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                        </EasingDoubleKeyFrame.EasingFunction>
                    </EasingDoubleKeyFrame>
                </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""0"" To=""1"" Duration=""0:0:0.350"" 
                                 Storyboard.TargetName=""LayoutRoot"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private static readonly string SlideDownStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.Y)"" 
                                           Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""150"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""1"" To=""0"" Duration=""0:0:0.25"" 
                                 Storyboard.TargetName=""LayoutRoot"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        internal static readonly string SwivelInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation BeginTime=""0:0:0"" Duration=""0"" 
                                Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.CenterOfRotationY)"" 
                                Storyboard.TargetName=""LayoutRoot""
                                To="".5""/>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"" Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""-30""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)""
                                            Storyboard.TargetName=""LayoutRoot"">
                <DiscreteDoubleKeyFrame KeyTime=""0"" Value=""1"" />
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string SwivelOutStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation BeginTime=""0:0:0"" Duration=""0"" 
                                Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.CenterOfRotationY)"" 
                                Storyboard.TargetName=""LayoutRoot""
                                To="".5""/>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"" Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""45"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)""
                                            Storyboard.TargetName=""LayoutRoot"">
                <DiscreteDoubleKeyFrame KeyTime=""0"" Value=""1"" />
                <DiscreteDoubleKeyFrame KeyTime=""0:0:0.267"" Value=""0"" />
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        private static ContentPresenter _popupContainer;
        private PhoneApplicationFrame _rootVisual;
        private PhoneApplicationPage _page;
        private IApplicationBar _originalAppBar;
        private Panel _overlay;
        private Storyboard _showStoryboard;
        private Storyboard _hideStoryboard;

        public FrameworkElement Child { get; set; }
        public AnimationTypes AnimationType { get; set; }
        public double VerticalOffset { get; set; }
        public Brush BackgroundBrush { get; set; }

        internal ApplicationBar AppBar { get; set; }
        internal bool IsOpen { get; set; }

        public event EventHandler Closed;
        public event EventHandler Opened;

        // set this to prevent the dialog service from closing on back click
        public bool HasPopup { get; set; }

        internal FrameworkElement RootVisual
        {
            get
            {
                if (_rootVisual == null)
                {
                    _rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
                    _page = _rootVisual.GetVisualDescendants().OfType<PhoneApplicationPage>().FirstOrDefault();
                }

                return _rootVisual;
            }
        }

        internal static ContentPresenter PopupContainer
        {
            get
            {
                if (_popupContainer == null)
                {
                    var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
                    _popupContainer = rootVisual.GetVisualDescendants().OfType<ContentPresenter>().Where(p => p.Name.Equals("FauxPopup")).FirstOrDefault();
                }

                return _popupContainer;
            }
        }

        public DialogService()
        {
            AnimationType = AnimationTypes.Slide;
        }

        private void InitializePopup()
        {
            // Add overlay which is the size of RootVisual
            _overlay = new Grid
            {
                Width = RootVisual.ActualWidth,
                Height = RootVisual.ActualHeight
            };

            switch (AnimationType)
            {
                case AnimationTypes.Slide:
                    _showStoryboard = XamlReader.Load(SlideUpStoryboard) as Storyboard;
                    _hideStoryboard = XamlReader.Load(SlideDownStoryboard) as Storyboard;
                    _overlay.RenderTransform = new TranslateTransform();
                    break;

                default:
                    _showStoryboard = XamlReader.Load(SwivelInStoryboard) as Storyboard;
                    _hideStoryboard = XamlReader.Load(SwivelOutStoryboard) as Storyboard;
                    _overlay.Projection = new PlaneProjection();
                    break;
            }

            _overlay.Children.Add(Child);

            Child.Width = _rootVisual.ActualWidth;

            // Initialize popup to draw the context menu over all controls
            PopupContainer.Content = _overlay;

            if (BackgroundBrush != null)
                _overlay.Background = BackgroundBrush;

            _overlay.Margin = new Thickness(0, VerticalOffset, 0, 0);

        }

        /// <summary>
        /// Shows the context menu.
        /// </summary>
        public void Show()
        {
            IsOpen = true;

            InitializePopup();

            _overlay.Opacity = 0;

            _page.BackKeyPress += OnBackKeyPress;
            _page.NavigationService.Navigated += OnNavigated;

            _originalAppBar = _page.ApplicationBar;

            _showStoryboard.Completed += new EventHandler(_showStoryboard_Completed);
            foreach (Timeline t in _showStoryboard.Children)
                Storyboard.SetTarget(t, _overlay);

            _overlay.InvokeOnLayoutUpdated(() =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _showStoryboard.Begin();

                    if (_page != null)
                    {

                        _page.ApplicationBar = AppBar;
                    }
                });
            });


            // Show the popup
            PopupContainer.Visibility = Visibility.Visible;
        }


        void _showStoryboard_Completed(object sender, EventArgs e)
        {
            var sb = sender as Storyboard;
            sb.Completed -= _showStoryboard_Completed;

            if (Opened != null)
                Opened(this, null);
        }

        void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Hide();
        }

        public void Hide()
        {
            if (!IsOpen)
                return;

            if (_page != null)
            {
                _page.BackKeyPress -= OnBackKeyPress;
                _page.NavigationService.Navigated -= OnNavigated;

                var page = _page as AnimatedBasePage;
                if (page != null && _originalAppBar != null)
                    _page.ApplicationBar = _originalAppBar;
                else
                    _page.ApplicationBar = null;

                _page = null;
            }

            _hideStoryboard.Stop();
            foreach (Timeline t in _hideStoryboard.Children)
            {
                Storyboard.SetTarget(t, _overlay);
            }
            _hideStoryboard.Completed += _hideStoryboard_Completed;
            _hideStoryboard.Begin();
        }

        void _hideStoryboard_Completed(object sender, EventArgs e)
        {
            _hideStoryboard.Completed -= _hideStoryboard_Completed;
            _hideStoryboard = null;

            IsOpen = false;

            if (PopupContainer != null)
            {
                PopupContainer.Visibility = Visibility.Collapsed;
                PopupContainer.Content = null;
            }

            if (null != _overlay)
            {
                _overlay.Children.Clear();
                _overlay = null;
            }

            if (Closed != null)
                Closed(this, null);
        }

        public void OnBackKeyPress(object sender, CancelEventArgs e)
        {
            if (HasPopup)
            {
                e.Cancel = true;
                return;
            }
            if (IsOpen)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
