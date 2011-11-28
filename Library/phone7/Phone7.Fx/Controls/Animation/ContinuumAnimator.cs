using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Phone7.Fx.Extensions;
using Microsoft.Phone.Controls;

namespace Phone7.Fx.Controls.Animations
{
    public class ContinuumAnimator : AnimatorHelperBase
    {
        public FrameworkElement LayoutRoot { get; set; }
        private Popup _popup;

        public override void Begin(Action completionAction)
        {
            Storyboard.Stop();

            PrepareElement(LayoutRoot);

            if (this is ContinuumForwardOutAnimator)
            {
                WriteableBitmap bitmap = new WriteableBitmap(RootElement, null);
                bitmap.Invalidate();
                var image = new Image() { Source = bitmap, Stretch = System.Windows.Media.Stretch.None };

                var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
                _popup = new Popup();
                var popupChild = new Canvas()
                {
                    Width = rootVisual.ActualWidth,
                    Height = rootVisual.ActualHeight
                };

                var transfrom = RootElement.TransformToVisual(rootVisual);
                var origin = transfrom.Transform(new Point(0, 0));
                popupChild.Children.Add(image);
                PrepareElement(image);
                Canvas.SetLeft(image, origin.X);
                Canvas.SetTop(image, origin.Y);

                _popup.Child = popupChild;
                RootElement.Opacity = 0;
                _popup.IsOpen = true;

                Storyboard.Completed += new EventHandler(OnContinuumBackwardOutStoryboardCompleted);
                base.SetTargets(new Dictionary<string, FrameworkElement>()
                {
                    { "LayoutRoot", LayoutRoot },
                    { "ContinuumElement", image }
                });
            }
            else
            {
                PrepareElement(RootElement);
                base.SetTargets(new Dictionary<string, FrameworkElement>()
                {
                    { "LayoutRoot", LayoutRoot },
                    { "ContinuumElement", RootElement }
                });
            }

            base.Begin(completionAction);
        }

        void OnContinuumBackwardOutStoryboardCompleted(object sender, EventArgs e)
        {
            Storyboard.Completed -= new EventHandler(OnContinuumBackwardOutStoryboardCompleted);
            _popup.IsOpen = false;
            _popup.Child = null;
            _popup = null;
        }

        private bool PrepareElement(UIElement element)
        {
            element.GetTransform<CompositeTransform>(Extensions.TransformCreationMode.CreateOrAddAndIgnoreMatrix);

            return true;
        }
    }

    public class ContinuumForwardInAnimator : ContinuumAnimator
    {
        public ContinuumForwardInAnimator()
            : base()
        {
            Storyboard = XamlReader.Load(Storyboards.ContinuumForwardInStoryboard) as Storyboard;
        }
    }

    public class ContinuumBackwardOutAnimator : ContinuumAnimator
    {
        public ContinuumBackwardOutAnimator()
            : base()
        {
            Storyboard = XamlReader.Load(Storyboards.ContinuumBackwardOutStoryboard) as Storyboard;
        }
    }

    public class ContinuumBackwardInAnimator : ContinuumAnimator
    {
        public ContinuumBackwardInAnimator()
            : base()
        {
            Storyboard = XamlReader.Load(Storyboards.ContinuumBackwardInStoryboard) as Storyboard;
        }
    }

    public class ContinuumForwardOutAnimator : ContinuumAnimator
    {
        public ContinuumForwardOutAnimator()
            : base()
        {
            Storyboard = XamlReader.Load(Storyboards.ContinuumForwardOutStoryboard) as Storyboard;
        }
    }

}
