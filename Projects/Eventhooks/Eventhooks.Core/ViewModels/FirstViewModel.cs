using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;

namespace Eventhooks.Core.ViewModels
{
    public class FirstViewModel
        : MvxViewModel
    {
        public override void ViewCreated()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View is created");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View is destroyed");
        }

        public override void ViewAppearing()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View is appearing");
        }

        public override void ViewAppeared()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View appeared");
        }

        public override void ViewDisappearing()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View is disappearing");
        }

        public override void ViewDisappeared()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View has disappeared");
        }

        public IMvxCommand ShowSecondView
        {
            get { return new MvxCommand(ExecuteSecondViewCommand); }
        }

        private void ExecuteSecondViewCommand()
        {
            ShowViewModel<SecondViewModel>();
        }
    }
}
