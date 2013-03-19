// MvxMainThreadActionRunner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.Messenger.ThreadRunners
{
    public class MvxMainThreadActionRunner
        : IMvxActionRunner
    {
        private readonly IMvxMainThreadDispatcherProvider _dispatcherProvider;

        public MvxMainThreadActionRunner()
        {
            _dispatcherProvider = Mvx.Resolve<IMvxMainThreadDispatcherProvider>();
        }

        public void Run(Action action)
        {
            var dispatcher = _dispatcherProvider.Dispatcher;
            if (dispatcher == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Not able to deliver message - no ui thread dispatcher available");
                return;
            }
            dispatcher.RequestMainThreadAction(action);
        }
    }
}