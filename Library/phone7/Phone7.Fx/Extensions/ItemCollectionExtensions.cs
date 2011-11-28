using System.Windows;
using System.Windows.Controls;

namespace Phone7.Fx.Extensions
{
    /// <summary>
    /// Thanks to stephan cr for this class: http://phone.codeplex.com/
    /// </summary>
    public static class ItemCollectionExtensions
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static FrameworkElement GetItem(this ItemCollection items, int index)
        {
            if ((index >= 0) && (index < items.Count))
                return (FrameworkElement)items[index];

            return null;
        }

        /// <summary>
        /// Gets the last item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static FrameworkElement GetLastItem(this ItemCollection items)
        {
            if (items.Count == 0)
                return null;

            return (FrameworkElement)items[items.Count - 1];
        }

        /// <summary>
        /// Gets the index of position.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static int GetIndexOfPosition(this ItemCollection items, double position)
        {
            if (items.Count == 0)
                return -1;

            // far left : back to last item
            if (position < 0)
                return items.Count - 1;

            double start = 0.0;
            for (int i = 0; i < items.Count; i++)
            {
                FrameworkElement item = (FrameworkElement)items[i];
                if ((position >= start) && (position < start + item.Width))
                    return i;

                start += item.Width;
            }

            // far right : assume first
            return 0;
        }

        /// <summary>
        /// Gets the item position.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static double GetItemPosition(this ItemCollection items, int index)
        {
            double position = 0.0;
            if ((index >= 0) && (index < items.Count))
            {
                for (int i = 0; i != index; i++)
                {
                    FrameworkElement item = items.GetItem(i);
                    if (null != item)
                        position += item.Width;
                }
            }

            return position;
        }

        /// <summary>
        /// Gets the last item position.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static double GetLastItemPosition(this ItemCollection items)
        {
            return items.GetItemPosition(items.Count - 1);
        }

        /// <summary>
        /// Gets the width of the item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static double GetItemWidth(this ItemCollection items, int index)
        {
            FrameworkElement item = items.GetItem(index);
            if (null != item)
                return item.Width;

            return 0.0;
        }

        /// <summary>
        /// Gets the total width.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static double GetTotalWidth(this ItemCollection items)
        {
            FrameworkElement item = items.GetLastItem();
            if (null == item)
                return 0.0;

            return items.GetLastItemPosition() + item.Width;
        }
    }
}