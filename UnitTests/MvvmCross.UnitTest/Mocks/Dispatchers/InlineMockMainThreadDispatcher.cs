﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    public class InlineMockMainThreadDispatcher
        : MvxMainThreadAsyncDispatcher
    {
        public override bool RequestMainThreadAction(Action action, 
            bool maskExceptions = true)
        {
            action();
            return true;
        }
    }
}
