// RootElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    /// <summary>
    ///    RootElements are responsible for showing a full configuration page.
    /// </summary>
    /// <remarks>
    ///    At least one RootElement is required to start the MonoTouch.Dialogs
    ///    process.
    ///
    ///    RootElements can also be used inside Sections to trigger
    ///    loading a new nested configuration page.   When used in this mode
    ///    the caption provided is used while rendered inside a section and
    ///    is also used as the Title for the subpage.
    ///
    ///    If a RootElement is initialized with a section/element value then
    ///    this value is used to locate a child Element that will provide
    ///    a summary of the configuration which is rendered on the right-side
    ///    of the display.
    ///
    ///    RootElements are also used to coordinate radio elements.  The
    ///    RadioElement members can span multiple Sections (for example to
    ///    implement something similar to the ring tone selector and separate
    ///    custom ring tones from system ringtones).
    ///
    ///    Sections are added by calling the Add method which supports the
    ///    C# 4.0 syntax to initialize a RootElement in one pass.
    /// </remarks>
    public class RootElement : Element, IEnumerable, IEnumerable<Section>
    {
        private static readonly NSString rkey1 = new NSString("RootElement1");
        private static readonly NSString rkey2 = new NSString("RootElement2");
        private readonly int _summarySection;
        private readonly int _summaryElement;
        public Group Group { get; set; }
        public bool UnevenRows { get; set; }
        public Func<RootElement, UIViewController> CreateOnSelected;
        public UITableView TableView;

        // This is used to indicate that we need the DVC to dispatch calls to
        // WillDisplayCell so we can prepare the color of the cell before
        // display
        public bool NeedColorUpdate { get; set; }

        /// <summary>
        ///  Initializes a RootSection with a caption
        /// </summary>
        /// <param name="caption">
        ///  The caption to render.
        /// </param>
        public RootElement(string caption = "") : base(caption)
        {
            _summarySection = -1;
            Sections = new List<Section>();
        }

        /// <summary>
        /// Initializes a RootSection with a caption and a callback that will
        /// create the nested UIViewController that is activated when the user
        /// taps on the element.
        /// </summary>
        /// <param name="caption">
        ///  The caption to render.
        /// </param>
        public RootElement(string caption, Func<RootElement, UIViewController> createOnSelected) : base(caption)
        {
            _summarySection = -1;
            this.CreateOnSelected = createOnSelected;
            Sections = new List<Section>();
        }

        /// <summary>
        ///   Initializes a RootElement with a caption with a summary fetched from the specified section and leement
        /// </summary>
        /// <param name="caption">
        /// The caption to render cref="System.String"/>
        /// </param>
        /// <param name="section">
        /// The section that contains the element with the summary.
        /// </param>
        /// <param name="element">
        /// The element index inside the section that contains the summary for this RootSection.
        /// </param>
        public RootElement(string caption, int section, int element) : base(caption)
        {
            _summarySection = section;
            _summaryElement = element;
            Sections = new List<Section>();
        }

        /// <summary>
        /// Initializes a RootElement that renders the summary based on the radio settings of the contained elements.
        /// </summary>
        /// <param name="caption">
        /// The caption to ender
        /// </param>
        /// <param name="group">
        /// The group that contains the checkbox or radio information.  This is used to display
        /// the summary information when a RootElement is rendered inside a section.
        /// </param>
        public RootElement(string caption, Group group) : base(caption)
        {
            this.Group = group;
            Sections = new List<Section>();
        }

        public List<Section> Sections { get; set; }

        internal NSIndexPath PathForRadio(int idx)
        {
            var radio = Group as RadioGroup;
            if (radio == null)
                return null;

            uint current = 0, section = 0;
            foreach (Section s in Sections)
            {
                uint row = 0;

                foreach (Element e in s.Elements)
                {
                    if (!(e is RadioElement))
                        continue;

                    if (current == idx)
                    {
                        return NSIndexPath.Create(section, row);
                    }
                    row++;
                    current++;
                }
                section++;
            }
            return null;
        }

        public int Count => Sections.Count;

        public Section this[int idx] => Sections[idx];

        internal int IndexOf(Section target)
        {
            int idx = 0;
            foreach (Section s in Sections)
            {
                if (s == target)
                    return idx;
                idx++;
            }
            return -1;
        }

        public void Prepare()
        {
            int current = 0;
            foreach (Section s in Sections)
            {
                foreach (Element e in s.Elements)
                {
                    var re = e as RadioElement;
                    if (re != null)
                        re.RadioIdx = current++;
                    if (UnevenRows == false && e is IElementSizing)
                        UnevenRows = true;
                    if (NeedColorUpdate == false && e is IColorizeBackground)
                        NeedColorUpdate = true;
                }
            }
        }

        /// <summary>
        /// Adds a new section to this RootElement
        /// </summary>
        /// <param name="section">
        /// The section to add, if the root is visible, the section is inserted with no animation
        /// </param>
        public void Add(Section section)
        {
            if (section == null)
                return;

            Sections.Add(section);
            section.Parent = this;

            TableView?.InsertSections(MakeIndexSet(Sections.Count - 1, 1), UITableViewRowAnimation.None);
        }

        //
        // This makes things LINQ friendly;  You can now create RootElements
        // with an embedded LINQ expression, like this:
        // new RootElement ("Title") {
        //     from x in names
        //         select new Section (x) { new StringElement ("Sample") }
        //
        public void Add(IEnumerable<Section> sections)
        {
            foreach (var s in sections)
                Add(s);
        }

        private NSIndexSet MakeIndexSet(int start, int count)
        {
            NSRange range;
            range.Location = start;
            range.Length = count;
            return NSIndexSet.FromNSRange(range);
        }

        /// <summary>
        /// Inserts a new section into the RootElement
        /// </summary>
        /// <param name="idx">
        /// The index where the section is added <see cref="System.Int32"/>
        /// </param>
        /// <param name="anim">
        /// The <see cref="UITableViewRowAnimation"/> type.
        /// </param>
        /// <param name="newSections">
        /// A <see cref="Section[]"/> list of sections to insert
        /// </param>
        /// <remarks>
        ///    This inserts the specified list of sections (a params argument) into the
        ///    root using the specified animation.
        /// </remarks>
        public void Insert(int idx, UITableViewRowAnimation anim, params Section[] newSections)
        {
            if (idx < 0 || idx > Sections.Count)
                return;
            if (newSections == null)
                return;

            TableView?.BeginUpdates();

            int pos = idx;
            foreach (var s in newSections)
            {
                s.Parent = this;
                Sections.Insert(pos++, s);
            }

            if (TableView == null)
                return;

            TableView.InsertSections(MakeIndexSet(idx, newSections.Length), anim);
            TableView.EndUpdates();
        }

        /// <summary>
        /// Inserts a new section into the RootElement
        /// </summary>
        /// <param name="idx">
        /// The index where the section is added <see cref="System.Int32"/>
        /// </param>
        /// <param name="newSections">
        /// A <see cref="Section[]"/> list of sections to insert
        /// </param>
        /// <remarks>
        ///    This inserts the specified list of sections (a params argument) into the
        ///    root using the Fade animation.
        /// </remarks>
        public void Insert(int idx, Section section)
        {
            Insert(idx, UITableViewRowAnimation.None, section);
        }

        /// <summary>
        /// Removes a section at a specified location
        /// </summary>
        public void RemoveAt(int idx)
        {
            RemoveAt(idx, UITableViewRowAnimation.Fade);
        }

        /// <summary>
        /// Removes a section at a specified location using the specified animation
        /// </summary>
        /// <param name="idx">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <param name="anim">
        /// A <see cref="UITableViewRowAnimation"/>
        /// </param>
        public void RemoveAt(int idx, UITableViewRowAnimation anim)
        {
            if (idx < 0 || idx >= Sections.Count)
                return;

            Sections.RemoveAt(idx);

            TableView?.DeleteSections(NSIndexSet.FromIndex(idx), anim);
        }

        public void Remove(Section s)
        {
            if (s == null)
                return;
            int idx = Sections.IndexOf(s);
            if (idx == -1)
                return;
            RemoveAt(idx, UITableViewRowAnimation.Fade);
        }

        public void Remove(Section s, UITableViewRowAnimation anim)
        {
            if (s == null)
                return;
            int idx = Sections.IndexOf(s);
            if (idx == -1)
                return;
            RemoveAt(idx, anim);
        }

        public void Clear()
        {
            foreach (var s in Sections)
                s.Dispose();
            Sections = new List<Section>();
            TableView?.ReloadData();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Sections == null)
                    return;

                TableView = null;
                Clear();
                Sections = null;
            }
        }

        /// <summary>
        /// Enumerator that returns all the sections in the RootElement.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator"/>
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var s in Sections)
                yield return s;
        }

        IEnumerator<Section> IEnumerable<Section>.GetEnumerator()
        {
            foreach (var s in Sections)
                yield return s;
        }

        /// <summary>
        /// The currently selected Radio item in the whole Root.
        /// </summary>
        public int RadioSelected
        {
            get
            {
                var radio = Group as RadioGroup;
                if (radio != null)
                    return radio.Selected;
                return -1;
            }
            set
            {
#warning More needed here for two way binding!
                var radio = Group as RadioGroup;
                if (radio != null)
                    radio.Selected = value;
                var handler = RadioSelectedChanged;
                if (handler != null)
                    RadioSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler RadioSelectedChanged;

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            NSString key = _summarySection == -1 ? rkey1 : rkey2;
            var cell = tv.DequeueReusableCell(key);
            if (cell == null)
            {
                var style = _summarySection == -1 && Group == null ?
                    UITableViewCellStyle.Default : UITableViewCellStyle.Value1;

                cell = new UITableViewCell(style, key) { SelectionStyle = UITableViewCellSelectionStyle.Blue };
            }

            cell.TextLabel.Text = Caption;
            var radio = Group as RadioGroup;
            if (radio != null)
            {
                int selected = radio.Selected;
                int current = 0;

                foreach (var s in Sections)
                {
                    foreach (var e in s.Elements)
                    {
                        if (!(e is RadioElement))
                            continue;

                        if (current == selected && cell.DetailTextLabel != null)
                        {
                            cell.DetailTextLabel.Text = e.Summary();
                            goto le;
                        }
                        current++;
                    }
                }
            }
            else if (Group != null)
            {
                int count = 0;

                foreach (var s in Sections)
                {
                    foreach (var e in s.Elements)
                    {
                        var ce = e as CheckboxElement;
                        if (ce != null)
                        {
                            if (ce.Value)
                                count++;
                            continue;
                        }
                        var be = e as ValueElement<bool>;
                        if (be != null)
                        {
                            if (be.Value)
                                count++;
                            continue;
                        }
                    }
                }
                cell.DetailTextLabel.Text = count.ToString();
            }
            else if (_summarySection != -1 && _summarySection < Sections.Count)
            {
                var s = Sections[_summarySection];
                if (_summaryElement < s.Elements.Count && cell.DetailTextLabel != null)
                    cell.DetailTextLabel.Text = s.Elements[_summaryElement].Summary();
            }
            le:
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            return cell;
        }

        /// <summary>
        ///    This method does nothing by default, but gives a chance to subclasses to
        ///    customize the UIViewController before it is presented
        /// </summary>
        protected virtual void PrepareDialogViewController(UIViewController dvc)
        {
        }

        /// <summary>
        /// Creates the UIViewController that will be pushed by this RootElement
        /// </summary>
        protected virtual UIViewController MakeViewController()
        {
            if (CreateOnSelected != null)
                return CreateOnSelected(this);

            return new DialogViewController(this, true)
            {
                Autorotate = true
            };
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            tableView.DeselectRow(path, false);
            var newDvc = MakeViewController();
            PrepareDialogViewController(newDvc);
            dvc.ActivateController(newDvc);
        }

        public void Reload(Section section, UITableViewRowAnimation animation)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));
            if (section.Parent == null || section.Parent != this)
                throw new ArgumentException("Section is not attached to this root");

            int idx = 0;
            foreach (var sect in Sections)
            {
                if (sect == section)
                {
                    TableView.ReloadSections(new NSIndexSet((uint)idx), animation);
                    return;
                }
                idx++;
            }
        }

        public void Reload(Element element, UITableViewRowAnimation animation)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            var section = element.Parent as Section;
            if (section == null)
                throw new ArgumentException("Element is not attached to this root");
            var root = section.Parent as RootElement;
            if (root == null)
                throw new ArgumentException("Element is not attached to this root");
            var path = element.IndexPath;
            if (path == null)
                return;
            TableView.ReloadRows(new[] { path }, animation);
        }
    }
}