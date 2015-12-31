// Section.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;
using UIKit;

namespace CrossUI.iOS.Dialog.Elements
{
    /// <summary>
    /// Sections contain individual Element instances that are rendered by MonoTouch.Dialog
    /// </summary>
    /// <remarks>
    /// Sections are used to group elements in the screen and they are the
    /// only valid direct child of the RootElement.    Sections can contain
    /// any of the standard elements, including new RootElements.
    ///
    /// RootElements embedded in a section are used to navigate to a new
    /// deeper level.
    ///
    /// You can assign a header and a footer either as strings (Header and Footer)
    /// properties, or as UIViews to be shown (HeaderView and FooterView).   Internally
    /// this uses the same storage, so you can only show one or the other.
    /// </remarks>
    public class Section : Element, IEnumerable
    {
        // , ISection {
        private object _header;

        private object _footer;
        private List<Element> _elements = new List<Element>();

        public List<Element> Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }

        // X corresponds to the alignment, Y to the height of the password
        public CGSize EntryAlignment { get; set; }

        /// <summary>
        ///  Constructs a Section without header or footers.
        /// </summary>
        public Section() : base(null)
        {
            Elements = new List<Element>();
        }

        /// <summary>
        ///  Constructs a Section with the specified header
        /// </summary>
        /// <param name="caption">
        /// The header to display
        /// </param>
        public Section(string caption) : base(caption)
        {
            Elements = new List<Element>();
        }

        /// <summary>
        /// Constructs a Section with a header and a footer
        /// </summary>
        /// <param name="caption">
        /// The caption to display (or null to not display a caption)
        /// </param>
        /// <param name="footer">
        /// The footer to display.
        /// </param>
        public Section(string caption, string footer) : base(caption)
        {
            Footer = footer;
            Elements = new List<Element>();
        }

        public Section(UIView header) : base(null)
        {
            HeaderView = header;
            Elements = new List<Element>();
        }

        public Section(UIView header, UIView footer) : base(null)
        {
            HeaderView = header;
            FooterView = footer;
            Elements = new List<Element>();
        }

        /// <summary>
        ///    The section header, as a string
        /// </summary>
        public string Header
        {
            get { return _header as string; }
            set { _header = value; }
        }

        /// <summary>
        /// The section footer, as a string.
        /// </summary>
        public string Footer
        {
            get { return _footer as string; }

            set { _footer = value; }
        }

        /// <summary>
        /// The section's header view.
        /// </summary>
        public UIView HeaderView
        {
            get { return _header as UIView; }
            set { _header = value; }
        }

        /// <summary>
        /// The section's footer view.
        /// </summary>
        public UIView FooterView
        {
            get { return _footer as UIView; }
            set { _footer = value; }
        }

        /// <summary>
        /// Adds a new child Element to the Section
        /// </summary>
        /// <param name="element">
        /// An element to add to the section.
        /// </param>
        public void Add(Element element)
        {
            if (element == null)
                return;

            Elements.Add(element);
            element.Parent = this;

            if (Parent != null)
                InsertVisual(Elements.Count - 1, UITableViewRowAnimation.None, 1);
        }

        /// <summary>
        ///    Add version that can be used with LINQ
        /// </summary>
        /// <param name="elements">
        /// An enumerable list that can be produced by something like:
        ///    from x in ... select (Element) new MyElement (...)
        /// </param>
        public int AddAll(IEnumerable<Element> elements)
        {
            int count = 0;
            foreach (var e in elements)
            {
                Add(e);
                count++;
            }
            return count;
        }

        /// <summary>
        ///    This method is being obsoleted, use AddAll to add an IEnumerable<Element> instead.
        /// </summary>
        [Obsolete(
            "Please use AddAll since this version will not work in future versions of MonoTouch when we introduce 4.0 covariance"
            )]
        public int Add(IEnumerable<Element> elements)
        {
            return AddAll(elements);
        }

        /// <summary>
        /// Use to add a UIView to a section, it makes the section opaque, to
        /// get a transparent one, you must manually call UIViewElement
        /// </summary>
        public void Add(UIView view)
        {
            if (view == null)
                return;
            Add(new UIViewElement(null, view, false));
        }

        /// <summary>
        ///   Adds the UIViews to the section.
        /// </summary>
        /// <param name="views">
        /// An enumarable list that can be produced by something like:
        ///    from x in ... select (UIView) new UIFoo ();
        /// </param>
        public void Add(IEnumerable<UIView> views)
        {
            foreach (var v in views)
                Add(v);
        }

