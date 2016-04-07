using JASidePanels;

namespace MvvmCross.iOS.Support.JASidePanels
{
    using UIKit;

    public class MvxMultiPanelController : JASidePanelController
    {
        /// <summary>
        /// Method s simply overridden to remove any styling such as the default corner radius.
        /// </summary>
        /// <param name="panel">The panel.</param>
        public override void StylePanel(UIView panel)
        {
			
        }
    }
}