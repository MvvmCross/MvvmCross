using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Phone7.Fx.Extensions;
using System.Linq;

namespace Phone7.Fx.Controls.Animations
{
    public class TurnstileFeatherAnimator : AnimatorHelperBase
    {
        protected enum Directions
        {
            In,
            Out
        }

        public ListBox ListBox { get; set; }
        protected int Duration { get; set; }
        protected int Angle { get; set; }
        protected int FeatherDelay { get; set; }
        protected Directions Direction { get; set; }
        protected int InitialDelay { get; set; }
        protected bool HoldSelectedItem { get; set; }

        private FrameworkElement _visual;
        private bool? _isVerticalOrientation;

        internal bool IsOnCurrentPage(ListBoxItem item)
        {
            var itemsHostRect = Rect.Empty;
            var listBoxItemRect = Rect.Empty;

            if (_visual == null)
            {
                ItemsControlHelper ich = new ItemsControlHelper(ListBox);
                ScrollContentPresenter scp = ich.ScrollHost == null ? null : ich.ScrollHost.GetVisualDescendants().OfType<ScrollContentPresenter>().FirstOrDefault();
                _visual = (ich.ScrollHost == null) ? null : ((scp == null) ? ((FrameworkElement)ich.ScrollHost) : ((FrameworkElement)scp));
            }

            if (_visual == null)
                return true;

            itemsHostRect = new Rect(0.0, 0.0, _visual.ActualWidth, _visual.ActualHeight);
            //ListBoxItem item = ListBox.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
            if (item == null)
            {
                listBoxItemRect = Rect.Empty;
                return false;
            }

            GeneralTransform transform = item.TransformToVisual(_visual);
            listBoxItemRect = new Rect(transform.Transform(new Point()), transform.Transform(new Point(item.ActualWidth, item.ActualHeight)));
            if (!this.IsVerticalOrientation())
            {
                return ((itemsHostRect.Left <= listBoxItemRect.Left) && (listBoxItemRect.Right <= itemsHostRect.Right));
            }

            return ((listBoxItemRect.Bottom + 100 >= itemsHostRect.Top) && (listBoxItemRect.Top - 100 <= itemsHostRect.Bottom));
            //return ((itemsHostRect.Top <= listBoxItemRect.Bottom) && (listBoxItemRect.Top <= itemsHostRect.Bottom));
        }

        internal bool IsVerticalOrientation()
        {
            if (_isVerticalOrientation.HasValue)
                return _isVerticalOrientation.Value;

            ItemsControlHelper ich = new ItemsControlHelper(ListBox);
            StackPanel itemsHost = ich.ItemsHost as StackPanel;
            if (itemsHost != null)
            {
                _isVerticalOrientation = (itemsHost.Orientation == Orientation.Vertical);
                return _isVerticalOrientation.Value;
            }
            VirtualizingStackPanel panel2 = ich.ItemsHost as VirtualizingStackPanel;
            _isVerticalOrientation = ((panel2 == null) || (panel2.Orientation == Orientation.Vertical));
            return _isVerticalOrientation.Value;
        }

        public TurnstileFeatherAnimator()
            :base()
        {
            InitialDelay = 0;
        }

        public override void Begin(Action completionAction)
        {
            Storyboard = new Storyboard();

            double liCounter = 0;
            var listBoxItems = ListBox.GetVisualDescendants().OfType<ListBoxItem>().Where(lbi => IsOnCurrentPage(lbi) && lbi.IsEnabled).ToList();
            
            if (HoldSelectedItem && Direction == Directions.Out && ListBox.SelectedItem != null)
            {
                //move selected container to end
                var selectedContainer = ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.SelectedItem);
                listBoxItems.Remove(selectedContainer);
                listBoxItems.Add(selectedContainer);
            }

