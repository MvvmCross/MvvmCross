// <copyright file="MvxAccelerometer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com - Hire me - I'm worth it!

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using UIKit;

namespace MvvmCross.Plugins.Accelerometer.Touch
{
    public class MvxAccelerometer
        :  IMvxAccelerometer
    {
		bool _initialized = false;

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

        void HandleAccelerationChange (object sender, UIAccelerometerEventArgs e)
        {
			var reading = ToReading(e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z);
			
			LastReading = reading.Clone();
			
			var handler = ReadingAvailable;
			
			if (handler == null)
				return;
			
			handler(this, new MvxValueEventArgs<MvxAccelerometerReading>(reading));
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
                    Z = z,
                };
            return reading;
        }

        public bool Started
        {
            get { return _initialized; }
        }

        public MvxAccelerometerReading LastReading { get; private set; }

        public event EventHandler<MvxValueEventArgs<MvxAccelerometerReading>> ReadingAvailable;
    }
}