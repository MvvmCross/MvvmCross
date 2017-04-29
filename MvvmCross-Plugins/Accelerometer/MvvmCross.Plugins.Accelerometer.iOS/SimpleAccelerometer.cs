// <copyright file="MvxAccelerometer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com - Hire me - I'm worth it!

using System;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using UIKit;

namespace MvvmCross.Plugins.Accelerometer.iOS
{
    [Preserve(AllMembers = true)]
    public class MvxAccelerometer
        : IMvxAccelerometer
    {
        public void Start()
        {
            if (Started)
                throw new MvxException("Accelerometer already started");

            Started = true;

            UIAccelerometer.SharedAccelerometer.UpdateInterval = 0.1;

            UIAccelerometer.SharedAccelerometer.Acceleration += HandleAccelerationChange;
        }

        public void Stop()
        {
            if (!Started)
                throw new MvxException("Accelerometer not started");

            Started = false;
            UIAccelerometer.SharedAccelerometer.Acceleration -= HandleAccelerationChange;
        }

        public bool Started { get; private set; }

        public MvxAccelerometerReading LastReading { get; private set; }

        public event EventHandler<MvxValueEventArgs<MvxAccelerometerReading>> ReadingAvailable;

        private void HandleAccelerationChange(object sender, UIAccelerometerEventArgs e)
        {
            var reading = ToReading(e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z);

            LastReading = reading.Clone();

            var handler = ReadingAvailable;

            handler?.Invoke(this, new MvxValueEventArgs<MvxAccelerometerReading>(reading));
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
    }
}