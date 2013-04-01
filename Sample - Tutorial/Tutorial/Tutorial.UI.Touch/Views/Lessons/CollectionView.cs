using System;
using Cirrious.MvvmCross.Dialog.Touch;
using Tutorial.Core.ViewModels.Lessons;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using CrossUI.Touch.Dialog.Elements;
using Tutorial.UI.Touch.Dialog;

namespace Tutorial.UI.Touch.Views.Lessons
{
	public class CollectionView
		: MvxTouchDialogViewController<CollectionViewModel>
	{
		public CollectionView(MvxShowViewModelRequest request) 
			: base(request, UITableViewStyle.Grouped, null, true)
		{
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, null), false);
			this.NavigationItem.LeftBarButtonItem.Clicked += delegate
			{
				ViewModel.DoClose();
			};

			var addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => {
				ViewModel.AddCommand.Execute(null);
			});
			var removeButton = new UIBarButtonItem("-", UIBarButtonItemStyle.Plain, (s, e) => {
				ViewModel.RemoveRandom.Execute(null);
			});
			this.NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[]{ addButton, removeButton}, false);

			this.Root = new RootElement("Observable Collection")
			{
				new SectionEx<string>(ViewModel.Items, (item) => new StringElement(item))
			};
		}
	}
}

