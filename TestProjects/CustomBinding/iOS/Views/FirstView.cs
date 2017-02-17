using System.Drawing;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.TestProjects.CustomBinding.Core.ViewModels;
using ObjCRuntime;
using UIKit;
using Foundation;

namespace MvvmCross.TestProjects.CustomBinding.iOS
{
    [Register("FirstView")]
    public class FirstView : MvxViewController
    {
        public override void ViewDidLoad()
        {
            View = new UIView(){ BackgroundColor = UIColor.White};
            base.ViewDidLoad();

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
            set.Apply();

            var tap = new UITapGestureRecognizer(() => textField.ResignFirstResponder());
            View.AddGestureRecognizer(tap);
        }
    }
}