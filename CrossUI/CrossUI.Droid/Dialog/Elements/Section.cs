// Section.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Android.Content;
using Android.Views;
using Android.Widget;
using CrossUI.Core;

namespace CrossUI.Droid.Dialog.Elements
{
    public class Section : Element, IEnumerable<Element>
    {
        public IList<Element> Elements { get; private set; }

        private readonly List<string> ElementTypes = new List<string>();

        private object footer;
        private object header;

        /// <summary>
        ///  Constructs a Section without header or footers and an hidden section block
        /// </summary>
        public Section()
            : this((string) null)
        {
        }

        /// <summary>
        ///  Constructs a Section with the specified header
        /// </summary>
        /// <param name="caption">
        /// The header to display
        /// </param>
        public Section(string caption)
            : base(caption)
        {
            Elements = new ObservableCollection<Element>();
            ((ObservableCollection<Element>)Elements).CollectionChanged +=OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ParentAddedElements(args);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OrphanRemovedElements(args);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    OrphanRemovedElements(args);
                    ParentAddedElements(args);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
#warning should we throw an exception here?
                    DialogTrace.WriteLine("Warning - Reset seen - not expecting this - our dialog section may go very wrong now!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            HandleElementsChangedEvent();
        }

        private void OrphanRemovedElements(NotifyCollectionChangedEventArgs args)
        {
            foreach (Element element in args.OldItems)
            {
                element.Parent = null;

                // bind value changed to our local handler so section itself is aware of events, allows cascacding upward notifications
                if (element is ValueElement)
                    (element as ValueElement).ValueChanged -= HandleValueChangedEvent;
            }
        }

        private void ParentAddedElements(NotifyCollectionChangedEventArgs args)
        {
            foreach (Element element in args.NewItems)
            {
                element.Parent = this;

                // bind value changed to our local handler so section itself is aware of events, allows cascacding upward notifications
                if (element is ValueElement)
                    (element as ValueElement).ValueChanged += HandleValueChangedEvent;
            }
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
        public Section(string caption, string footer)
            : this(caption)
        {
            Footer = footer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="header">The header as an <see cref="Element"/>.</param>
        /// <remarks>The header can be customized as a custom <see cref="View"/> with a <see cref="ViewElement"/></remarks>
        public Section(Element header)
            : this()
        {
            HeaderView = header;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="header">The header, either a <see cref="string"/> for a simple header, or a custom <see cref="Element"/> (likely a <see cref="ViewElement"/>).</param>
        /// <param name="footer">The footer, either a <see cref="string"/> for a simple footer, or a custom <see cref="Element"/> (likely a <see cref="ViewElement"/>).</param>
        public Section(object header, object footer)
            : this()
        {
            this.header = header;
            this.footer = footer;
        }

        /// <summary>
        ///    The section header, as a string
        /// </summary>
        public string Header
        {
            get { return Caption; }
            set { Caption = value; }
        }

        /// <summary>
        /// The section footer, as a string.
        /// </summary>
        public string Footer
        {
            get { return footer == null ? null : footer.ToString(); }
            set { footer = value; }
        }

        /// <summary>
        /// The section's header view.  
        /// </summary>
        public Element HeaderView
        {
            get { return header as Element; }
            set { header = value; }
        }

        /// <summary>
        /// The section's footer view.
        /// </summary>
        public Element FooterView
        {
            get { return footer as Element; }
            set { footer = value; }
        }

        public int Count
        {
            get { return Elements.Count; }
        }

        public Element this[int idx]
        {
            get { return Elements[idx]; }
        }

        public event EventHandler ValueChanged;

        protected void HandleValueChangedEvent(object sender, EventArgs args)
        {
            var handler = ValueChanged;
            if (handler != null)
                handler(sender, args);
        }

        public event EventHandler ElementsChanged;
        protected void HandleElementsChangedEvent()
        {
            var handler = ElementsChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
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

            var elementType = element.GetType().FullName;

            if (!ElementTypes.Contains(elementType))
                ElementTypes.Add(elementType);

            Elements.Add(element);
        }

        /// <summary>Add version that can be used with LINQ</summary>
        /// <param name="elements">
        /// An enumerable list that can be produced by something like:
        ///    from x in ... select (Element) new MyElement (...)
        /// </param>
        public int Add(IEnumerable<Element> elements)
        {
            int count = 0;
            foreach (Element e in elements)
            {
                Add(e);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Inserts a series of elements into the Section
        /// </summary>
        /// <param name="idx">
        /// The index where the elements are inserted
        /// </param>
        /// <param name="newElements">
        /// A series of elements.
        /// </param>
        public void Insert(int idx, params Element[] newElements)
        {
            if (newElements == null)
                return;

            foreach (var e in newElements)
            {
                Elements.Insert(idx++, e);
            }
        }

        /// <summary>
        /// Inserts a <see cref="IEnumerable{T}"/> of Elements into the Section
        /// </summary>
        /// <param name="idx">The index to insert the elements.</param>
        /// <param name="newElements">A series of elements.</param>
        /// <returns></returns>
        public int Insert(int idx, IEnumerable<Element> newElements)
        {
            if (newElements == null)
                return 0;

            int count = 0;
            foreach (var e in newElements)
            {
                Elements.Insert(idx++, e);
                count++;
            }

            return count;
        }

        public void Remove(Element e)
        {
            if (e == null)
                return;
            for (int i = Elements.Count; i > 0;)
            {
                i--;
                if (Elements[i] != e) continue;
                RemoveRange(i, 1);
                return;
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
            if (start < 0 || start >= Elements.Count)
                return;
            if (count == 0)
                return;


            if (start + count > Elements.Count)
                count = Elements.Count - start;

            for (; count > 0; count--)
            {
                Elements.RemoveAt(start);
            } 
            
            //var root = Parent as RootElement;
            //if (root == null)
            //    return;

            //int sidx = root.IndexOf(this);
            //var paths = new NSIndexPath[count];
            //for (int i = 0; i < count; i++)
            //    paths[i] = NSIndexPath.FromRowSection(start + i, sidx);
            //root.TableView.DeleteRows(paths, anim);
        }

        public void Clear()
        {
            foreach (Element e in Elements)
                e.Dispose();
            Elements.Clear();

            //var root = Parent as RootElement;
            //if (root != null && root.TableView != null)
            //    root.TableView.ReloadData();
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

        public int GetElementViewType(Element e)
        {
            var elementType = e.GetType().FullName;

            for (int i = 0; i < ElementTypes.Count; i++)
            {
                if (ElementTypes[i].Equals(elementType))
                    return i + 1;
            }

            return 0;
        }

        public int ElementViewTypeCount
        {
            get { return ElementTypes.Count; }
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            if (HeaderView != null)
            {
                return (HeaderView).GetView(context, null, parent);
            }

            if (Caption != null)
            {
                var view = new TextView(context, null, Android.Resource.Attribute.ListSeparatorTextViewStyle);
                if (Caption.Length > 0)
                {
                    view.Text = Caption;
                    view.Visibility = ViewStates.Visible;
                }
                else
                {
                    view.Text = string.Empty;
                    view.Visibility = ViewStates.Visible;
                }
                return view;
            }

            // invisible/empty section header, could be re-shown by setting the caption and refreshing the list
            return new View(context, null)
            {
                LayoutParameters = new ListView.LayoutParams(ViewGroup.LayoutParams.FillParent, 0),
                Visibility = ViewStates.Gone,
            };
        }

        public View GetFooterView(Context context, View convertView, ViewGroup parent)
        {
            if (FooterView != null)
            {
                return (FooterView).GetView(context, convertView, parent);
            }

            if (Footer != null)
            {
                var view = (convertView as TextView) ?? new TextView(context);
                view.Gravity = GravityFlags.Center;
                if (Footer.Length > 0)
                {
                    view.Text = Footer;
                    view.Visibility = ViewStates.Visible;
                }
                else
                {
                    view.Text = string.Empty;
                    view.Visibility = ViewStates.Visible;
                }
                return view;
            }

            // invisible/empty section footer, could be re-shown by setting the footer and refreshing the list
            return new View(context, null)
            {
                LayoutParameters = new ListView.LayoutParams(ViewGroup.LayoutParams.FillParent, 0),
                Visibility = ViewStates.Gone,
            };
        }

        /// <summary>
        /// Enumerator to get all the elements in the Section.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/>
        /// </returns>
        public IEnumerator<Element> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        /// <summary>
        /// Enumerator to get all the elements in the Section.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/>
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }
    }
}
