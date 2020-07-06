using System;
using System.Threading.Tasks;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Tizen.Views
{
    public class MvxTizenMainThreadDispatcher : MvxMainThreadDispatcher
    {
        public override bool IsOnMainThread => true;

        public override void ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            //TODO: implement
            ExceptionMaskedAction(action, maskExceptions);
            
        }

        public override ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            //TODO: implement
            return ExceptionMaskedActionAsync(action, maskExceptions);
        }
    }
}
