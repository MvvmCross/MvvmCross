// RootElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using Android.Views;
using CrossUI.Core;
using CrossUI.Core.Elements.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace CrossUI.Droid.Dialog.Elements
{
    public class RootElement : StringDisplayingValueElement<string>, IEnumerable<Section>,
                               IDialogInterfaceOnClickListener
    {
        private Group _group;

        public Group Group
        {
            get { return _group; }
            set { _group = value; }
        }

        public bool UnevenRows { get; set; }

        public Func<RootElement, View> _createOnSelected;

        public event EventHandler RadioSelectedChanged;

        public RootElement()
            : this(null)
        {
        }

        public RootElement(string caption = null, Group group = null, string layoutRoot = null)
            : base(caption, null, layoutRoot ?? "dialog_root")
        {
            this._group = group;
            Click = (o, e) => SelectRadio();
            Sections = new ObservableCollection<Section>();
            ((ObservableCollection<Section>)Sections).CollectionChanged += OnCollectionChanged;
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
                    DialogTrace.WriteLine("Warning - Reset seen - not expecting this - our dialog may go very wrong now!");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            HandleElementsChangedEvent(this, args);
            ActOnCurrentAttachedCell(UpdateDetailDisplay);
        }

        private void OrphanRemovedElements(NotifyCollectionChangedEventArgs args)
        {
            foreach (Section section in args.OldItems)
            {
                if (section.Parent == this)
                    section.Parent = null;

                section.ValueChanged -= HandleValueChangedEvent;
                section.ElementsChanged -= HandleElementsChangedEvent;
            }
        }

        private void ParentAddedElements(NotifyCollectionChangedEventArgs args)
        {
            foreach (Section section in args.NewItems)
            {
                if (section.Parent != this)
                    section.Parent = this;
                // bind value changed to our local handler so section itself is aware of events, allows cascacding upward notifications
                section.ValueChanged += HandleValueChangedEvent;
                section.ElementsChanged += HandleElementsChangedEvent;
            }
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            RefreshValue();
            return base.GetViewImpl(context, parent);
        }

        private void RefreshValue()
        {
            Value = GetSelectedValue() ?? Caption;
        }

        public IList<Section> Sections { get; private set; }

        public int Count => Sections.Count;

        public Section this[int idx] => Sections[idx];

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            base.FireValueChanged();
        }

        public event EventHandler ElementsChanged;

        protected void HandleElementsChangedEvent(object sender, EventArgs eventArgs)
        {
            var handler = ElementsChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        internal int IndexOf(Section target)
        {
            int idx = 0;
            foreach (var s in Sections)
            {
                if (s == target)
                    return idx;
                idx++;
            }
            return -1;
        }

        internal void Prepare()
        {
            int current = 0;
            foreach (var element in Sections.SelectMany(s => s))
            {
                var re = element as RadioElement;
                if (re != null)
                    re.RadioIdx = current++;
                if (UnevenRows == false && element is IElementSizing)
                    UnevenRows = true;
            }
        }

        public override string Summary()
        {
            return GetSelectedValue();
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
        ///    root using the specified animation.
        /// </remarks>
        public void Insert(int idx, params Section[] newSections)
        {
            if (idx < 0 || idx > Sections.Count)
                return;
            if (newSections == null)
                return;

            //if (Table != null)
            //    Table.BeginUpdates();

            int pos = idx;
            foreach (var s in newSections)
            {
                Sections.Insert(pos++, s);
            }
        }

        /// <summary>
        /// Removes a section at a specified location
        /// </summary>
        public void RemoveAt(int idx)
        {
            if (idx < 0 || idx >= Sections.Count)
                return;

            Sections.RemoveAt(idx);
        }

        public void Remove(Section s)
        {
            if (s == null)
                return;
            int idx = Sections.IndexOf(s);
            if (idx == -1)
                return;
            RemoveAt(idx);
        }

        public void Clear()
        {
            foreach (var s in Sections)
            {
                s.Dispose();
            }
            Sections.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Sections != null)
                {
                    Clear();
                    Sections = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The currently selected Radio item in the whole Root.
        /// </summary>
        public int RadioSelected
        {
            get
            {
                var radio = _group as RadioGroup;
                if (radio != null)
                    return radio.Selected;
                return -1;
            }
            set
            {
                var radio = _group as RadioGroup;
                if (radio != null)
                    radio.Selected = value;
            }
        }

        private string GetSelectedValue()
        {
            var radio = _group as RadioGroup;
            if (radio == null)
                return string.Empty;

            int selected = radio.Selected;
            int current = 0;
            foreach (RadioElement e in Sections
                                        .SelectMany(s => s)
                                        .Where(e => e is IRadioElement))
            {
                if (current == selected)
                    return e.Summary();

                current++;
            }

            return string.Empty;
        }

        protected override string Format(string value)
        {
            return value;
        }

        public void SelectRadio()
        {
            var radio = _group as RadioGroup;

            if (radio == null)
                return;

            var dialog = new AlertDialog.Builder(Context);
            dialog.SetSingleChoiceItems(
                Sections.SelectMany(s => s)
                        .Where(e => e is IRadioElement)
                        .Select(e => e.Summary()).ToArray(), RadioSelected,
                this);
            dialog.SetTitle(Caption);
            dialog.SetNegativeButton("Cancel", this);
            dialog.Create().Show();
        }

        void IDialogInterfaceOnClickListener.OnClick(IDialogInterface dialog, int which)
        {
            if (which >= 0 && RadioSelected != which)
            {
                RadioSelected = which;
                var radioValue = GetSelectedValue();
#warning This radio selection is a bit of a mess currently - both radio value and RadioSelected change...
                var handler = RadioSelectedChanged;
                handler?.Invoke(this, EventArgs.Empty);
                OnUserValueChanged(radioValue);
            }

            dialog.Dismiss();
        }

        /// <summary>
        /// Enumerator that returns all the sections in the RootElement.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator"/>
        /// </returns>
        public IEnumerator<Section> GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

        /// <summary>
        /// Enumerator that returns all the sections in the RootElement.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Sections.GetEnumerator();
        }
    }
}