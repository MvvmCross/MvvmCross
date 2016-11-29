using System;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Support.Views.Expandable;
using UIKit;

namespace MvvmCross.iOS.Support.ExpandableTableView.iOS
{
	public partial class HeaderCell : MvxTableViewCell, IExpandableHeaderCell
	{
		private const string BindingText = "Title Title";
		public static readonly NSString Key = new NSString("HeaderCell");
		public static readonly UINib Nib;

		static HeaderCell()
		{
			Nib = UINib.FromName("HeaderCell", NSBundle.MainBundle);
		}

		public HeaderCell()
			: base(BindingText)
		{
		}

		public HeaderCell(IntPtr handle)
			: base(BindingText, handle)
		{
		}

		public string Title
		{
			get { return MainLabel.Text; }
			set { MainLabel.Text = value; }
		}

		public void OnExpanded()
		{
			ContentView.BackgroundColor = UIColor.Blue;
		}

		public void OnCollapsed()
		{
			ContentView.BackgroundColor = UIColor.Green;
		}
	}
}