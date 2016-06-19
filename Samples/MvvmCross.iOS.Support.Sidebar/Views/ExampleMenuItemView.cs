using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Support.Core.ViewModels;
using MvvmCross.iOS.Support.iOS.Views;
using MvvmCross.iOS.Support.SidePanels;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MvvmCross.iOS.Support.Sidebar.Views
{
    [Register("ExampleMenuItemView")]
    [MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, true)]
    public class ExampleMenuItemView : BaseViewController<ExampleMenuItemViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label = new UILabel();

            var bindingSet = this.CreateBindingSet<ExampleMenuItemView, ExampleMenuItemViewModel>();
            bindingSet.Bind(label).To(vm => vm.ExampleValue);
            bindingSet.Apply();

            Add(label);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(

                label.WithSameHeight(View),
                label.WithSameCenterX(View),
                label.WithSameCenterY(View)

                );
        }
    }
}
