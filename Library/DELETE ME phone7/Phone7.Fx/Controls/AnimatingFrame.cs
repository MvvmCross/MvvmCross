using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace Phone7.Fx.Controls
{
    /// <summary>
    /// Represents a control with a single piece of content and when that content 
    /// changes performs a transition animation. 
    /// </summary>
    /// <QualityBand>Experimental</QualityBand>
    /// <remarks>The API for this control will change considerably in the future.</remarks>
    [TemplateVisualState(GroupName = PresentationGroup, Name = NormalState)]
    [TemplateVisualState(GroupName = PresentationGroup, Name = DefaultTransitionState)]
    [TemplatePart(Name = CurrentContentPresentationSitePartName, Type = typeof(ContentControl))]
    public class AnimatingFrame : PhoneApplicationFrame
    {

        public static AnimatingFrame Instance
        {
            get { return Application.Current.RootVisual as AnimatingFrame; }
        }

        public enum Transitions
        {
            SlideUp,
            SlideDown,
            FlipToBack,
            FlipToFront,
            SwingIn,
            SwingOut
        }

        #region Visual state names
        /// <summary>
        /// The name of the group that holds the presentation states.
        /// </summary>
        private const string PresentationGroup = "PresentationStates";

        /// <summary>
        /// The name of the state that represents a normal situation where no
        /// transition is currently being used.
        /// </summary>
        private const string NormalState = "Normal";

        /// <summary>
        /// </summary>
        private const string ClonedState = "Cloned";

        private const string PreClonedState = "PreCloned";

        /// <summary>
        /// The name of the state that represents the default transition.
        /// </summary>
        public const string DefaultTransitionState = "DefaultTransition";
        #endregion Visual state names

        #region Template part names

        /// <summary>
        /// The name of the control that will display the current content.
        /// </summary>
        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

        internal const string CurrentContentClonePartName = "CurrentContentClone";
        internal const string PreviousContentClonePartName = "PreviousContentClone";

        #endregion Template part names

        #region TemplateParts
        /// <summary>
        /// Gets or sets the current content presentation site.
        /// </summary>
        /// <value>The current content presentation site.</value>
        private ContentPresenter CurrentContentPresentationSite { get; set; }

        private Image CurrentContentClone { get; set; }

        private Image PreviousContentClone { get; set; }

        #endregion TemplateParts

        #region public bool IsTransitioning

        /// <summary>
        /// Indicates whether the control allows writing IsTransitioning.
        /// </summary>
        private bool _allowIsTransitioningWrite;

        /// <summary>
        /// Gets a value indicating whether this instance is currently performing
        /// a transition.
        /// </summary>
        public bool IsTransitioning
        {
            get { return (bool)GetValue(IsTransitioningProperty); }
            private set
            {
                _allowIsTransitioningWrite = true;
                SetValue(IsTransitioningProperty, value);
                _allowIsTransitioningWrite = false;
            }
        }

        /// <summary>
        /// Identifies the IsTransitioning dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTransitioningProperty =
            DependencyProperty.Register(
                "IsTransitioning",
                typeof(bool),
                typeof(AnimatingFrame),
                new PropertyMetadata(OnIsTransitioningPropertyChanged));

        /// <summary>
        /// IsTransitioningProperty property changed handler.
        /// </summary>
        /// <param name="d">TransitioningContentControl that changed its IsTransitioning.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AnimatingFrame source = (AnimatingFrame)d;

            if (!source._allowIsTransitioningWrite)
            {
                source.IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException();
            }
        }
        #endregion public bool IsTransitioning


        private WriteableBitmap _previousContentSnapshot;

        /// <summary>
        /// The storyboard that is used to transition old and new content.
        /// </summary>
        private Storyboard _currentTransition;

        /// <summary>
        /// Gets or sets the storyboard that is used to transition old and new content.
        /// </summary>
        private Storyboard CurrentTransition
        {
            get { return _currentTransition; }
            set
            {
                // decouple event
                if (_currentTransition != null)
                {
                    _currentTransition.Completed -= OnTransitionCompleted;
                }

                _currentTransition = value;

                if (_currentTransition != null)
                {
                    _currentTransition.Completed += OnTransitionCompleted;
                }
            }
        }

        #region public string Transition
        /// <summary>
        /// Gets or sets the name of the transition to use. These correspond
        /// directly to the VisualStates inside the PresentationStates group.
        /// </summary>
        public string Transition
        {
            get { return GetValue(TransitionProperty) as string; }
            set { SetValue(TransitionProperty, value); }
        }

        /// <summary>
        /// Identifies the Transition dependency property.
        /// </summary>
        public static readonly DependencyProperty TransitionProperty =
            DependencyProperty.Register(
                "Transition",
                typeof(string),
                typeof(AnimatingFrame),
                new PropertyMetadata(DefaultTransitionState, OnTransitionPropertyChanged));

        /// <summary>
        /// TransitionProperty property changed handler.
        /// </summary>
        /// <param name="d">TransitioningContentControl that changed its Transition.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AnimatingFrame source = (AnimatingFrame)d;
            string oldTransition = e.OldValue as string;
            string newTransition = e.NewValue as string;

            if (source.IsTransitioning)
            {
                source.AbortTransition();
            }

            // find new transition
            Storyboard newStoryboard = source.GetStoryboard(newTransition);

            // unable to find the transition.
            if (newStoryboard == null)
            {
                // could be during initialization of xaml that presentationgroups was not yet defined
                if (TryGetVisualStateGroup(source, PresentationGroup) == null)
                {
                    // will delay check
                    source.CurrentTransition = null;
                }
                else
                {
                    // revert to old value
                    source.SetValue(TransitionProperty, oldTransition);

                    throw new ArgumentException(
                        string.Format("Transition not found {0}", newTransition));
                }
            }
            else
            {
                source.CurrentTransition = newStoryboard;
            }
        }
        #endregion public string Transition

        #region public bool RestartTransitionOnContentChange
        /// <summary>
        /// Gets or sets a value indicating whether the current transition
        /// will be aborted when setting new content during a transition.
        /// </summary>
        public bool RestartTransitionOnContentChange
        {
            get { return (bool)GetValue(RestartTransitionOnContentChangeProperty); }
            set { SetValue(RestartTransitionOnContentChangeProperty, value); }
        }

        /// <summary>
        /// Identifies the RestartTransitionOnContentChange dependency property.
        /// </summary>
        public static readonly DependencyProperty RestartTransitionOnContentChangeProperty =
            DependencyProperty.Register(
                "RestartTransitionOnContentChange",
                typeof(bool),
                typeof(AnimatingFrame),
                new PropertyMetadata(false, OnRestartTransitionOnContentChangePropertyChanged));

        /// <summary>
        /// RestartTransitionOnContentChangeProperty property changed handler.
        /// </summary>
        /// <param name="d">TransitioningContentControl that changed its RestartTransitionOnContentChange.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnRestartTransitionOnContentChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatingFrame)d).OnRestartTransitionOnContentChangeChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Called when the RestartTransitionOnContentChangeProperty changes.
        /// </summary>
        /// <param name="oldValue">The old value of RestartTransitionOnContentChange.</param>
        /// <param name="newValue">The new value of RestartTransitionOnContentChange.</param>
        protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
        {
        }
        #endregion public bool RestartTransitionOnContentChange

        #region Events
        /// <summary>
        /// Occurs when the current transition has completed.
        /// </summary>
        public event RoutedEventHandler TransitionCompleted;
        #endregion Events

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitioningContentControl"/> class.
        /// </summary>
        public AnimatingFrame()
        {
            DefaultStyleKey = typeof(AnimatingFrame);
        }

        /// <summary>
        /// Builds the visual tree for the TransitioningContentControl control 
        /// when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (IsTransitioning)
            {
                AbortTransition();
            }

            base.OnApplyTemplate();

            CurrentContentPresentationSite = GetTemplateChild(CurrentContentPresentationSitePartName) as ContentPresenter;
            PreviousContentClone = GetTemplateChild(PreviousContentClonePartName) as Image;
            CurrentContentClone = GetTemplateChild(CurrentContentClonePartName) as Image;

            this.Navigating += new System.Windows.Navigation.NavigatingCancelEventHandler(AnimatingFrame_Navigating);

            if (CurrentContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = Content;
            }

            // hookup currenttransition
            Storyboard transition = GetStoryboard(Transition);
            CurrentTransition = transition;
            if (transition == null)
            {
                string invalidTransition = Transition;
                // revert to default
                Transition = DefaultTransitionState;

                throw new ArgumentException(
                    string.Format("Transition not found {0}", invalidTransition));
            }

            VisualStateManager.GoToState(this, NormalState, false);
        }

        void AnimatingFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            _previousContentSnapshot = ConvertToImage(CurrentContentPresentationSite.Content as PhoneApplicationPage);
        }

        /// <summary>
        /// Called when the value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        /// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            StartTransition(oldContent, newContent);
        }

        /// <summary>
        /// Starts the transition.
        /// </summary>
        /// <param name="oldContent">The old content.</param>
        /// <param name="newContent">The new content.</param>
        private void StartTransition(object oldContent, object newContent)
        {
            // both presenters must be available, otherwise a transition is useless.
            if (oldContent != null && newContent != null)
            {
                PreviousContentClone.Source = _previousContentSnapshot;
                VisualStateManager.GoToState(this, PreClonedState, false);

                CurrentContentPresentationSite.Content = newContent;
                CurrentContentClone.Source = ConvertToImage(CurrentContentPresentationSite.Content as PhoneApplicationPage);

                // and start a new transition
                if (!IsTransitioning || RestartTransitionOnContentChange)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(delegate
                    {
                        IsTransitioning = true;
                        VisualStateManager.GoToState(this, ClonedState, false);
                        VisualStateManager.GoToState(this, Transition, true);
                    });
                }
                else if (IsTransitioning) //TODO: Is needed? Gets stuck in cloned mode if you cancel nav quickly during transition otherwise
                {
                    AbortTransition();
                }
            }
        }

        /// <summary>
        /// Handles the Completed event of the transition storyboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            AbortTransition();

            RoutedEventHandler handler = TransitionCompleted;
            if (handler != null)
            {
                handler(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Aborts the transition and releases the previous content.
        /// </summary>
        public void AbortTransition()
        {
            // go to normal state and release our hold on the old content.
            VisualStateManager.GoToState(this, NormalState, false);

            IsTransitioning = false;

            if (PreviousContentClone != null)
            {
                PreviousContentClone.Source = null;
            }

            if (CurrentContentClone != null)
            {
                CurrentContentClone.Source = null;
            }
        }

        /// <summary>
        /// Attempts to find a storyboard that matches the newTransition name.
        /// </summary>
        /// <param name="newTransition">The new transition.</param>
        /// <returns>A storyboard or null, if no storyboard was found.</returns>
        private Storyboard GetStoryboard(string newTransition)
        {
            VisualStateGroup presentationGroup = TryGetVisualStateGroup(this, PresentationGroup);
            Storyboard newStoryboard = null;
            if (presentationGroup != null)
            {
                newStoryboard = presentationGroup.States
                    .OfType<VisualState>()
                    .Where(state => state.Name == newTransition)
                    .Select(state => state.Storyboard)
                    .FirstOrDefault();
            }
            return newStoryboard;
        }


        private static FrameworkElement GetImplementationRoot(DependencyObject dependencyObject)
        {
            Debug.Assert(dependencyObject != null, "DependencyObject should not be null.");
            return (1 == VisualTreeHelper.GetChildrenCount(dependencyObject)) ?
                VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement :
                null;
        }

        /// <summary>
        /// This method tries to get the named VisualStateGroup for the 
        /// dependency object. The provided object's ImplementationRoot will be 
        /// looked up in this call.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="groupName">The visual state group's name.</param>
        /// <returns>Returns null or the VisualStateGroup object.</returns>
        private static VisualStateGroup TryGetVisualStateGroup(DependencyObject dependencyObject, string groupName)
        {
            FrameworkElement root = GetImplementationRoot(dependencyObject);
            if (root == null)
            {
                return null;
            }

            return VisualStateManager.GetVisualStateGroups(root)
                .OfType<VisualStateGroup>()
                .Where(group => string.CompareOrdinal(groupName, group.Name) == 0)
                .FirstOrDefault();
        }

        private WriteableBitmap ConvertToImage(PhoneApplicationPage element)
        {
            if (0.0 == element.ActualHeight && 0.0 == element.ActualWidth)
            {
                //needed to render bitmap properly. otherwise won't have 
                //proper layout
                //TODO: might be a better way to render the element w/o doing this
                element.UpdateLayout();
            }

            var bitmap = new WriteableBitmap(element, null);
            bitmap.Invalidate();

            return bitmap;
        }
    }
}