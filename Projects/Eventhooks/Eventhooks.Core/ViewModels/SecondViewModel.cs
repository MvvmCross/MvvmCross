using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;

namespace Eventhooks.Core.ViewModels
{
    public class SecondViewModel : MvxViewModel
    {
        public override void ViewCreated()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "2nd View is created");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "2nd View is destroyed");
        }

        public override void ViewAppearing()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "2nd View is appearing");
        }

        public override void ViewAppeared()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "2nd View appeared");
        }

        public override void ViewDisappearing()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "2nd View is disappearing");
        }

        public override void ViewDisappeared()
        {
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "2nd View has disappeared");
        }

        public IMvxCommand CloseCommand
        {
            get { return new MvxCommand(ExecuteCloseCommand); }
        }

        private void ExecuteCloseCommand()
        {
            Close(this);
        }
    }
}
