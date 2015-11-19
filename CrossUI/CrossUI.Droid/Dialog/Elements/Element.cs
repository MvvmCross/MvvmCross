// Element.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Views;
using CrossUI.Core.Elements.Dialog;
using CrossUI.Droid.Dialog.Enums;
using Java.Lang;
using System;
using System.Windows.Input;
using Object = System.Object;

namespace CrossUI.Droid.Dialog.Elements
{
    public abstract class Element : Java.Lang.Object, IElement
    {
        private static int _currentElementID = 1;

        /// <summary>
        /// An app unique identifier for this element.
        /// Note that it is expected that Elements will always created on the UI thread - so no locking is used on CurrentElementID
        /// </summary>
        private readonly Java.Lang.Integer _elementID = new Integer(_currentElementID++);

        /// <summary>
        ///  Initializes the element with the given caption and layout.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="layoutName">
        /// The layout to load.
        /// </param>
        public Element(string caption = null, string layoutName = null)
        {
            Caption = caption;
            LayoutName = layoutName;
            Click = (s, e) =>
                {
                    if (SelectedCommand != null)
                    {
                        SelectedCommand.Execute(null);
                    }
                };
        }

        private string _caption;

        /// <summary>
        ///  The caption to display for this given element
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set
            {
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
                _visible = value;
                UpdateCellDisplay(CurrentAttachedCell);
            }
        }

        protected virtual void UpdateCaptionDisplay(View cell)
        {
            // by default do nothing!
        }

        private ICommand _selectedCommand;

        public ICommand SelectedCommand
        {
            get { return _selectedCommand; }
            set { _selectedCommand = value; }
        }

        public string LayoutName { get; set; }

        /// <summary>
        /// Override this method if you want some other action to be taken when
        /// a cell view is set
        /// </summary>
        protected virtual void UpdateCellDisplay(View cell)
        {
            if (cell == null)
                return;

            cell.Clickable = Clickable;
            cell.LongClickable = LongClickable;

#warning Visible with parent not fully implemented across Sections and RootElements currently - if a section changes visibility then the children are not informed?
#warning Visible not currently completely consistent with iOS Dialogs?
#warning SL _ removed  && Parent.Visible
            cell.Visibility = Visible ? ViewStates.Visible : ViewStates.Gone;
            UpdateCaptionDisplay(cell);
        }

        /// <summary>
        /// What to set the cell's clickable property to (defaults to true)
        /// </summary>
        public virtual bool Clickable { get; set; }

        /// <summary>
        /// What to set the cell's longclickable property to (defaults to true)
        /// </summary>
        public virtual bool LongClickable { get; set; }

        /// <summary>
        ///  Handle to the container object.
        /// </summary>
        /// <remarks>
        /// For sections this points to a RootElement, for every other object this points to a Section and it is null
        /// for the root RootElement.
        /// </remarks>
        public Element Parent { get; set; }

        /// <summary>
        /// Override for click the click event
        /// </summary>
        public EventHandler Click { get; set; }

        /// <summary>
        /// Override for long click events, some elements use this for action
        /// </summary>
        public EventHandler LongClick { get; set; }

        /// <summary>
        /// Alternative alias to the click event, naming more like MonoTouch Dialog.
        /// </summary>
        public EventHandler Tapped
        {
            get { return Click; }
            set { Click = value; }
        }

        /// <summary>
        /// An Object that contains data about the element. The default is null.
        /// </summary>
        public Object Tag { get; set; }

        /// <summary>
        /// Returns a summary of the value represented by this object, suitable
        /// for rendering as the result of a RootElement with child objects.
        /// </summary>
        /// <returns>
        /// The return value must be a short description of the value.
        /// </returns>
        public virtual string Summary()
        {
            return string.Empty;
        }

        protected Context Context { get; private set; }

        public View GetView(Context context, View convertView, ViewGroup parent)
        {
            Context = context;

            if (CurrentAttachedCell == null || convertView != CurrentAttachedCell)
            {
                //we only get a new cell if convertview is not the currentattachecell.
                //this for example is the case when you replace an element
                CurrentAttachedCell = GetViewImpl(context, parent);
            }

            UpdateCellDisplay(CurrentAttachedCell);
            return CurrentAttachedCell;
        }

        /// <summary>
        /// Overriden by most derived classes, creates a View with the contents for display
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected virtual View GetViewImpl(Context context, ViewGroup parent)
        {
            throw new NotImplementedException("GetViewImpl should be implemented in derived Element classes");
        }

        /// <summary>
        /// The last cell attached to this Element
        /// Use the Tag property of the Cell to determine of this cell is still attached to this Element
        /// </summary>
        private View _lastAttachedCell;

        protected View CurrentAttachedCell
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
                if (_lastAttachedCell != null)
                    _lastAttachedCell.Tag = _elementID;
            }
        }

        public virtual void Selected()
        {
        }

        public virtual bool Matches(string text)
        {
            return Caption != null && Caption.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        #region MonoTouch Dialog Mimicry

        // Not used in any way, just there to match MT Dialog api.
        public UITableViewCellAccessory Accessory
        {
            get { return accessory; }
            set { accessory = value; }
        }

        private UITableViewCellAccessory accessory;

        public void ActOnCurrentAttachedCell(Action<View> updateAction)
        {
            var cell = CurrentAttachedCell;
            // note that we call the update action even if the attached cell is null
            // - as some elements use fixed UIViews (e.g. sliders) which are independent of the cell
            updateAction(cell);
        }

        #endregion MonoTouch Dialog Mimicry
    }
}