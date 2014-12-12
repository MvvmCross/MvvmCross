using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Helpers
{
    public static class PopupHelper
    {
        /// <summary>
        /// Gets a value indicating whether a popup is currently visible. 
        /// </summary>
        public static bool IsPopupVisible
        {
            get { return VisualTreeHelper.GetOpenPopups(Window.Current).Any(); }
        }

        /// <summary>
        /// Gets the parent popup of the given element or null if it is not contained in a popup. 
        /// </summary>
        public static Popup GetParentPopup(FrameworkElement element)
        {
            return element.GetVisualAncestors().LastOrDefault() as Popup;
        }

        /// <summary>
        /// Returns true if the element is contained in a popup. 
        /// </summary>
        public static bool IsInPopup(FrameworkElement element)
        {
            if (element is Popup)
                return true;

            return GetParentPopup(element) != null;
        }     
    }
}