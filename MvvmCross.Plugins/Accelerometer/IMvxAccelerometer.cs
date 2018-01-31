// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform.Core;

namespace MvvmCross.Plugins.Accelerometer
{
    public interface IMvxAccelerometer
    {
        void Start();

        void Stop();

        bool Started { get; }
        MvxAccelerometerReading LastReading { get; }

        event EventHandler<MvxValueEventArgs<MvxAccelerometerReading>> ReadingAvailable;
    }
}