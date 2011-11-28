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
    public class SlideAnimator : AnimatorHelperBase
    {
        public override void Begin(Action completionAction)
        {
            if (this.PrepareElement(RootElement))
            {
                Storyboard.Stop();
                base.SetTarget(RootElement);
            }

            base.Begin(completionAction);
        }

        private bool PrepareElement(UIElement element)
        {
            element.GetTransform<CompositeTransform>(Extensions.TransformCreationMode.CreateOrAddAndIgnoreMatrix);

            return true;
        }
    }

    public class SlideUpAnimator : SlideAnimator
    {
        private static Storyboard _storyboard;

        public SlideUpAnimator()
            : base()
        {
            if (_storyboard == null)
                _storyboard = XamlReader.Load(Storyboards.SlideUpFadeInStoryboard) as Storyboard;

            Storyboard = _storyboard;
        }
    }

    public class SlideDownAnimator : SlideAnimator
    {
        private static Storyboard _storyboard;

        public SlideDownAnimator()
            : base()
        {
            if (_storyboard == null)
                _storyboard = XamlReader.Load(Storyboards.SlideDownFadeOutStoryboard) as Storyboard;

            Storyboard = _storyboard;
        }
    }
}
