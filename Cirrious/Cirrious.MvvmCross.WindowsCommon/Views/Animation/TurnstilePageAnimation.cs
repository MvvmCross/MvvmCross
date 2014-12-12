using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Animation
{
    public class TurnstilePageAnimation : IPageAnimation
    {
        public Task NavigatedToForward(FrameworkElement source)
        {
            return Turnstile(source, -75, 0, 0, 1);
        }

        public Task NavigatedToBackward(FrameworkElement source)
        {
            return Turnstile(source, 75, 0, 0, 1);
        }

        public Task NavigatingFromForward(FrameworkElement source)
        {
            return Turnstile(source, 5, 75, 1, 0);
        }

        public Task NavigatingFromBackward(FrameworkElement source)
        {
            return Turnstile(source, -5, -75, 1, 0);
        }

        private Task Turnstile(FrameworkElement source, double fromRotation, double toRotation, double fromOpacity, double toOpacity)
        {
            if (source == null)
                return Task.FromResult<object>(null);

            source.Opacity = 1;
            source.Projection = new PlaneProjection { CenterOfRotationX = 0 };

            var duration = TimeSpan.FromSeconds(0.15);
            var story = new Storyboard();

            var turnstileAnimation = new DoubleAnimation
            {
                Duration = duration,
                From = fromRotation,
                To = toRotation
            };
            Storyboard.SetTargetProperty(turnstileAnimation, "(UIElement.Projection).(PlaneProjection.RotationY)");
            Storyboard.SetTarget(turnstileAnimation, source);

            var opacityAnimation = new DoubleAnimation
            {
                Duration = duration,
                From = fromOpacity,
                To = toOpacity,
            };
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");
            Storyboard.SetTarget(opacityAnimation, source);

            story.Children.Add(turnstileAnimation);
            story.Children.Add(opacityAnimation);

            var completion = new TaskCompletionSource<object>();
            story.Completed += delegate
            {
                ((PlaneProjection)source.Projection).RotationY = toRotation;
                source.Opacity = toOpacity;
                completion.SetResult(null);
            };
            story.Begin();
            return completion.Task;
        }
    }
}