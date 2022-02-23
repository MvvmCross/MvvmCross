// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using Windows.Devices.Sensors;

namespace MvvmCross.Plugin.Accelerometer.Platforms.Uap
{
    public class MvxWindowsAccelerometer : IMvxAccelerometer
    {
        private bool _started;
        private Windows.Devices.Sensors.Accelerometer _accelerometer;

        public void Start()
        {
            if (_started)
            {
                throw new MvxException("Accelerometer already started");
            }
            _accelerometer = Windows.Devices.Sensors.Accelerometer.GetDefault();
            if (_accelerometer != null)
            {
                _accelerometer.ReadingChanged += AccelerometerOnReadingChanged;
            }
            _started = true;
        }

        public void Stop()
        {
            if (!_started)
            {
                throw new MvxException("Accelerometer not started");
            }
            if (_accelerometer != null)
            {
                _accelerometer.ReadingChanged -= AccelerometerOnReadingChanged;
                _accelerometer = null;
            }
            _started = false;
        }

        private void AccelerometerOnReadingChanged(Windows.Devices.Sensors.Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            var handler = ReadingAvailable;

            if (handler == null)
                return;

            var reading = ToReading(args.Reading);

            handler(this, new MvxValueEventArgs<MvxAccelerometerReading>(reading));
        }

        private static MvxAccelerometerReading ToReading(AccelerometerReading sensorReading)
        {
            var reading = new MvxAccelerometerReading
            {
                X = sensorReading.AccelerationX,
                Y = sensorReading.AccelerationY,
                Z = sensorReading.AccelerationZ
            };
            return reading;
        }

        public bool Started => _accelerometer != null;

        public MvxAccelerometerReading LastReading
        {
            get
            {
                try
                {
                    var reading = ToReading(_accelerometer.GetCurrentReading());
                    return reading;
                }
                catch (Exception exception)
                {
                    throw exception.MvxWrap("Problem getting current Accelerometer reading");
                }
            }
        }

        public event EventHandler<MvxValueEventArgs<MvxAccelerometerReading>> ReadingAvailable;
    }
}
