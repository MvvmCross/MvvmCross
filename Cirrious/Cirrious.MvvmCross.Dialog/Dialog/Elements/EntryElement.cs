using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class EntryElement : ValueElement<string>
    {
        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (_entry != null)
            {
                if (_entry.Text != Value)
                    _entry.Text = Value;
            }
        }

        /// <summary>
        /// The key used for reusable UITableViewCells.
        /// </summary>
        static readonly NSString entryKey = new NSString ("EntryElement");
        protected virtual NSString EntryKey {
            get {
                return entryKey;
            }
        }

        UIKeyboardType keyboardType = UIKeyboardType.Default;

        /// <summary>
        /// The type of keyboard used for input, you can change
        /// this to use this for numeric input, email addressed,
        /// urls, phones.
        /// </summary>
        public UIKeyboardType KeyboardType {
            get {
                return keyboardType;
            }
            set {
                keyboardType = value;
                if (_entry != null)
                    _entry.KeyboardType = value;
            }
        }

        UIReturnKeyType? returnKeyType = null;
        /// <summary>
        /// The type of Return Key that is displayed on the
        /// keyboard, you can change this to use this for
        /// Done, Return, Save, etc. keys on the keyboard
        /// </summary>
        public UIReturnKeyType? ReturnKeyType
        {
            get {
                return returnKeyType;
            }
            set {
                returnKeyType = value;
                if (_entry != null && returnKeyType.HasValue)
                    _entry.ReturnKeyType = returnKeyType.Value;
            }
        }

        UITextAutocapitalizationType autocapitalizationType = UITextAutocapitalizationType.Sentences;
        public UITextAutocapitalizationType AutocapitalizationType
        {
            get {
                return autocapitalizationType;	
            }
            set { 
                autocapitalizationType = value;
                if (_entry != null)
                    _entry.AutocapitalizationType = value;
            }
        }

        UITextAutocorrectionType autocorrectionType = UITextAutocorrectionType.Default;
        public UITextAutocorrectionType AutocorrectionType
        { 
            get { 
                return autocorrectionType;
            }
            set { 
                autocorrectionType = value;
                if (_entry != null)
                    this.autocorrectionType = value;
            }
        }

        readonly bool isPassword;
        bool _becomeResponder;
        UITextField _entry;
        readonly string placeholder;
        static UIFont DefaultFont = UIFont.BoldSystemFontOfSize (17);

        public event EventHandler Changed;
        public event Func<bool> ShouldReturn;

        /// <summary>
        /// Constructs an EntryElement with the given caption, placeholder and initial value.
        /// </summary>
        /// <param name="caption">
        /// The caption to use
        /// </param>
        public EntryElement(string caption)
            : this(caption, string.Empty, string.Empty, false)
        {
        }

        /// <summary>
        /// Constructs an EntryElement with the given caption, placeholder and initial value.
        /// </summary>
        /// <param name="caption">
        /// The caption to use
        /// </param>
        /// <param name="placeholder">
        /// Placeholder to display when no value is set.
        /// </param>
        public EntryElement(string caption, string placeholder)
            : this(caption, placeholder, string.Empty, false)
        {
        }

        /// <summary>
        /// Constructs an EntryElement with the given caption, placeholder and initial value.
        /// </summary>
        /// <param name="caption">
        /// The caption to use
        /// </param>
        /// <param name="placeholder">
        /// Placeholder to display when no value is set.
        /// </param>
        /// <param name="value">
        /// Initial value.
        /// </param>
        public EntryElement (string caption, string placeholder, string value) 
            : this (caption, placeholder, value, false)
        { 
        }
        
        /// <summary>
        /// Constructs an EntryElement for password entry with the given caption, placeholder and initial value.
        /// </summary>
        /// <param name="caption">
        /// The caption to use.
        /// </param>
        /// <param name="placeholder">
        /// Placeholder to display when no value is set.
        /// </param>
        /// <param name="value">
        /// Initial value.
        /// </param>
        /// <param name="isPassword">
        /// True if this should be used to enter a password.
        /// </param>
        public EntryElement (string caption, string placeholder, string value, bool isPassword) : base (caption)
        {
            Value = value;
            this.isPassword = isPassword;
            this.placeholder = placeholder;
        }

        public override string Summary ()
        {
            return Value;
        }

        // 
        // Computes the X position for the entry by aligning all the entries in the Section
        //
        SizeF ComputeEntryPosition (UITableView tv, UITableViewCell cell)
        {
            Section s = Parent as Section;
            if (s.EntryAlignment.Width != 0)
                return s.EntryAlignment;
            
            // If all EntryElements have a null Caption, align UITextField with the Caption
            // offset of normal cells (at 10px).
            SizeF max = new SizeF (-15, tv.StringSize ("M", DefaultFont).Height);
            foreach (var e in s.Elements){
                var ee = e as EntryElement;
                if (ee == null)
                    continue;
                
                if (ee.Caption != null) {
                    var size = tv.StringSize (ee.Caption, DefaultFont);
                    if (size.Width > max.Width)
                        max = size;
                }
            }
            s.EntryAlignment = new SizeF (25 + Math.Min (max.Width, 160), max.Height);
            return s.EntryAlignment;
        }

        protected virtual UITextField CreateTextField (RectangleF frame)
        {
            return new UITextField (frame) {
                                               AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleLeftMargin,
                                               Placeholder = placeholder ?? "",
                                               SecureTextEntry = isPassword,
                                               Text = Value ?? "",
                                               Tag = 1
                                           };
        }
        
        static readonly NSString cellkey = new NSString ("EntryElement");
        
        protected override NSString CellKey {
            get {
                return cellkey;
            }
        }
        
        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            var cell = tv.DequeueReusableCell (CellKey);
            if (cell == null){
                cell = new UITableViewCell (UITableViewCellStyle.Default, CellKey);
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            } else 
                RemoveTag (cell, 1);
            
            if (_entry == null){
                SizeF size = ComputeEntryPosition (tv, cell);
                float yOffset = (cell.ContentView.Bounds.Height - size.Height) / 2 - 1;
                float width = cell.ContentView.Bounds.Width - size.Width;
                
                _entry = CreateTextField (new RectangleF (size.Width, yOffset, width, size.Height));
                
                _entry.ValueChanged += delegate {
                                                   FetchAndUpdateValue ();
                };
                _entry.EditingChanged += delegate {
                                                     FetchAndUpdateValue ();					
                };
                _entry.Ended += delegate {
                                            FetchAndUpdateValue ();
                };
                _entry.ShouldReturn += delegate {
                    
                                                   if (ShouldReturn != null)
                                                       return ShouldReturn();
                    
                                                   RootElement root = GetImmediateRootElement ();
                                                   EntryElement focus = null;
                    
                                                   if (root == null)
                                                       return true;
                    
                                                   foreach (var s in root.Sections) {
                                                       foreach (var e in s.Elements) {
                                                           if (e == this) {
                                                               focus = this;
                                                           } else if (focus != null && e is EntryElement) {
                                                               focus = e as EntryElement;
                                                               break;
                                                           }
                                                       }
                        
                                                       if (focus != null && focus != this)
                                                           break;
                                                   }

                                                   if (focus != null)
                                                   {
                                                       if (focus != this)
                                                           focus.BecomeFirstResponder(true);
                                                       else
                                                           focus.ResignFirstResponder(true);
                                                   }

                                                   return true;
                };
                _entry.Started += delegate {
                                              EntryElement self = null;
                    
                                              if (!returnKeyType.HasValue) {
                                                  var returnType = UIReturnKeyType.Default;
                        
                                                  foreach (var e in (Parent as Section).Elements){
                                                      if (e == this)
                                                          self = this;
                                                      else if (self != null && e is EntryElement)
                                                          returnType = UIReturnKeyType.Next;
                                                  }
                                                  _entry.ReturnKeyType = returnType;
                                              } else
                                                  _entry.ReturnKeyType = returnKeyType.Value;

                                              tv.ScrollToRow (IndexPath, UITableViewScrollPosition.Middle, true);
                };
            }
            if (_becomeResponder){
                _entry.BecomeFirstResponder ();
                _becomeResponder = false;
            }
            _entry.KeyboardType = KeyboardType;
            
            _entry.AutocapitalizationType = AutocapitalizationType;
            _entry.AutocorrectionType = AutocorrectionType;
            
            cell.TextLabel.Text = Caption;
            cell.ContentView.AddSubview (_entry);
            return cell;
        }
        
        /// <summary>
        ///  Copies the value from the UITextField in the EntryElement to the
        //   Value property and raises the Changed event if necessary.
        /// </summary>
        public void FetchAndUpdateValue ()
        {
            if (_entry == null)
                return;

            var newValue = _entry.Text;
            if (newValue == Value)
                return;
            
            OnUserValueChanged(newValue);
            
            if (Changed != null)
                Changed (this, EventArgs.Empty);
        }
        
        protected override void Dispose (bool disposing)
        {
            if (disposing){
                if (_entry != null){
                    _entry.Dispose ();
                    _entry = null;
                }
            }
        }

        public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
        {
            BecomeFirstResponder(true);
            tableView.DeselectRow (indexPath, true);
        }
        
        public override bool Matches (string text)
        {
            return (Value != null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1: false) || base.Matches (text);
        }
        
        /// <summary>
        /// Makes this cell the first responder (get the focus)
        /// </summary>
        /// <param name="animated">
        /// Whether scrolling to the location of this cell should be animated
        /// </param>
        public void BecomeFirstResponder (bool animated)
        {
            _becomeResponder = true;
            var tv = GetContainerTableView ();
            if (tv == null)
                return;
            tv.ScrollToRow (IndexPath, UITableViewScrollPosition.Middle, animated);
            if (_entry != null){
                _entry.BecomeFirstResponder ();
                _becomeResponder = false;
            }
        }

        public void ResignFirstResponder (bool animated)
        {
            _becomeResponder = false;
            var tv = GetContainerTableView ();
            if (tv == null)
                return;
            tv.ScrollToRow (IndexPath, UITableViewScrollPosition.Middle, animated);
            if (_entry != null)
                _entry.ResignFirstResponder ();
        }
    }
}