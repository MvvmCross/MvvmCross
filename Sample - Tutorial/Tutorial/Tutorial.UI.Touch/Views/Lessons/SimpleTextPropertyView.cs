using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels.Lessons;
using CrossUI.Touch.Dialog.Elements;

namespace Tutorial.UI.Touch.Views.Lessons
{
    public class SimpleTextPropertyView
         : MvxDialogViewController
    {
        public SimpleTextPropertyView() 
            : base(UITableViewStyle.Grouped, null, true)
        {
        }

		public new SimpleTextPropertyViewModel ViewModel {
			get { return (SimpleTextPropertyViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, null), false);
            this.NavigationItem.LeftBarButtonItem.Clicked += delegate
            {
				NavigationController.PopViewControllerAnimated(true);
            };

            this.Root = new RootElement("Simple Text Property")
                            {
                                new Section("Display")
                                    {
                                        new StringElement("Current").Bind(this, "Value TheText"),
                                        new StringElement("Length").Bind(this, "Value TheText, Converter=StringLength"),
                                        new StringElement("Reversed").Bind(this, "Value TheText,Converter=StringReverse"),
                                    },
                                new Section("Editing")
                                    {
                                        new EntryElement("Edit").Bind(this, "Value TheText"),
                                    },
                            };
        }
    }
}
