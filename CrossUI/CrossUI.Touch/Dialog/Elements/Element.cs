// Element.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core;
using CrossUI.Core.Elements.Dialog;
using Foundation;
using System;
using System.Windows.Input;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    /// <summary>
    /// Base class for all elements in MonoTouch.Dialog
    /// </summary>
    public class Element : IElement, IDisposable
    {
        private static int _currentElementID = 1;

        /// <summary>
        /// An app unique identifier for this element.
        /// Note that it is expected that Elements will always created on the UI thread - so no locking is used on CurrentElementID
        /// </summary>
        private readonly int _elementID = _currentElementID++;

        /// <summary>
        /// The last cell attached to this Tlement
        /// Use the Tag property of the Cell to determine of this cell is still attached to this Element
        /// </summary>
        private UITableViewCell _lastAttachedCell;

        /// <summary>
        /// General selection handler
        /// </summary>
        public event Action Tapped;

        public bool ShouldDeselectAfterTouch { get; set; }

        /// <summary>
        ///  Handle to the container object.
        /// </summary>
        /// <remarks>
        /// For sections this points to a RootElement, for every
        /// other object this points to a Section and it is null
        /// for the root RootElement.
        /// </remarks>
        public Element Parent { get; set; }

        /// <summary>
        /// The caption to display for this given element
        /// </summary>
        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption == value)
                    return;
                _caption = value;
                UpdateCaptionDisplay(CurrentAttachedCell);
            }
        }

        private bool _visible = true;

        /// <summary>
        ///  Whether or not to display this element
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible == value)
                    return;
                _visible = value;

                UpdateVisibility();
            }
        }

        private void UpdateVisibility()
        {
            var cell = CurrentAttachedCell;
            if (cell == null)
            {
                // no cell attached - but ask for update anyway
                // - this may update cached UIViews
                UpdateCellDisplay(cell);
                return;
            }

            var tableView = GetParentTableView(cell);
            if (tableView == null)
            {
                DialogTrace.WriteLine("How did this happen - CurrentAttachedCell is a child of a non-UITableView");
                // no parented cell attached - but ask for update anyway
                // - this may update cached UIViews
                UpdateCellDisplay(cell);
                return;
            }

            var indexPath = tableView.IndexPathForCell(cell);
            if (indexPath == null)
            {
                // Indexpath can sometimes be null when replacing content of a list by setting a new RootElement.
                // It's a really rare situation, only seen when you perform several actions on a listview and it's
                // busy animating stuff.
                // In this case just do the simple update
                UpdateCellDisplay(cell);
                return;
            }

            // we have a table and an indexPath - so let's do an animated update
            tableView.ReloadRows(
                new[] { indexPath },
                UITableViewRowAnimation.Fade);
        }

        private UITableView GetParentTableView(UITableViewCell cell)
        {
            if (cell.Superview == null)
                return null;

            // in IOS 6 SDK the superview is a UITableView
            var parent = cell.Superview as UITableView;
            if (parent != null)
                return parent;

            // in IOS 7 SDK the superview is a UITableViewWrapper,
            // and the UiTableView is now in superview.superview (http://stackoverflow.com/questions/15711645/how-to-get-uitableview-from-uitableviewcell)
            var grandParent = cell.Superview.Superview as UITableView;
            return grandParent;
        }

        private ICommand _selectedCommand;

        public ICommand SelectedCommand
        {
            get { return _selectedCommand; }
            set { _selectedCommand = value; }
        }

        /// <summary>
        /// Override this method if you want some other action to be taken when
        /// a cell view is set
        /// </summary>
        protected virtual void UpdateCellDisplay(UITableViewCell cell)
        {
            if (cell == null)
                return;
#warning SL _ removed  || !Parent.Visible
            // NOTE - SL removed  !Parent.Visible - it caused exception
            cell.Hidden = !Visible;
            UpdateCaptionDisplay(cell);
        }

        /// <summary>
        /// Override this method if you want some other action to be taken when
        /// the caption changes
        /// </summary>
        protected virtual void UpdateCaptionDisplay(UITableViewCell cell)
        {
            if (cell?.TextLabel != null)
                cell.TextLabel.Text = _caption;
        }

        /// <summary>
        ///  Initializes the element with the given caption.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        public Element(string caption)
        {
            this.Caption = caption;
        }

        public Element(string caption, Action tapped)
        {
            this.Caption = caption;
            Tapped += tapped;
        }

        ~Element()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private static readonly NSString Key = new NSString("xx");

        /// <summary>
        /// Subclasses that override the GetCellImpl method should override this method as well
        /// </summary>
        /// <remarks>
        /// This method should return the key passed to UITableView.DequeueReusableCell.
        /// If your code overrides the GetCellImpl method to change the cell, you must also
        /// override this method and return a unique key for it.
        ///
        /// This works in most subclasses with a couple of exceptions: StringElement and
        /// various derived classes do not use this setting as they need a wider range
        /// of keys for different uses, so you need to look at the source code for those
        /// if you are trying to override StringElement or StyledStringElement.
        /// </remarks>
        protected virtual NSString CellKey => Key;

        /// <summary>
        /// Gets a UITableViewCell for this element.
        /// Must not be overridden - override GetCellImpl instead
        /// </summary>
        public UITableViewCell GetCell(UITableView tv)
        {
            var cell = GetCellImpl(tv);
            CurrentAttachedCell = cell;
            UpdateCellDisplay(cell);
            return cell;
        }

        /// <summary>
        /// Gets a UITableViewCell for this element.   Can be overridden, but if you
        /// customize the style or contents of the cell you must also override the CellKey
        /// property in your derived class.
        /// </summary>
        protected virtual UITableViewCell GetCellImpl(UITableView tv)
        {
            return new UITableViewCell(UITableViewCellStyle.Default, CellKey);
        }

        /// <summary>
        /// Access to the current attached cell (view)
        /// Can be null
        /// </summary>
        protected UITableViewCell CurrentAttachedCell
        {
            get
            {
                if (_lastAttachedCell == null)
                    return null;

                if (_lastAttachedCell.Tag != _elementID)
                    _lastAttachedCell = null;

                return _lastAttachedCell;
            }
            private set
            {
                _lastAttachedCell = value;
                _lastAttachedCell.Tag = _elementID;
            }
        }

        /// <summary>
        /// Act on the current attached cell
        /// </summary>
        /// <param name="updateAction"></param>
        protected void ActOnCurrentAttachedCell(Action<UITableViewCell> updateAction)
        {
            var cell = CurrentAttachedCell;
            // note that we call the update action even if the attached cell is null
            // - as some elements use fixed UIViews (e.g. sliders) which are independent of the cell
            updateAction(cell);
        }

        protected static void RemoveTag(UITableViewCell cell, int tag)
        {
            var viewToRemove = cell.ContentView.ViewWithTag(tag);
            viewToRemove?.RemoveFromSuperview();
        }

        /// <summary>
        /// Returns a summary of the value represented by this object, suitable
        /// for rendering as the result of a RootElement with child objects.
        /// </summary>
        /// <returns>
        /// The return value must be a short description of the value.
        /// </returns>
        public virtual string Summary()
        {
            return "";
        }

        /// <summary>
        /// Invoked when the given element has been deslected by the user.
        /// </summary>
        /// <param name="dvc">
        /// The <see cref="DialogViewController"/> where the deselection took place
        /// </param>
        /// <param name="tableView">
        /// The <see cref="UITableView"/> that contains the element.
        /// </param>
        /// <param name="path">
        /// The <see cref="NSIndexPath"/> that contains the Section and Row for the element.
        /// </param>
        public virtual void Deselected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
        }

        /// <summary>
        /// Invoked when the given element has been selected by the user.
        /// </summary>
        /// <param name="dvc">
        /// The <see cref="DialogViewController"/> where the selection took place
        /// </param>
        /// <param name="tableView">
        /// The <see cref="UITableView"/> that contains the element.
        /// </param>
        /// <param name="path">
        /// The <see cref="NSIndexPath"/> that contains the Section and Row for the element.
        /// </param>
        public virtual void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            Tapped?.Invoke();
            SelectedCommand?.Execute(null);
            if (ShouldDeselectAfterTouch)
                tableView.DeselectRow(path, true);
        }

        /// <summary>
        /// Is anything registered to the select handler(s) for this Element
        /// </summary>
        protected bool IsSelectable => Tapped != null
                                       || (SelectedCommand != null && SelectedCommand.CanExecute(null));

        /// <summary>
        /// If the cell is attached will return the immediate RootElement
        /// </summary>
        public RootElement GetImmediateRootElement()
        {
            var section = Parent as Section;
            return section?.Parent as RootElement;
        }

        /// <summary>
        /// Returns the UITableView associated with this element, or null if this cell is not currently attached to a UITableView
        /// </summary>
        public UITableView GetContainerTableView()
        {
            var root = GetImmediateRootElement();
            return root?.TableView;
        }

        /// <summary>
        /// Returns the currently active UITableViewCell for this element, or null if the element is not currently visible
        /// </summary>
        public UITableViewCell GetActiveCell()
        {
            var tv = GetContainerTableView();
            if (tv == null)
                return null;
            var path = IndexPath;
            if (path == null)
                return null;
            return tv.CellAt(path);
        }

        /// <summary>
        ///  Returns the IndexPath of a given element.   This is only valid for leaf elements,
        ///  it does not work for a toplevel RootElement or a Section of if the Element has
        ///  not been attached yet.
        /// </summary>
        public NSIndexPath IndexPath
        {
            get
            {
                var section = Parent as Section;
                var root = section?.Parent as RootElement;
                if (root == null)
                    return null;

                int row = 0;
                foreach (var element in section.Elements)
                {
                    if (element == this)
                    {
                        int nsect = 0;
                        foreach (var sect in root.Sections)
                        {
                            if (section == sect)
                            {
                                return NSIndexPath.FromRowSection(row, nsect);
                            }
                            nsect++;
                        }
                    }
                    row++;
                }
                return null;
            }
        }

        /// <summary>
        ///   Method invoked to determine if the cell matches the given text, never invoked with a null value or an empty string.
        /// </summary>
        public virtual bool Matches(string text)
        {
            if (Caption == null)
                return false;
            return Caption.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1;
        }
    }
}