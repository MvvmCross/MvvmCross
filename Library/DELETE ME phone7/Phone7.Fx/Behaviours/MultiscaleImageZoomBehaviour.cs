using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Phone7.Fx.Behaviours
{
    /// <summary>
    /// A behaviour for zooming and panning around on a MultiScaleImage
    /// using manipulation events
    /// </summary>
    public class MultiscaleImageZoomBehaviour : Behavior<MultiScaleImage>
    {
        /// <summary>
        /// Initialize the behavior
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ManipulationStarted += AssociatedObject_ManipulationStarted;
            AssociatedObject.ManipulationDelta += AssociatedObject_ManipulationDelta;
        }

        /// <summary>
        /// Shortcut for the Multiscale image
        /// </summary>
        public MultiScaleImage Msi { get { return AssociatedObject; } }

        /// <summary>
        /// Screen point where the manipulation started
        /// </summary>
        private Point ManipulationOrigin { get; set; }

        /// <summary>
        /// Multiscale view point origin on the moment the manipulation started
        /// </summary>
        private Point MsiOrigin { get; set; }

        void AssociatedObject_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            // Save the current manipulation origin and MSI view point origin
            MsiOrigin = new Point(Msi.ViewportOrigin.X, Msi.ViewportOrigin.Y);
            ManipulationOrigin = e.ManipulationOrigin;
        }

        void AssociatedObject_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {

            if (e.DeltaManipulation.Scale.X == 0 && e.DeltaManipulation.Scale.Y == 0)
            {
                // No scaling took place (i.e. no multi touch)
                // 'Simply' calculate a new view point origin
                Msi.ViewportOrigin =
                  new Point
                  {
                      X = MsiOrigin.X - (e.CumulativeManipulation.Translation.X /
                        Msi.ActualWidth * Msi.ViewportWidth),
                      Y = MsiOrigin.Y - (e.CumulativeManipulation.Translation.Y /
                        Msi.ActualHeight * Msi.ViewportWidth),
                  };
            }
            else
            {
                // Multi touch - choose to interpretet this either as zoom or pinch
                var zoomscale = (e.DeltaManipulation.Scale.X + e.DeltaManipulation.Scale.Y) / 2;

                // Calculate a new 'logical point' - the MSI has its own 'coordinate system'
                var logicalPoint = Msi.ElementToLogicalPoint(
                  new Point
                  {
                      X = ManipulationOrigin.X - e.CumulativeManipulation.Translation.X,
                      Y = ManipulationOrigin.Y - e.CumulativeManipulation.Translation.Y
                  }
                  );
                Msi.ZoomAboutLogicalPoint(zoomscale, logicalPoint.X, logicalPoint.Y);
                if (Msi.ViewportWidth > 1) Msi.ViewportWidth = 1;
            }
        }

        /// <summary>
        /// Occurs when detaching the behavior
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.ManipulationStarted -= AssociatedObject_ManipulationStarted;
            AssociatedObject.ManipulationDelta -= AssociatedObject_ManipulationDelta;
            base.OnDetaching();
        }
    }
}