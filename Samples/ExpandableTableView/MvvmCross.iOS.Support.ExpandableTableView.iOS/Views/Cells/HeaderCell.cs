using System;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace MvvmCross.iOS.Support.ExpandableTableView.iOS
{
	public partial class HeaderCell : MvxTableViewCell
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
	}
}