using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Phone7.Fx.Controls
{
    [TemplatePart(Name = "Element", Type = typeof(FrameworkElement))]
    public class LoadingIndicator : Control
    {
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(LoadingIndicator),
            new PropertyMetadata(20, new PropertyChangedCallback(ValueChangedCallback)));

        public static readonly DependencyProperty StartOpacityProperty =
            DependencyProperty.Register("StartOpacity", typeof(double), typeof(LoadingIndicator),
            new PropertyMetadata(1.0, new PropertyChangedCallback(StartOpacityChangedCallback)));

        public static readonly DependencyProperty EndOpacityProperty =
            DependencyProperty.Register("EndOpacity", typeof(double), typeof(LoadingIndicator),
            new PropertyMetadata(0.1, new PropertyChangedCallback(EndOpacityChangedCallback)));

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(LoadingIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(1), new PropertyChangedCallback(ValueChangedCallback)));

        private static readonly DependencyProperty ControlVisibilityProperty =
            DependencyProperty.Register("ControlVisibility", typeof(Visibility), typeof(LoadingIndicator),
            new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(ControlVisibilityCallback)));

        /// <summary>
        /// Gets or sets inner Radius.
        /// </summary>
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets start Opacity value.
        /// </summary>
        public double StartOpacity
        {
            get { return (double)GetValue(StartOpacityProperty); }
            set { SetValue(StartOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets end Opacity value.
        /// </summary>
        public double EndOpacity
        {
            get { return (double)GetValue(EndOpacityProperty); }
            set { SetValue(EndOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets Duration value.
        /// </summary>
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets Control Visibility.
        /// </summary>
        private bool ControlVisibility
        {
            get { return (bool)GetValue(ControlVisibilityProperty); }
            set { SetValue(ControlVisibilityProperty, value); }
        }

        private List<FrameworkElement> AnimationElements { get; set; }
        private Grid GridRoot { get; set; }

        /// <summary>
        /// Redraw control with new parameters.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static void ValueChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            LoadingIndicator ctl = (LoadingIndicator)obj;
            ctl.CreateAnimation();
        }

        /// <summary>
        /// Check start opacity and redraw control with new parameters.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static void StartOpacityChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            LoadingIndicator ctl = (LoadingIndicator)obj;
            ctl.StartOpacity = LoadingIndicator.CorrectOpacityValue((double)args.NewValue);

            ctl.CreateAnimation();
        }

        /// <summary>
        /// Check end opacity and redraw control with new parameters.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static void EndOpacityChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            LoadingIndicator ctl = (LoadingIndicator)obj;
            ctl.EndOpacity = LoadingIndicator.CorrectOpacityValue((double)args.NewValue);

            ctl.CreateAnimation();
        }

        /// <summary>
        /// Correct opacity value not to cross valid range borders.
        /// </summary>
        /// <param name="opacity">Initial opacity value.</param>
        /// <returns>Corrected opacity value.</returns>
        private static double CorrectOpacityValue(double opacity)
        {
            if (opacity < 0) return 0;
            if (opacity > 1) return 1;

            return opacity;
        }

        /// <summary>
        /// Stop animation when control becomes collapsed and create it anew - when visible.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static void ControlVisibilityCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            LoadingIndicator ctl = (LoadingIndicator)obj;

            Visibility visibility = (Visibility)args.NewValue;
            if (ctl.GridRoot != null)
            {
                if (visibility == Visibility.Collapsed)
                {
                    ctl.GridRoot.Children.Clear();
                }
                else
                {
                    ctl.CreateAnimation();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingIndicator"/> class.
        /// </summary>
        public LoadingIndicator()
        {
            AnimationElements = new List<FrameworkElement>();
            this.DefaultStyleKey = typeof(LoadingIndicator);
        }


        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            GridRoot = GetTemplateChild("GridRoot") as Grid;
            if (GridRoot == null)
            {
                throw new NotImplementedException("GridRoot is required to display LoadingIndicator.");
            }

            for (int i = 0; i < 12; i++)
            {
                AnimationElements.Add(GetTemplateChild(string.Format("Element{0}", i)) as FrameworkElement);
            }

            CreateAnimation();
        }


        /// <summary>
        /// Creates the animation.
        /// </summary>
        private void CreateAnimation()
        {
            if (GridRoot != null)
            {
                GridRoot.Children.Clear();

                double angle = 360.0 / AnimationElements.Count;
                double width = AnimationElements[0].Width;
                double x = (Width - width) / 2;
                double y = Height / 2 + Radius;

                for (int i   = 0; i < AnimationElements.Count; i++)
                {
                    var element = AnimationElements[i];
                    TranslateTransform tt = new TranslateTransform() {X = x, Y = y};
                    RotateTransform rt = new RotateTransform()
                                             {Angle = i*angle + 180, CenterX = (width/2), CenterY = -Radius};
                    TransformGroup tg = new TransformGroup();
                    tg.Children.Add(rt);
                    tg.Children.Add(tt);
                    element.RenderTransform = tg;

                    GridRoot.Children.Add(element);

                    DoubleAnimation animation = new DoubleAnimation
                                                    {
                                                        From = this.StartOpacity,
                                                        To = this.EndOpacity,
                                                        Duration = this.Duration,
                                                        RepeatBehavior = RepeatBehavior.Forever,
                                                        BeginTime =
                                                            TimeSpan.FromMilliseconds((this.Duration.TotalMilliseconds / AnimationElements.Count) * i)
                                                    };
                    Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
                    Storyboard.SetTarget(animation, element);

                    Storyboard sb = new Storyboard();
                    sb.Children.Add(animation);
                    sb.Begin();
                }

                // Bind ControlVisibilityProperty to the Visibility property 
                // in order to catch missing OnVisibilityChanged event
                Binding binding = new Binding {Source = this, Path = new PropertyPath("Visibility")};
                this.SetBinding(ControlVisibilityProperty, binding);
            }
        }
    }
}