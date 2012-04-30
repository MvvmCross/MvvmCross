using System.Windows;
using System.Windows.Controls;

namespace Phone7.Fx.Extensions
{
    /// <summary>
    /// Thanks to stephan cr for this class: http://phone.codeplex.com/
    /// </summary>
    public static class UIElementCollectionExtensions
    {
        /// <summary>
        /// Gets the item position.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static double GetItemPosition(this UIElementCollection items, int index)
        {
            double position = 0.0;
            if ((index >= 0) && (index < items.Count))
            {
                for (int i = 0; i != index; i++)
                {
                    FrameworkElement item = (FrameworkElement)items[i];
                    if (null != item)
                        position += item.ActualWidth;
                }
            }

            return position;
        }
    }
}