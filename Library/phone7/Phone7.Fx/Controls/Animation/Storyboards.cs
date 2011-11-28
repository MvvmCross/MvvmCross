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

namespace Phone7.Fx.Controls.Animations
{
    public class Storyboards
    {
        internal static readonly string DefaultStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation Duration=""0"" To=""1"" Storyboard.TargetProperty=""(UIElement.Opacity)"" Storyboard.TargetName=""LayoutRoot"" />
        </Storyboard>";

        internal static readonly string TurnstileForwardInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
	        <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationY)"" Storyboard.TargetName=""LayoutRoot"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""-80""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string TurnstileForwardOutStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
	        <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationY)"" Storyboard.TargetName=""LayoutRoot"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""50"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
			<DoubleAnimationUsingKeyFrames Storyboard.TargetName=""LayoutRoot"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
				<DiscreteDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""0"" />
			</DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string TurnstileBackwardInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
	        <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationY)"" Storyboard.TargetName=""LayoutRoot"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""50""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string TurnstileBackwardOutStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
	        <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationY)"" Storyboard.TargetName=""LayoutRoot"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""-80"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
			<DoubleAnimationUsingKeyFrames Storyboard.TargetName=""LayoutRoot"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
				<DiscreteDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""0"" />
			</DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string SwivelShowStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
	        <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"" Storyboard.TargetName=""MyPopup"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""-45""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseOut"" Exponent=""3""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string SwivelHideStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
	        <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"" Storyboard.TargetName=""MyPopup"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""60"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseIn"" Exponent=""3""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string SwivelFullScreenShowStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"" Storyboard.TargetName=""MyPopup"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""-30""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseOut"" Exponent=""3""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string SwivelFullScreenHideStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"" Storyboard.TargetName=""MyPopup"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""45"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseIn"" Exponent=""3""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string ContinuumForwardOutStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""70"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""3""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
	        <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)"" Storyboard.TargetName=""LayoutRoot"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""1""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""0"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseIn"" Exponent=""3""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" Storyboard.TargetName=""ContinuumElement"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""73"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""3""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateX)"" Storyboard.TargetName=""ContinuumElement"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""225"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""3""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
			<DoubleAnimationUsingKeyFrames Storyboard.TargetName=""ContinuumElement"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
				<DiscreteDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""0"" />
			</DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        internal static readonly string ContinuumForwardInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""50""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""3""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" Storyboard.TargetName=""ContinuumElement"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""-70""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""3""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateX)"" Storyboard.TargetName=""ContinuumElement"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""130""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""3""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""0"" To=""1"" Duration=""0:0:0.15"" 
                                 Storyboard.TargetName=""LayoutRoot"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        internal static readonly string ContinuumBackwardOutStoryboard =
        @"<Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" 
                                           Storyboard.TargetName=""LayoutRoot"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""50"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""1"" To=""0"" Duration=""0:0:0.15"" 
                                 Storyboard.TargetName=""LayoutRoot"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        internal static readonly string ContinuumBackwardInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateX)"" Storyboard.TargetName=""ContinuumElement"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""-70""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""3""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" Storyboard.TargetName=""ContinuumElement"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""-30""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""3""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
			<DoubleAnimationUsingKeyFrames Storyboard.TargetName=""ContinuumElement"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
				<DiscreteDoubleKeyFrame KeyTime=""0:0:0"" Value=""1"" />
			</DoubleAnimationUsingKeyFrames>
	        <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)"" Storyboard.TargetName=""LayoutRoot"">
		        <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
		        <EasingDoubleKeyFrame KeyTime=""0:0:0.15"" Value=""1"">
			        <EasingDoubleKeyFrame.EasingFunction>
				        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
			        </EasingDoubleKeyFrame.EasingFunction>
		        </EasingDoubleKeyFrame>
	        </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Duration=""0"" To=""0"" Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" Storyboard.TargetName=""LayoutRoot""/>
        </Storyboard>";

        internal static readonly string RotateLeftStoryboard =
            @"";

        internal static readonly string RotateRightStoryboard =
            @"";

        internal static readonly string SlideUpFadeInStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" 
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

        internal static readonly string SlideDownFadeOutStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(CompositeTransform.TranslateY)"" 
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

        internal static readonly string SlideLeftFadeInStoryboard =
    @"";

        internal static readonly string SlideLeftFadeOutStoryboard =
@"";

        internal static readonly string SlideRightFadeInStoryboard =
@"";

        internal static readonly string SlideRightFadeOutStoryboard =
@"";

        internal static readonly string SlideRightStoryboard =
@"";
        internal static readonly string SlideLeftStoryboard =
@"";
    }
}
