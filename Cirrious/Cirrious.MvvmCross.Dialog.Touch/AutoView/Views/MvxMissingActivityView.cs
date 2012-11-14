using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    [MvxUnconventionalView]
    public class MvxMissingViewController
        : MvxBindingTouchViewController<MvxViewModel>
          , IMvxTouchAutoView<MvxViewModel>
    {
        protected MvxMissingViewController(MvxShowViewModelRequest request) : base(request)
        {
        }

        protected MvxMissingViewController(MvxShowViewModelRequest request, string nibName, NSBundle bundle) : base(request, nibName, bundle)
        {
        }

        public void RegisterBinding(IMvxUpdateableBinding binding)
        {
#warning            // TODO - what to do with these bindings !
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

#warning // TODO - something vaguely useful
#warning // TODO - something vaguely useful
#warning // TODO - something vaguely useful
#warning // TODO - something vaguely useful
#warning // TODO - something vaguely useful
        }
    }
}