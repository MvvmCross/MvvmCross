using System;
using System.Threading.Tasks;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Tizen.Views
{
    public class MvxTizenMainThreadDispatcher : MvxMainThreadAsyncDispatcher
    {
        public override bool IsOnMainThread => true;

        public override ValueTask<bool> RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            //TODO: implement
            ExceptionMaskedAction(action, maskExceptions);
            return new ValueTask<bool>(true);
        }
    }
}
