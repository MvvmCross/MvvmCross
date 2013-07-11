// Element.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Android.Content;
using Android.Views;
using CrossUI.Core.Elements.Dialog;
using CrossUI.Droid.Dialog.Enums;
using Java.Lang;
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
        ///  Initializes the element with the given caption.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        public Element(string caption = null)
        {
            Caption = caption;
        }

        public Element(string caption, string layoutName)
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
            UpdateCaptionDisplay(cell);
        }

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
            var cell = GetViewImpl(context, convertView, parent);
            CurrentAttachedCell = cell;
            UpdateCellDisplay(cell);
            return cell;
        }

        /// <summary>
        /// Overriden by most derived classes, creates a View with the contents for display
        /// </summary>
        /// <param name="context"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected virtual View GetViewImpl(Context context, View convertView, ViewGroup parent)
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

        #endregion
    }
}