        /// <summary>
        /// Inserts a series of elements into the Section using the specified animation
        /// </summary>
        /// <param name="idx">
        /// The index where the elements are inserted
        /// </param>
        /// <param name="anim">
        /// The animation to use
        /// </param>
        /// <param name="newElements">
        /// A series of elements.
        /// </param>
        public void Insert(int idx, UITableViewRowAnimation anim, params Element[] newElements)
        {
            if (newElements == null)
                return;

            int pos = idx;
            foreach (var e in newElements)
            {
                Elements.Insert(pos++, e);
                e.Parent = this;
            }
            var root = Parent as RootElement;
            if (Parent != null && root?.TableView != null)
            {
                if (anim == UITableViewRowAnimation.None)
                    root.TableView.ReloadData();
                else
                    InsertVisual(idx, anim, newElements.Length);
            }
        }

        public int Insert(int idx, UITableViewRowAnimation anim, IEnumerable<Element> newElements)
        {
            if (newElements == null)
                return 0;

            int pos = idx;
            int count = 0;
            foreach (var e in newElements)
            {
                Elements.Insert(pos++, e);
                e.Parent = this;
                count++;
            }
            var root = Parent as RootElement;
            if (root?.TableView != null)
            {
                if (anim == UITableViewRowAnimation.None)
                    root.TableView.ReloadData();
                else
                    InsertVisual(idx, anim, pos - idx);
            }
            return count;
        }

        private void InsertVisual(int idx, UITableViewRowAnimation anim, int count)
        {
            var root = Parent as RootElement;

            if (root?.TableView == null)
                return;

            int sidx = root.IndexOf(this);
            var paths = new NSIndexPath[count];
            for (int i = 0; i < count; i++)
                paths[i] = NSIndexPath.FromRowSection(idx + i, sidx);

            root.TableView.InsertRows(paths, anim);
        }

        public void Insert(int index, params Element[] newElements)
        {
            Insert(index, UITableViewRowAnimation.None, newElements);
        }

        public void Remove(Element e)
        {
            if (e == null)
                return;
            for (int i = Elements.Count; i > 0;)
            {
                i--;
                if (Elements[i] == e)
                {
                    RemoveRange(i, 1);
                    return;
                }
            }
        }

        public void Remove(int idx)
        {
            RemoveRange(idx, 1);
        }

        /// <summary>
        /// Removes a range of elements from the Section
        /// </summary>
        /// <param name="start">
        /// Starting position
        /// </param>
        /// <param name="count">
        /// Number of elements to remove from the section
        /// </param>
        public void RemoveRange(int start, int count)
        {
            RemoveRange(start, count, UITableViewRowAnimation.Fade);
        }

        /// <summary>
        /// Remove a range of elements from the section with the given animation
        /// </summary>
        /// <param name="start">
        /// Starting position
        /// </param>
        /// <param name="count">
        /// Number of elements to remove form the section
        /// </param>
        /// <param name="anim">
        /// The animation to use while removing the elements
        /// </param>
        public void RemoveRange(int start, int count, UITableViewRowAnimation anim)
        {
            if (start < 0 || start >= Elements.Count)
                return;
            if (count == 0)
                return;

            var root = Parent as RootElement;

            if (start + count > Elements.Count)
                count = Elements.Count - start;

            Elements.RemoveRange(start, count);

            if (root?.TableView == null)
                return;

            int sidx = root.IndexOf(this);
            var paths = new NSIndexPath[count];
            for (int i = 0; i < count; i++)
                paths[i] = NSIndexPath.FromRowSection(start + i, sidx);
            root.TableView.DeleteRows(paths, anim);
        }

        /// <summary>
        /// Enumerator to get all the elements in the Section.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator"/>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            foreach (var e in Elements)
                yield return e;
        }

        public int Count => Elements.Count;

        public Element this[int idx] => Elements[idx];

        public void Clear()
        {
            if (Elements != null)
            {
                foreach (var e in Elements)
                    e.Dispose();
            }
            Elements = new List<Element>();

            var root = Parent as RootElement;
            root?.TableView?.ReloadData();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Parent = null;
                Clear();
                Elements = null;
            }
            base.Dispose(disposing);
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = new UITableViewCell(UITableViewCellStyle.Default, "");
            cell.TextLabel.Text = "Section was used for Element";

            return cell;
        }
    }
}