            foreach (ListBoxItem li in listBoxItems)
            {
                GeneralTransform gt = li.TransformToVisual(RootElement);
                Point globalCoords = gt.Transform(new Point(0, 0));
                double heightAdjustment = li.Content is FrameworkElement ? ((li.Content as FrameworkElement).ActualHeight / 2) : (li.ActualHeight / 2);
                //double yCoord = globalCoords.Y + ((((System.Windows.FrameworkElement)(((System.Windows.Controls.ContentControl)(li)).Content)).ActualHeight) / 2);
                double yCoord = globalCoords.Y + heightAdjustment;

                double offsetAmount = (RootElement.ActualHeight / 2) - yCoord;

                PlaneProjection pp = new PlaneProjection();
                pp.GlobalOffsetY = offsetAmount * -1;
                pp.CenterOfRotationX = 0;
                li.Projection = pp;

                CompositeTransform ct = new CompositeTransform();
                ct.TranslateY = offsetAmount;
                li.RenderTransform = ct;

                var beginTime = TimeSpan.FromMilliseconds((FeatherDelay * liCounter) + InitialDelay);

                if (Direction == Directions.In)
                {
                    li.Opacity = 0;

                    DoubleAnimationUsingKeyFrames daukf = new DoubleAnimationUsingKeyFrames();

                    EasingDoubleKeyFrame edkf1 = new EasingDoubleKeyFrame();
                    edkf1.KeyTime = beginTime;
                    edkf1.Value = Angle;
                    daukf.KeyFrames.Add(edkf1);

                    EasingDoubleKeyFrame edkf2 = new EasingDoubleKeyFrame();
                    edkf2.KeyTime = TimeSpan.FromMilliseconds(Duration).Add(beginTime);
                    edkf2.Value = 0;

                    ExponentialEase ee = new ExponentialEase();
                    ee.EasingMode = EasingMode.EaseOut;
                    ee.Exponent = 6;

                    edkf2.EasingFunction = ee;
                    daukf.KeyFrames.Add(edkf2);

                    Storyboard.SetTarget(daukf, li);
                    Storyboard.SetTargetProperty(daukf, new PropertyPath("(UIElement.Projection).(PlaneProjection.RotationY)"));
                    Storyboard.Children.Add(daukf);

                    DoubleAnimation da = new DoubleAnimation();
                    da.Duration = TimeSpan.FromMilliseconds(0);
                    da.BeginTime = beginTime;
                    da.To = 1;

                    Storyboard.SetTarget(da, li);
                    Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.Opacity)"));
                    Storyboard.Children.Add(da);
                }
                else
                {
                    li.Opacity = 1;

                    DoubleAnimation da = new DoubleAnimation();
                    da.BeginTime = beginTime;
                    da.Duration = TimeSpan.FromMilliseconds(Duration);
                    da.To = Angle;

                    ExponentialEase ee = new ExponentialEase();
                    ee.EasingMode = EasingMode.EaseIn;
                    ee.Exponent = 6;

                    da.EasingFunction = ee;

                    Storyboard.SetTarget(da, li);
                    Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.Projection).(PlaneProjection.RotationY)"));
                    Storyboard.Children.Add(da);

                    da = new DoubleAnimation();
                    da.Duration = TimeSpan.FromMilliseconds(10);
                    da.To = 0;
                    da.BeginTime = TimeSpan.FromMilliseconds(Duration).Add(beginTime);

                    Storyboard.SetTarget(da, li);
                    Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.Opacity)"));
                    Storyboard.Children.Add(da);
                }

                liCounter++;
            }

            base.Begin(completionAction);
        }
    }

    public class TurnstileFeatherForwardInAnimator : TurnstileFeatherAnimator
    {
        public TurnstileFeatherForwardInAnimator()
            : base()
        {
            Duration = 350;
            Angle = -80;
            FeatherDelay = 50;
            Direction = Directions.In;
        }
    }

    public class TurnstileFeatherForwardOutAnimator : TurnstileFeatherAnimator
    {
        public TurnstileFeatherForwardOutAnimator()
            : base()
        {
            Duration = 250;
            Angle = 50;
            FeatherDelay = 50;
            Direction = Directions.Out;
            HoldSelectedItem = true;
        }
    }

    public class TurnstileFeatherBackwardInAnimator : TurnstileFeatherAnimator
    {
        public TurnstileFeatherBackwardInAnimator()
            : base()
        {
            Duration = 350;
            Angle = 50;
            FeatherDelay = 50;
            Direction = Directions.In;
        }
    }

    public class TurnstileFeatherBackwardOutAnimator : TurnstileFeatherAnimator
    {
        public TurnstileFeatherBackwardOutAnimator()
            : base()
        {
            Duration = 250;
            Angle = -80;
            FeatherDelay = 50;
            Direction = Directions.Out;
        }
    }
}
