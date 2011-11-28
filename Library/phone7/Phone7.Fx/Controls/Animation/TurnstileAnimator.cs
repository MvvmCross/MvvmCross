using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Phone7.Fx.Extensions;
using System.Collections.Generic;
using System.Windows.Media;

namespace Phone7.Fx.Controls.Animations
{
    public class TurnstileAnimator : AnimatorHelperBase
    {
        public override void Begin(Action completionAction)
        {
            if (this.PrepareElement(RootElement))
            {
                (RootElement.Projection as PlaneProjection).CenterOfRotationX = 0;
                Storyboard.Stop();
                base.SetTarget(RootElement);
            }

            base.Begin(completionAction);
        }

        private bool PrepareElement(UIElement element)
        {
            if (element.GetPlaneProjection(true) == null)
            {
                return false;
            }
            return true;
        }
    }

    public class TurnstileForwardInAnimator : TurnstileAnimator
    {
        private static Storyboard _storyboard;

        public TurnstileForwardInAnimator()
            : base()
        {
            if (_storyboard == null)
                _storyboard = XamlReader.Load(Storyboards.TurnstileForwardInStoryboard) as Storyboard;

            Storyboard = _storyboard;
        }
    }

    public class TurnstileForwardOutAnimator : TurnstileAnimator
    {
        private static Storyboard _storyboard;

        public TurnstileForwardOutAnimator()
            : base()
        {
            if (_storyboard == null)
                _storyboard = XamlReader.Load(Storyboards.TurnstileForwardOutStoryboard) as Storyboard;

            Storyboard = _storyboard;
        }
    }

    public class TurnstileBackwardInAnimator : TurnstileAnimator
    {
        private static Storyboard _storyboard;

        public TurnstileBackwardInAnimator()
            : base()
        {
            if (_storyboard == null)
                _storyboard = XamlReader.Load(Storyboards.TurnstileBackwardInStoryboard) as Storyboard;

            Storyboard = _storyboard;
        }
    }

    public class TurnstileBackwardOutAnimator : TurnstileAnimator
    {
        private static Storyboard _storyboard;

        public TurnstileBackwardOutAnimator()
            : base()
        {
            if (_storyboard == null)
                _storyboard = XamlReader.Load(Storyboards.TurnstileBackwardOutStoryboard) as Storyboard;

            Storyboard = _storyboard;
        }
    }

    public class DefaultPageAnimator : TurnstileAnimator 
    {
        private static Storyboard _storyboard;

        public DefaultPageAnimator()
            : base()
        {
            if (_storyboard == null)
                _storyboard = XamlReader.Load(Storyboards.DefaultStoryboard) as Storyboard;

            Storyboard = XamlReader.Load(Storyboards.DefaultStoryboard) as Storyboard;
        }
    }
}
