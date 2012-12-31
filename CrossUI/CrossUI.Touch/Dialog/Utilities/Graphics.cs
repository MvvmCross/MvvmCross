#region Copyright

// <copyright file="Graphics.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Drawing;
using MonoTouch.CoreGraphics;

namespace CrossUI.Touch.Dialog.Utilities
{
    public static class GraphicsUtil
    {
        /// <summary>
        ///    Creates a path for a rectangle with rounded corners
        /// </summary>
        /// <param name="rect">
        /// The <see cref="RectangleF"/> rectangle bounds
        /// </param>
        /// <param name="radius">
        /// The <see cref="System.Single"/> size of the rounded corners
        /// </param>
        /// <returns>
        /// A <see cref="CGPath"/> that can be used to stroke the rounded rectangle
        /// </returns>
        public static CGPath MakeRoundedRectPath(RectangleF rect, float radius)
        {
            float minx = rect.Left;
            float midx = rect.Left + (rect.Width)/2;
            float maxx = rect.Right;
            float miny = rect.Top;
            float midy = rect.Y + rect.Size.Height/2;
            float maxy = rect.Bottom;

            var path = new CGPath();
            path.MoveToPoint(minx, midy);
            path.AddArcToPoint(minx, miny, midx, miny, radius);
            path.AddArcToPoint(maxx, miny, maxx, midy, radius);
            path.AddArcToPoint(maxx, maxy, midx, maxy, radius);
            path.AddArcToPoint(minx, maxy, minx, midy, radius);
            path.CloseSubpath();

            return path;
        }

        public static void FillRoundedRect(CGContext ctx, RectangleF rect, float radius)
        {
            var p = GraphicsUtil.MakeRoundedRectPath(rect, radius);
            ctx.AddPath(p);
            ctx.FillPath();
        }

        public static CGPath MakeRoundedPath(float size, float radius)
        {
            float hsize = size/2;

            var path = new CGPath();
            path.MoveToPoint(size, hsize);
            path.AddArcToPoint(size, size, hsize, size, radius);
            path.AddArcToPoint(0, size, 0, hsize, radius);
            path.AddArcToPoint(0, 0, hsize, 0, radius);
            path.AddArcToPoint(size, 0, size, hsize, radius);
            path.CloseSubpath();

            return path;
        }
    }
}