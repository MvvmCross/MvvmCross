// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using UIKit;

namespace MvvmCross.Plugin.Accelerometer.Platforms.Ios
{
    [Preserve(AllMembers = true)]
    public class MvxAccelerometer
        : IMvxAccelerometer
    {
        private bool _initialized;

        public void Start()
        {
            if (_initialized)
            {
                throw new MvxException("Accelerometer already started");
            }

            _initialized = true;

            UIAccelerometer.SharedAccelerometer.UpdateInterval = 0.1;

            UIAccelerometer.SharedAccelerometer.Acceleration += HandleAccelerationChange;
        }

        private void HandleAccelerationChange(object sender, UIAccelerometerEventArgs e)
        {
            var reading = ToReading(e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z);

            LastReading = reading.Clone();

            var handler = ReadingAvailable;

            handler?.Invoke(this, new MvxValueEventArgs<MvxAccelerometerReading>(reading));
        }

        public void Stop()
        {
            if (!_initialized)
            {
                throw new MvxException("Accelerometer not started");
            }

            _initialized = false;
            UIAccelerometer.SharedAccelerometer.Acceleration -= HandleAccelerationChange;
        }

        private static MvxAccelerometerReading ToReading(double x, double y, double z)
        {
            var reading = new MvxAccelerometerReading
            {
                X = x,
                Y = y,
                Z = z
            };
            return reading;
        }

        public bool Started => _initialized;

        public MvxAccelerometerReading LastReading { get; private set; }

        public event EventHandler<MvxValueEventArgs<MvxAccelerometerReading>> ReadingAvailable;
    }
}
