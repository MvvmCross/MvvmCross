﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace MvvmCross.Core
{
    public interface IMvxSetupMonitor
    {
        Task InitializationComplete(Exception inititializationException);
    }
}
