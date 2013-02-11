using System;
using CrossUI.Touch.Dialog.Elements;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using CrossUI.Touch.Dialog;

namespace StopDumping.UI.Touch.Extensions
{
	/// <summary>
	/// Multiline entry element.
	/// </summary>
	public class MultilineEntryElement : EntryElement, IElementSizing
	{
		private bool _becomeResponder;
		private readonly string _placeholder;

		/// <summary>
		/// Constructs an MultilineEntryElement with the given placeholder.
		/// </summary>
		/// <param name="placeholder">
		/// Placeholder to display when no value is set.
		/// </param>
		public MultilineEntryElement(string placeholder)
			:base()
		{
			this._placeholder = placeholder;
		}

		public event EventHandler Changed;
		/// <summary>
		/// LostFocus essentially
		/// </summary>
		public event EventHandler Ended;
		
		public override string Summary()
		{
			return Value;
		}
		
		private static readonly NSString cellkey = new NSString("MultilineEntryElement");
		
		protected override NSString CellKey
		{
			get { return cellkey; }
		}
		
		protected override UITableViewCell GetCellImpl(UITableView tv)
		{
			var cell = tv.DequeueReusableCell(CellKey) as MultilineEntryCell;
			if (cell == null)
			{
				cell = new MultilineEntryCell(CellKey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;

				cell.Entry.Changed += delegate { FetchAndUpdateValue(); };

				cell.Entry.Ended += delegate {
					FetchAndUpdateValue();

					var f = Ended;
					if (f != null)
						f(this, null);
				};
			}

			cell.Update("_placeholder", Value);

			if (_becomeResponder)
			{
				cell.BecomeFirstResponder();
				_becomeResponder = false;
			}

			return cell;
		}

		protected override void UpdateDetailDisplay(UITableViewCell cell)
		{
			if (cell != null)
				((MultilineEntryCell)cell).Update(_placeholder, Value);
		}
		
		/// <summary>
		///  Copies the value from the UITextField in the EntryElement to the
		//   Value property and raises the Changed event if necessary.
		/// </summary>
		public void FetchAndUpdateValue()
		{
			var cell = (MultilineEntryCell)this.GetActiveCell();

			if (cell == null)
				return;
			
			var newValue = cell.Entry.Text;
			if (newValue == Value)
				return;
			
			OnUserValueChanged(newValue);

			if (Changed != null)
				Changed(this, EventArgs.Empty);
		}
		
		public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			BecomeFirstResponder(true);
			tableView.DeselectRow(indexPath, true);
		}
		
		public override bool Matches(string text)
		{
			return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) ||
				base.Matches(text);
		}
		
		/// <summary>
		/// Makes this cell the first responder (get the focus)
		/// </summary>
		/// <param name="animated">
		/// Whether scrolling to the location of this cell should be animated
		/// </param>
		public override void BecomeFirstResponder(bool animated)
		{
			_becomeResponder = true;

			var tv = GetContainerTableView();
			if (tv == null)
				return;
			tv.ScrollToRow(IndexPath, UITableViewScrollPosition.Middle, animated);

			//After scrolling, become first responder. If you try do this before the scroll doesn't work.
			tv.ScrollAnimationEnded += BecomeFirstResponderDelayed;
		}

		private void BecomeFirstResponderDelayed(object sender, EventArgs args)
		{
			var cell = (MultilineEntryCell)this.GetActiveCell();
			if (cell != null)
			{
				cell.BecomeFirstResponder();
				_becomeResponder = false;
			}

			GetContainerTableView().ScrollAnimationEnded -= BecomeFirstResponderDelayed;
		}
		
		public void ResignFirstResponder(bool animated)
		{
			_becomeResponder = false;

			var tv = GetContainerTableView();
			if (tv == null)
				return;
			tv.ScrollToRow(IndexPath, UITableViewScrollPosition.Middle, animated);

			var cell = (MultilineEntryCell)this.GetActiveCell();
			if (cell != null)
				cell.ResignFirstResponder();
		}

		public bool IsFirstResponder
		{
			get
			{
				var cell = (MultilineEntryCell)this.GetActiveCell();
				return cell != null ? cell.IsFirstResponder : false;
			}
		}
		
		public virtual float GetHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return 114f;
		}

		/// <summary>
		/// Cell used to store the Multiline Entry and Placeholder.
		/// </summary>
		private class MultilineEntryCell : UITableViewCell
		{
			public const float Height = 114f;
			
			public static UIFont Font = UIFont.SystemFontOfSize (15);
			
			private UIView _view;
			private UILabel _placeholderLabel;
			private string _placeholder = null;
			
			public MultilineEntryCell(NSString key)
				: base (UITableViewCellStyle.Default, key)
			{
				_view = new UIView(new RectangleF (0, 0, 1, 1));
				
				_placeholderLabel = new UILabel(new RectangleF(8f, 1f, 290f, 37f))
				{
					TextColor = UIColor.LightGray,
					Font = Font,
					BackgroundColor = UIColor.Clear
				};
				_view.AddSubview(_placeholderLabel);
				
				Entry = new UITextView(new RectangleF(0f, 2f, 290f, 108f))
				{
					AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleLeftMargin,
					Font = Font,
					BackgroundColor = UIColor.Clear,
					TextColor = UIColor.FromRGB(25, 75, 127)
				};

				Entry.Changed += delegate {
					if(Entry.Text.Length == 0)
						_placeholderLabel.Text = _placeholder;
					else
						_placeholderLabel.Text = "";
				};
				
				_view.AddSubview(Entry);
				
				ContentView.AddSubview(_view);
			}

			public UITextView Entry { get; private set; }
			
			public override void LayoutSubviews()
			{
				base.LayoutSubviews();
				
				Entry.Frame = new RectangleF(0f, 2f, 290f, 108f);
				_view.Frame = new RectangleF(new PointF(3f, 0), ContentView.Frame.Size);
				_view.SetNeedsLayout();
			}
			
			public void Update(string placeholder, string value)
			{
				_placeholder = placeholder;
				_placeholderLabel.Text = string.IsNullOrEmpty(value) ? placeholder : "";
				Entry.Text = value;
				SetNeedsDisplay();
			}

			public override bool BecomeFirstResponder()
			{
				return Entry.BecomeFirstResponder();
			}
			
			public override bool ResignFirstResponder()
			{
				return Entry.ResignFirstResponder();
			}
			
			public bool IsFirstResponder
			{
				get
				{
					return Entry.IsFirstResponder;
				}
			}
		}
	}
}

