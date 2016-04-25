using JASidePanels;
using MvvmCross.iOS.Support.SidePanels;

namespace MvvmCross.iOS.Support.JASidePanels
{
    using UIKit;

    public class MvxMultiPanelController : JASidePanelController, IMvxSideMenu
    {
        /// <summary>
        /// Method s simply overridden to remove any styling such as the default corner radius.
        /// </summary>
        /// <param name="panel">The panel.</param>
        public override void StylePanel(UIView panel)
        {
			
        }

        public void Close()
        {
            ShowCenterPanelAnimated(false);
        }

        public void Open(MvxPanelEnum panelEnum)
        {
            if (panelEnum == MvxPanelEnum.Left)
                ShowLeftPanelAnimated(false);
            else if (panelEnum == MvxPanelEnum.Right)
                ShowRightPanelAnimated(false);
            else
                ShowCenterPanelAnimated(false);
        }
    }
}