using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Touch.Dialog;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.Touch.Views.Lessons
{
    public class SimpleTextPropertyView
         : MvxTouchDialogViewController<SimpleTextPropertyViewModel>
    {
        public SimpleTextPropertyView(MvxShowViewModelRequest request) 
            : base(request, UITableViewStyle.Grouped, null, true)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, null), false);
            this.NavigationItem.LeftBarButtonItem.Clicked += delegate
            {
                ViewModel.BackCommand.Execute();
            };

            this.Root = new RootElement("Simple Text Property")
                            {
                                new Section("Display")
                                    {
                                        new StringElement("Current").Bind(this, "{'Value':{'Path':'TheText'}}"),
                                        new StringElement("Length").Bind(this, "{'Value':{'Path':'TheText','Converter':'StringLength'}}"),
                                        new StringElement("Reversed").Bind(this, "{'Value':{'Path':'TheText','Converter':'StringReverse'}}"),
                                    },
                                new Section("Editing")
                                    {
                                        new EntryElement("Edit").Bind(this, "{'Value':{'Path':'TheText'}}"),
                                    },
                            };
        }
    }
}
