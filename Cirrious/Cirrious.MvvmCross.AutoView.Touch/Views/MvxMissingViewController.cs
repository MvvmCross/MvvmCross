using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    [MvxUnconventionalView]
    public class MvxMissingViewController
        : MvxTouchDialogViewController<MvxViewModel>
        , IMvxTouchAutoView<MvxViewModel>
    {
        public MvxMissingViewController(MvxShowViewModelRequest request) : base(request)
        {
        }

        public void RegisterBinding(IMvxUpdateableBinding binding)
        {
            Bindings.Add(binding);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var description = this.ViewModel.CreateMissingDialogDescription();
            var root = this.LoadDialogRoot(description);
            Root = root;
        }
    }
}