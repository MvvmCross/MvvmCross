using System;

using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Drawing;

namespace MonoCross.Touch
{
	class MGDetailViewController : UIViewController
	{
		UIPopoverController popoverController;
		UIToolbar toolbar;
		UIBarButtonItem toggleItem, verticalItem, dividerStyleItem, masterBeforeDetailItem;
		MGSplitViewController splitController;
		internal string DetailItem;
		
		public MGDetailViewController (MGSplitViewController splitController)
		{
			this.splitController = splitController;
			
			toolbar = new UIToolbar();
			toggleItem = new UIBarButtonItem();
			verticalItem = new UIBarButtonItem();
			dividerStyleItem = new UIBarButtonItem();
			masterBeforeDetailItem = new UIBarButtonItem();
		}
		
		
		internal void SetDetailItem(string newDetailItem)
		{
		    if (DetailItem != newDetailItem) {
		        DetailItem = newDetailItem;
		        
		        // Update the view.
		        ConfigureView();
		    }
		
		    if (popoverController != null) {
		        popoverController.Dismiss(true);
		    }        
		}
		
		
		internal void ConfigureView()
		{
		    // Update the user interface for the detail item.
//		    detailDescriptionLabel.text = [detailItem description];
			toggleItem.Title = splitController.IsShowingMaster() ? "Hide Master" : "Show Master";
			verticalItem.Title = splitController.Vertical ? "Horizontal Split" : "Vertical Split";
			dividerStyleItem.Title = (splitController.DividerStyle == MGSplitViewDividerStyle.Thin) ? "Enable Dragging" : "Disable Dragging";
			masterBeforeDetailItem.Title = (splitController.MasterBeforeDetail) ? "Detail First" : "Master First";
		}
		
		
		new void SplitViewController(MGSplitViewController svc, UIViewController viewController,
		                         UIBarButtonItem barButtonItem, UIPopoverController pc)
		{
			if (barButtonItem != null) {
				barButtonItem.Title = "Popover";
				UIBarButtonItem[] items = toolbar.Items;
				items[0] = barButtonItem;
				toolbar.SetItems(items, true);
			}
		    popoverController = pc;
		}
		
		
		// Called when the view is shown again in the split view, invalidating the button and popover controller.
		new void SplitViewController(MGSplitViewController svc, UIViewController viewController, UIBarButtonItem barButtonItem)
		{
			if (barButtonItem != null) {
				UIBarButtonItem[] items = toolbar.Items;
				
				for(int i = 0; i < items.Length; i++)
					if (items[i] == barButtonItem) items[i] = null;
				
				toolbar.SetItems(items, true);
			}
		    popoverController = null;
		}
		
		
		new void SplitViewController(MGSplitViewController svc, UIPopoverController pc, UIViewController viewController)
		{
		}
		
		
		new void SplitViewController(MGSplitViewController svc, bool isVertical)
		{
		}
		
		
		new void SplitViewController(MGSplitViewController svc, float position)
		{
		}
		
		
		new float SplitViewController(MGSplitViewController svc, float proposedPosition, SizeF viewSize)
		{
			return proposedPosition;
		}
		
		
//		- (IBAction)toggleMasterView:(id)sender
//		{
//			[splitController toggleMasterView:sender];
//			[self configureView];
//		}
//		
//		
//		- (IBAction)toggleVertical:(id)sender
//		{
//			[splitController toggleSplitOrientation:self];
//			[self configureView];
//		}
//		
//		
//		- (IBAction)toggleDividerStyle:(id)sender
//		{
//			MGSplitViewDividerStyle newStyle = ((splitController.dividerStyle == MGSplitViewDividerStyleThin) ? MGSplitViewDividerStylePaneSplitter : MGSplitViewDividerStyleThin);
//			[splitController setDividerStyle:newStyle animated:YES];
//			[self configureView];
//		}
//		
//		
//		- (IBAction)toggleMasterBeforeDetail:(id)sender
//		{
//			[splitController toggleMasterBeforeDetail:sender];
//			[self configureView];
//		}
		
		
		// Ensure that the view controller supports rotation and that the split view can therefore show in both portrait and landscape.
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		
		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			ConfigureView();
		}
	}
}

