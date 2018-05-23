using System.Drawing;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.TestProjects.CustomBinding.Core.ViewModels;
using MvvmCross.TestProjects.CustomBinding.iOS.Controls;
using ObjCRuntime;
using UIKit;

namespace MvvmCross.TestProjects.CustomBinding.iOS.Views
{
    [Register("FirstView")]
    [MvxRootPresentation]
    public class FirstView : MvxViewController
    {
        private UIDatePicker _datePicker;

        public override void ViewDidLoad()
        {
            View = new UIView() { BackgroundColor = UIColor.White };
            base.ViewDidLoad();

            _datePicker = new UIDatePicker();
            View.AddSubview(_datePicker);

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;

            var binaryEdit = new BinaryEdit(new RectangleF(10, 70, 300, 120));
            Add(binaryEdit);
            var textField = new UITextField(new RectangleF(10, 190, 300, 40));
            Add(textField);

            var set = this.CreateBindingSet<FirstView, FirstViewModel>();
            set.Bind(binaryEdit).For("MyCount").To(vm => vm.Counter);
            set.Bind(textField).To(vm => vm.Counter);
            set.Bind(_datePicker).For(v => v.Date).To(vm => vm.Date);
            set.Apply();

            var tap = new UITapGestureRecognizer(() => textField.ResignFirstResponder());
            View.AddGestureRecognizer(tap);
        }
    }
}