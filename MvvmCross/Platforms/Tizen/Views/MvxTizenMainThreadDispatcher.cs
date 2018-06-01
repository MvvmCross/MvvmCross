using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Base;
using Tizen.Applications;

namespace MvvmCross.Platforms.Tizen.Views
{
    public class MvxTizenMainThreadDispatcher : MvxMainThreadAsyncDispatcher
    {
        public override bool IsOnMainThread => true;

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            //TODO: implement
            action();
            return true;
        }
    }
}
