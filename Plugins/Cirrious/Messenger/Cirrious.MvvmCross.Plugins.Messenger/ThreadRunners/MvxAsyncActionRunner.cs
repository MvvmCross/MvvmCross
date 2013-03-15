// MvxAsyncActionRunner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;

namespace Cirrious.MvvmCross.Plugins.Messenger.ThreadRunners
{
    public class MvxAsyncActionRunner
        : IMvxActionRunner
    {
        public void Run(Action action)
        {
            ThreadPool.QueueUserWorkItem(ignored => action());
        }
    }
}