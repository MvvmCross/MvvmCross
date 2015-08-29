﻿// MvxMainThreadActionRunner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.Messenger.ThreadRunners
{
    public class MvxMainThreadActionRunner
        : IMvxActionRunner
    {
        public void Run(Action action)
        {
            var dispatcher = MvxMainThreadDispatcher.Instance;
            if (dispatcher == null)
            {
                MvxTrace.Warning( "Not able to deliver message - no ui thread dispatcher available");
                return;
            }
            dispatcher.RequestMainThreadAction(action);
        }
    }
}