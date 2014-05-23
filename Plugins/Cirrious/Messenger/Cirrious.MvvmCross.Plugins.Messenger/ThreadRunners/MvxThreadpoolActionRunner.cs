// MvxThreadPoolActionRunner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.Messenger.ThreadRunners
{
    public class MvxThreadPoolActionRunner
        : IMvxActionRunner
    {
        public void Run(Action action)
        {
            Task.Run(action);
        }
    }
}