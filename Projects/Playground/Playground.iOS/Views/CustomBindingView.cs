using System;
using System.Drawing;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using ObjCRuntime;
using Playground.Core.ViewModels.Bindings;
using Playground.iOS.Controls;
using UIKit;

namespace Playground.iOS.Views
{

    [MvxFromStoryboard("Main")]
    [MvxChildPresentation]
    public partial class CustomBindingView : MvxViewController<CustomBindingViewModel>
    {
        private UIDatePicker _datePicker;

        public CustomBindingView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            View = new UIView
            {
                BackgroundColor = UIColor.White
            };
            base.ViewDidLoad();

            _datePicker = new UIDatePicker();
            View.AddSubview(_datePicker);

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
                EdgesForExtendedLayout = UIRectEdge.None;
            }

            var binaryEdit = new BinaryEdit(new RectangleF(10, 70, 300, 120));
            Add(binaryEdit);
            var textField = new UITextField(new RectangleF(10, 190, 300, 40));
            Add(textField);

            var set = this.CreateBindingSet<CustomBindingView, CustomBindingViewModel>();
            set.Bind(binaryEdit).For("MyCount").To(vm => vm.Counter);
            set.Bind(textField).To(vm => vm.Counter);
            set.Bind(_datePicker).For(v => v.Date).To(vm => vm.Date);
            set.Apply();

            var tap = new UITapGestureRecognizer(() => textField.ResignFirstResponder());
            View.AddGestureRecognizer(tap);
        }
    }
}

